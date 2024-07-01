using System;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Some1.Auth.Front
{
    public sealed class FakeAuthFront : IAuthFront, IDisposable
    {
        private readonly ReactiveProperty<IAuthUserFront?> _user = new();
        private bool _isPassed;

        public ReadOnlyReactiveProperty<IAuthUserFront?> User => _user;

        public void Dispose()
        {
            _user.Dispose();
        }

        public Task PassAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _isPassed = true;
            return Task.CompletedTask;
        }

        public Task SignInWithEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_isPassed == false)
            {
                throw new InvalidOperationException();
            }

            var user = new FakeAuthUserFront("email user", email, null, "EMAILUSER");
            _user.Value = user;
            return Task.CompletedTask;
        }

        public Task SignInWithGoogleAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_isPassed == false)
            {
                throw new InvalidOperationException();
            }

            var user = new FakeAuthUserFront("google user", "user@gmail.com", null, "GOOGLEUSER");
            _user.Value = user;
            return Task.CompletedTask;
        }

        public Task SignOutAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_isPassed == false)
            {
                throw new InvalidOperationException();
            }

            _user.Value = null;
            return Task.CompletedTask;
        }

        public Task CreateUserWithEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_isPassed == false)
            {
                throw new InvalidOperationException();
            }

            var user = new FakeAuthUserFront("email user", email, null, "EMAILUSER");
            _user.Value = user;
            return Task.CompletedTask;
        }
    }
}
