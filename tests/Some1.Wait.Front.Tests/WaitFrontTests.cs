using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Auth.Front;
using Some1.Data.InMemory;
using Some1.Store.Admin;
using Some1.Wait.Back;
using Some1.Wait.Data;
using Some1.Wait.Data.InMemory;

namespace Some1.Wait.Front;

public class WaitFrontTests
{
    [Fact]
    public async Task StartAsync()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();

        await wait.StartAsync(CancellationToken.None);

        Assert.NotNull(wait.User.CurrentValue);
        Assert.NotNull(wait.Wait.CurrentValue);
        Assert.NotNull(wait.Plays.CurrentValue);
        Assert.Null(wait.SelectedPlay.CurrentValue);
        Assert.NotNull(wait.PlayServers.CurrentValue);
        Assert.Null(wait.SelectedPlayServer.CurrentValue);
    }

    [Fact]
    public async Task StartAsync_WhenPlaying()
    {
        using var services = GetServices(userPlayId: "ap-seoul-1", userIsPlaying:true);
        var wait = services.GetRequiredService<IWaitFront>();

        await wait.StartAsync(CancellationToken.None);

        Assert.NotNull(wait.SelectedPlay.CurrentValue);
    }

    [Fact]
    public async Task StartAsync_WhenPlayingAndNotManagerAndMaintenance()
    {
        using var services = GetServices(
            userPlayId: "ap-seoul-1",
            userIsPlaying: true,
            waitMaintenance: true);
        var wait = services.GetRequiredService<IWaitFront>();

        await wait.StartAsync(CancellationToken.None);

        Assert.False(wait.User.CurrentValue?.Manager);
        Assert.True(wait.Wait.CurrentValue?.Maintenance);
        Assert.Null(wait.Plays.CurrentValue);
        Assert.Null(wait.SelectedPlay.CurrentValue);
        Assert.Null(wait.SelectedPlayServer.CurrentValue);
    }

    [Fact]
    public async Task StartAsync_WhenPlayingAndManagerAndMaintenance()
    {
        using var services = GetServices(
            userPlayId: "ap-seoul-1",
            userIsPlaying: true,
            userManager: true,
            waitMaintenance: true);
        var wait = services.GetRequiredService<IWaitFront>();

        await wait.StartAsync(CancellationToken.None);

        Assert.True(wait.User.CurrentValue?.Manager);
        Assert.True(wait.Wait.CurrentValue?.Maintenance);
        Assert.NotNull(wait.Plays.CurrentValue);
        Assert.NotNull(wait.SelectedPlay.CurrentValue);
        Assert.Null(wait.SelectedPlayServer.CurrentValue);
    }

    [Fact]
    public async Task FetchUserAsync()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();

        await wait.FetchUserAsync(CancellationToken.None);

        Assert.NotNull(wait.User.CurrentValue);
    }

    [Fact]
    public async Task FetchWaitAsync()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();

        await wait.FetchWaitAsync(CancellationToken.None);

        Assert.NotNull(wait.Wait.CurrentValue);
    }

    [Fact]
    public async Task FetchPlaysAsync()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();

        await wait.FetchPlaysAsync(CancellationToken.None);

        Assert.NotNull(wait.Plays.CurrentValue);
        Assert.NotNull(wait.PlayServers.CurrentValue);
        Assert.Null(wait.SelectedPlayServer.CurrentValue);
    }

    [Fact]
    public async Task SelectPlayServerAsync()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();
        await wait.StartAsync(CancellationToken.None);

        await wait.SelectPlayServerAsync(new("AsiaPacific", "Seoul"), CancellationToken.None);

        Assert.NotNull(wait.SelectedPlayServer.CurrentValue);
    }

    [Fact]
    public async Task SelectPlayServerAsync_BeforeFetchPlays_Throws()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();

        await Assert.ThrowsAsync<InvalidOperationException>(()
            => wait.SelectPlayServerAsync(new("AsiaPacific", "Seoul"), CancellationToken.None));
    }

    [Fact]
    public async Task SelectPlayChannelAsync()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();
        await wait.StartAsync(CancellationToken.None);
        await wait.SelectPlayServerAsync(new("AsiaPacific", "Seoul"), CancellationToken.None);

        await wait.SelectPlayChannelAsync(1, CancellationToken.None);

        Assert.NotNull(wait.SelectedPlay.CurrentValue);
    }

    [Fact]
    public async Task SelectPlayChannelAsync_NotFoundNumber_Throws()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();
        await wait.StartAsync(CancellationToken.None);
        await wait.SelectPlayServerAsync(new("AsiaPacific", "Seoul"), CancellationToken.None);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => wait.SelectPlayChannelAsync(0, CancellationToken.None));
    }

    [Fact]
    public async Task SelectPlayChannelAsync_BeforeSelectPlayServer_Throws()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();
        await wait.StartAsync(CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(() => wait.SelectPlayChannelAsync(0, CancellationToken.None));
    }

    [Fact]
    public async Task SelectPlayChannelAsync_SelectedPlayIsExists_Throws()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();
        await wait.StartAsync(CancellationToken.None);
        await wait.SelectPlayServerAsync(new("AsiaPacific", "Seoul"), CancellationToken.None);
        await wait.SelectPlayChannelAsync(1, CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(() => wait.SelectPlayChannelAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task FetchPlaysAsync_ClearSelectedPlay()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();
        await wait.StartAsync(CancellationToken.None);
        await wait.SelectPlayServerAsync(new("AsiaPacific", "Seoul"), CancellationToken.None);
        await wait.SelectPlayChannelAsync(1, CancellationToken.None);

        await wait.FetchPlaysAsync(CancellationToken.None);

        Assert.Null(wait.SelectedPlay.CurrentValue);
    }

    [Fact]
    public void Reset()
    {
        using var services = GetServices();
        var wait = services.GetRequiredService<IWaitFront>();

        wait.Reset();

        Assert.Null(wait.User.CurrentValue);
        Assert.Null(wait.Plays.CurrentValue);
        Assert.Null(wait.PlayServers.CurrentValue);
        Assert.Null(wait.SelectedPlayServer.CurrentValue);
    }

    private static ServiceProvider GetServices(
        bool userManager = false,
        string userPlayId = "",
        bool userIsPlaying = false,
        bool waitMaintenance = false)
    {
        var services = new ServiceCollection()
            .AddScoped<IWaitFront, WaitFront>()
            .AddScoped<IWaitBack, WaitBack>()
            .AddScoped<IWaitRepository, InMemoryWaitRepository>()
            .AddScoped<IAuthFront, FakeAuthFront>()
            .AddScoped<IStore, FakeStore>()
            .AddScoped<IStoreAdmin, FakeStoreAdmin>()
            .AddScoped<InMemoryRepository>()
            .AddLogging()
            .Configure<InMemoryRepositoryOptions>(x =>
            {
                x.User = new()
                {
                    PlayId = userPlayId,
                    IsPlaying = userIsPlaying,
                    Manager = userManager,
                };

                x.Wait = new()
                {
                    Maintenance = waitMaintenance
                };

                x.Plays = new InMemoryRepositoryOptionsPlay[]
                {
                    new()
                    {
                        Id = "ap-seoul-1",
                        Region = "AsiaPacific",
                        City = "Seoul",
                        Number = 1,
                        Address = "127.0.0.1:8000",
                        Busy = 0,
                    },
                    new()
                    {
                        Id = "ap-seoul-2",
                        Region = "AsiaPacific",
                        City = "Seoul",
                        Number = 2,
                        Address = "127.0.0.1:8001",
                        Busy = 0,
                    },
                };
            })
            .BuildServiceProvider();

        var auth = services.GetRequiredService<IAuthFront>();
        auth.PassAsync().Wait();
        auth.SignInWithGoogleAsync().Wait();

        return services;
    }

    private sealed class FakeStore : IStore
    {
        public IReadOnlyDictionary<string, IStoreProduct> Products => throw new NotImplementedException();

        public Observable<IStoreProduct> SingleBuyProcessed { get; } = new Subject<IStoreProduct>();

        public Task<IStoreProduct?> BuyAsync(string productId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ConfirmBuyAsync(string productId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
