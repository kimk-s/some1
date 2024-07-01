using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Some1.Net;

namespace Some1.Play.Client.Tcp
{
    public class TcpPlayClient : IPlayClient
    {
        public const string PublicKeyXml = "<RSAKeyValue><Modulus>9v+EWy8svizL1Wi707Ekp+2D/sLpUi8eOd4XrjRM+kJLPGMk7uQFwTqMxa7wWdgJKvXYxvJCiCDtVOSGqXG4J5m6JKah8VSCw5Jg05heUZ/ETvMaN2qNaqhNWaA4+dxLPEDUoqdua1amxSvzHvN9ez2NUH62JztufFvM4kbjOxU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        private const float TimeoutSeconds = 8;
        private readonly ILogger<TcpPlayClient> _logger;
        private readonly IPlayAddressGettable _addressGettable;
        private readonly IPlayClientAuth _auth;
        private readonly DuplexPipeProcessor _processor;
        private Task? _processing;

        public TcpPlayClient(
            ILogger<TcpPlayClient> logger,
            IPlayAddressGettable addressGettable,
            IPlayClientAuth auth,
            DuplexPipeProcessor processor)
        {
            _logger = logger;
            _addressGettable = addressGettable;
            _auth = auth;
            _processor = processor;
        }

        private IPEndPoint GetEndPoint() => IPEndPointForUnity.Parse(_addressGettable.GetAddress());

        public async Task<DuplexPipe> StartPipeAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Pipe starting");

            var pipe = await StartPipeCoreAsync(cancellationToken);

            _logger.LogTrace("Pipe started");

            return pipe;
        }

        public async Task<DuplexPipe> StartPipeCoreAsync(CancellationToken cancellationToken)
        {
            await WaitToEndProcessAsync(cancellationToken).ConfigureAwait(false);

            Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true
            };

            try
            {
                using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(TimeoutSeconds));
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
                {
                    await ConnectAsync(socket, cts.Token).ConfigureAwait(false);
                    await AuthenticateAsync(socket, cts.Token).ConfigureAwait(false);
                }

                return StartProcess(socket, cancellationToken);
            }
            catch
            {
                socket.ShutdownSafely(SocketShutdown.Both);
                socket.Dispose();
                throw;
            }
        }

        private async Task ConnectAsync(Socket socket, CancellationToken cancellationToken)
        {
            var endPoint = GetEndPoint();

            _logger.LogTrace($"Connecting {endPoint}");

#if UNITY
            using (var registration = cancellationToken.Register(
                static state => ((Socket)state!).Dispose(),
                socket))
            {
                await socket.ConnectAsync(endPoint).ConfigureAwait(false);
            }
#else
            await socket.ConnectAsync(endPoint, cancellationToken).ConfigureAwait(false);
#endif
            _logger.LogTrace($"Connected {endPoint}");
        }

        private async Task AuthenticateAsync(Socket socket, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Authenticating");

            string idToken = await _auth.GetIdTokenAsync(false, cancellationToken).ConfigureAwait(false);

            using var processor = new AuthenticationProcessor(socket);

            var request = AuthenticationRequest.Create(idToken, PublicKeyXml);
            await processor.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var response = await processor.ReadAsync<AuthenticationResponse>(cancellationToken).ConfigureAwait(false);
            switch (response.Result)
            {
                case AuthenticationResult.Success:
                    break;
                default:
                    throw new InvalidOperationException($"Authentication Error: {response.Result}.");
            }

            _logger.LogTrace($"Authenticated");
        }

        private DuplexPipe StartProcess(Socket socket, CancellationToken cancellationToken)
        {
            var pipePair = DuplexPipePairPool.Rent();

            try
            {
                _processing = Task.Run(async () =>
                {
                    _logger.LogTrace($"Processing");

                    await _processor.ProcessAsync(socket, pipePair.A, cancellationToken).ConfigureAwait(false);

                    _logger.LogTrace($"Processed");
                },
                cancellationToken);
            }
            catch
            {
                pipePair.CompleteAndReset();
                throw;
            }

            return pipePair.B;
        }

        private async Task WaitToEndProcessAsync(CancellationToken cancellationToken)
        {
            var processing = _processing;

            if (processing is null)
            {
                return;
            }

            if (!processing.IsCompleted)
            {
                var cancel = new TaskCompletionSource<bool>();
#if UNITY
                using var registration = cancellationToken.Register(
#else
                using var registration = cancellationToken.UnsafeRegister(
#endif
                    static state => ((TaskCompletionSource<bool>)state!).SetCanceled(),
                    cancel);
                var any = await Task.WhenAny(cancel.Task, processing).ConfigureAwait(false);
                await any.ConfigureAwait(false);
            }

            _processing = null;
        }
    }
}
