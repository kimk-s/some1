using System;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Some1.Auth.Front
{
    public interface IAuthFront
    {
        ReadOnlyReactiveProperty<IAuthUserFront?> User { get; }

        Task PassAsync(CancellationToken cancellationToken = default);
        Task SignInWithEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default);
        Task SignInWithGoogleAsync(CancellationToken cancellationToken = default);
        Task SignOutAsync(CancellationToken cancellationToken = default);
        Task CreateUserWithEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default);
    }

    public static class AbstractionAuthFrontExtensions
    {
        public static IAuthUserFront GetRequiredUser(this IAuthFront auth) => auth.User.CurrentValue ?? throw new InvalidOperationException();
    }
}
