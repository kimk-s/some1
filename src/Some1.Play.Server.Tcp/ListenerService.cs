using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Some1.Auth.Admin;
using Some1.Net;
using Some1.Play.Core;

namespace Some1.Play.Server.Tcp
{
    internal class ListenerService : BackgroundService
    {
        public const string KeyXML = "<RSAKeyValue><Modulus>9v+EWy8svizL1Wi707Ekp+2D/sLpUi8eOd4XrjRM+kJLPGMk7uQFwTqMxa7wWdgJKvXYxvJCiCDtVOSGqXG4J5m6JKah8VSCw5Jg05heUZ/ETvMaN2qNaqhNWaA4+dxLPEDUoqdua1amxSvzHvN9ez2NUH62JztufFvM4kbjOxU=</Modulus><Exponent>AQAB</Exponent><P>+O4/OxbxThSr+IsWXf4pU+1XpIWT96AIM7AgRIcgsw8Tf9LIK6X88FLj1gtapD/8lUJPt8atpxO26VJUtbDVbw==</P><Q>/gM4Ymqgti66dvAV8IRtONCJvzIoeUDbchjVY74rBbsW48LUwQj4hdbH5n8JnkFDCDMxvWrHWTKxs10htrJduw==</Q><DP>q+eLjvjXBz7LS8ZxWdONIsJCxgDhIB5Jy7gTcH+Im18L7jfXuBzWwffcExKgM9FkUocKmjT/8uNwa0xJ53cIzQ==</DP><DQ>iwvyOpWJ4hCUS/VC3UVwkJA/RyVK2I0zUzLa5N29qUZv0j/dvMmPWZxoEvnppvKsofl8OecQtvg0JC5P/TWIbw==</DQ><InverseQ>N0v+28K4UtDRqiSK5anRNLglNRJ3PXf8UI5wE/uVPqbCG727dVP9ihsmGUOEO0EnTt5QV947DWUgonrtPsoVQw==</InverseQ><D>toF9IgdMj0T4ZDscNkJ5LGATHSaWUnSgZ+/UHZNlobFkeD1l5+Ky4eFKbTNjQ2+e7pBz2iTdxkpWM1kcq41snNqYX0sUEXlxKHeFq8OmSae4IzJEeUuNEMFqY5euBekUEMgjLi+cExSKO0YX44KtDb2wefm1hCCz70tTCQONgM0=</D></RSAKeyValue>";

        private readonly ILogger<ListenerService> _logger;
        private readonly IOptions<ListenerOptions> _options;
        private readonly IPlayCore _play;
        private readonly DuplexPipeProcessor _processor;
        private readonly IAuthAdmin _authAdmin;

        public ListenerService(
            ILogger<ListenerService> logger,
            IOptions<ListenerOptions> options,
            IPlayCore play,
            DuplexPipeProcessor processor,
            IAuthAdmin authTokenVerifier)
        {
            _logger = logger;
            _options = options;
            _play = play;
            _processor = processor;
            _authAdmin = authTokenVerifier;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using Socket listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true
            };
            listener.Bind(new IPEndPoint(IPAddress.Any, _options.Value.Port));
            listener.Listen();
            
            _logger.LogInformation($"Listen {listener.LocalEndPoint}");
            
            try
            {
                while (true)
                {
                    Socket accepted = await AcceptAsync(listener, stoppingToken).ConfigureAwait(false);

                    try
                    {
                        _ = Task.Run(() => CommunicateAsync(accepted, stoppingToken), stoppingToken);
                    }
                    catch
                    {
                        accepted.Dispose();
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task<Socket> AcceptAsync(Socket listener, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Accepting");

            var accepted = await listener.AcceptAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogTrace("Accepted");

            return accepted;
        }

        private async Task CommunicateAsync(Socket socket, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Communicating");

            string? uid = null;
            try
            {
                uid = await CommunicateCoreAsync(socket, cancellationToken);
            }
            catch (Exception ex)
            when (ex is OperationCanceledException
                || (ex is SocketException socketException && socketException.SocketErrorCode.IsKnown()))
            {
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Communicated '{uid}'");
                return;
            }

            _logger.LogTrace($"Communicated '{uid}'");
        }

        private async Task<string?> CommunicateCoreAsync(Socket socket, CancellationToken cancellationToken)
        {
            try
            {
                var uid = await AuthenticateAsync(socket, cancellationToken).ConfigureAwait(false);
                if (uid is null)
                {
                    socket.ShutdownSafely(SocketShutdown.Both);
                    socket.Dispose();
                    return null;
                }

                await ProcessAsync(uid, socket, cancellationToken).ConfigureAwait(false);
                return uid;
            }
            catch
            {
                socket.ShutdownSafely(SocketShutdown.Both);
                socket.Dispose();
                throw;
            }
        }

        private async Task<string?> AuthenticateAsync(Socket socket, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Authenticating");

            using var processor = new AuthenticationProcessor(socket);
            var request = await processor.ReadAsync<AuthenticationRequest>(cancellationToken).ConfigureAwait(false);
            string idToken = request.GetIdToken(KeyXML);
            var verifyResult = await _authAdmin.VerifyIdTokenAsync(idToken, cancellationToken).ConfigureAwait(false);
            var response = new AuthenticationResponse()
            {
                Result = verifyResult.ErrorCode switch
                {
                    VerifyIdTokenErrorCode.Success => AuthenticationResult.Success,
                    VerifyIdTokenErrorCode.InvalidToken => AuthenticationResult.InvalidToken,
                    VerifyIdTokenErrorCode.ExpiredToken => AuthenticationResult.ExpiredToken,
                    _ => throw new InvalidOperationException()
                }
            };

            await processor.SendAsync(response, cancellationToken).ConfigureAwait(false);

            _logger.LogTrace($"Authenticated '{verifyResult.Token?.Uid}'");

            return response.Result == AuthenticationResult.Success ? verifyResult.Token?.Uid : null;
        }

        private async Task ProcessAsync(string uid, Socket socket, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Processing '{uid}'");

            var pipePair = DuplexPipePairPool.Rent();

            try
            {
                await _play.AddUidPipeAsync(new(uid, pipePair.B), cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                await pipePair.CompleteAndResetAsync().ConfigureAwait(false);
                throw;
            }

            await _processor.ProcessAsync(socket, pipePair.A, cancellationToken).ConfigureAwait(false);

            _logger.LogTrace($"Processed '{uid}'");
        }
    }
}
