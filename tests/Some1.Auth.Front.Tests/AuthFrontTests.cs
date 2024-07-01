using Microsoft.Extensions.DependencyInjection;

namespace Some1.Auth.Front;

public class AuthFrontTests
{
    [Fact]
    public async Task PassAsync()
    {
        using var services = GetServices();
        var auth = services.GetRequiredService<IAuthFront>();

        await auth.PassAsync();
    }

    [Fact]
    public async Task SignInWithEmailAndPasswordAsync()
    {
        using var services = GetServices();
        var auth = services.GetRequiredService<IAuthFront>();
        await auth.PassAsync();

        await auth.SignInWithEmailAndPasswordAsync("", "");

        Assert.NotNull(auth.User.CurrentValue);
    }

    [Fact]
    public async Task SignInWithEmailAndPasswordAsync_BeforePass_Throws()
    {
        using var services = GetServices();
        var auth = services.GetRequiredService<IAuthFront>();

        await Assert.ThrowsAsync<InvalidOperationException>(() => auth.SignInWithEmailAndPasswordAsync("", ""));
    }

    [Fact]
    public async Task SignInWithGoogleAsync()
    {
        using var services = GetServices();
        var auth = services.GetRequiredService<IAuthFront>();
        await auth.PassAsync();

        await auth.SignInWithGoogleAsync();

        Assert.NotNull(auth.User.CurrentValue);
    }

    [Fact]
    public async Task SignInWithGoogleAsync_BeforePass_Throws()
    {
        using var services = GetServices();
        var auth = services.GetRequiredService<IAuthFront>();

        await Assert.ThrowsAsync<InvalidOperationException>(() => auth.SignInWithGoogleAsync());
    }

    [Fact]
    public async Task SignOutAsync()
    {
        using var services = GetServices();
        var auth = services.GetRequiredService<IAuthFront>();
        await auth.PassAsync();

        await auth.SignOutAsync();

        Assert.Null(auth.User.CurrentValue);
    }

    [Fact]
    public async Task SignOutAsync_BeforePass_Throws()
    {
        using var services = GetServices();
        var auth = services.GetRequiredService<IAuthFront>();

        await Assert.ThrowsAsync<InvalidOperationException>(() => auth.SignOutAsync());
    }

    private static ServiceProvider GetServices()
    {
        return new ServiceCollection()
            .AddScoped<IAuthFront, FakeAuthFront>()
            .BuildServiceProvider();
    }
}
