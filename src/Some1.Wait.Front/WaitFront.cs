using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using R3;
using Some1.Auth.Front;
using Some1.Wait.Back;
using Some1.Wait.Front.Internal;

namespace Some1.Wait.Front
{
    public sealed class WaitFront : IWaitFront, IDisposable
    {
        private readonly CancellationTokenSource _ctsOnDispose = new();
        private readonly ILogger<WaitFront> _logger;
        private readonly IWaitBack _back;
        private readonly IAuthFront _auth;
        private readonly IStore _store;
        private readonly ReactiveProperty<IWaitUserFront?> _user = new();
        private readonly ReactiveProperty<IWaitWaitFront?> _wait = new();
        private readonly ReactiveProperty<IReadOnlyDictionary<string, IWaitPlayFront>?> _plays = new();
        private readonly ReactiveProperty<IWaitPlayFront?> _selectedPlay = new();
        private readonly ReactiveProperty<IReadOnlyDictionary<WaitPlayServerFrontId, IWaitPlayServerFront>?> _playServers = new();
        private readonly ReactiveProperty<IWaitPlayServerFront?> _selectedPlayServer = new();
        private readonly ReactiveProperty<WaitBuyProductResult> _buyProductResult = new();
        private readonly ReactiveProperty<WaitPremiumLogBack[]?> _premiumLogs = new();
        private DateTime _getPlaysTime;

        public WaitFront(ILogger<WaitFront> logger, IWaitBack back, IAuthFront auth, IStore store)
        {
            _logger = logger;
            _back = back;
            _auth = auth;
            _store = store;
            store.SingleBuyProcessed.Subscribe(Store_SingleBuyProcessed);
        }

        public ReadOnlyReactiveProperty<IWaitUserFront?> User => _user;

        public ReadOnlyReactiveProperty<IWaitWaitFront?> Wait => _wait;

        public ReadOnlyReactiveProperty<IReadOnlyDictionary<string, IWaitPlayFront>?> Plays => _plays;

        public ReadOnlyReactiveProperty<IWaitPlayFront?> SelectedPlay => _selectedPlay;

        public ReadOnlyReactiveProperty<IReadOnlyDictionary<WaitPlayServerFrontId, IWaitPlayServerFront>?> PlayServers => _playServers;

        public ReadOnlyReactiveProperty<IWaitPlayServerFront?> SelectedPlayServer => _selectedPlayServer;

        public IReadOnlyDictionary<string, IStoreProduct> Products => _store.Products;

        public ReadOnlyReactiveProperty<WaitBuyProductResult> BuyProductResult => _buyProductResult;

        public ReadOnlyReactiveProperty<WaitPremiumLogBack[]?> PremiumLogs => _premiumLogs;

        public void Dispose()
        {
            _ctsOnDispose.Cancel();
            _user.Dispose();
            _wait.Dispose();
            _plays.Dispose();
            _selectedPlay.Dispose();
            _playServers.Dispose();
            _selectedPlayServer.Dispose();
            _buyProductResult.Dispose();
            _premiumLogs.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _selectedPlay.Value = null;

            await FetchUserAsync(cancellationToken);
            await FetchWaitAsync(cancellationToken);

            var user = _user.Value ?? throw new InvalidOperationException();
            var wait = _wait.Value ?? throw new InvalidOperationException();

            if (wait.Maintenance && !user.Manager)
            {
                return;
            }

            await StartStoreAsync(cancellationToken);

            await FetchPlaysAsync(cancellationToken);

            var plays = _plays.Value ?? throw new InvalidOperationException();

            plays.TryGetValue(user.PlayId, out var play);
            if (user.IsPlaying)
            {
                if (play is not null)
                {
                    _selectedPlay.Value = play;
                    return;
                }

                _logger.LogWarning($"WaitUser is playing but not found WaitPlay.");
            }

            var playServers = _playServers.Value ?? throw new InvalidOperationException();

            if (_selectedPlayServer.Value is null
                && play is not null
                && playServers.ContainsKey(new(play.Region, play.City)))
            {
                await SelectPlayServerAsync(new(play.Region, play.City), cancellationToken);
            }
        }

        public async Task FetchUserAsync(CancellationToken cancellationToken)
        {
            var authUser = _auth.GetRequiredUser();
            var userBack = await _back.GetUserAsync(authUser.UserId, cancellationToken);
            var userFront = new WaitUserFront(userBack, authUser);
            _user.Value = userFront;
        }

        public async Task FetchWaitAsync(CancellationToken cancellationToken)
        {
            var wait = await _back.GetWaitAsync(cancellationToken);
            var waitFront = new WaitWaitFront(wait.Maintenance);
            _wait.Value = waitFront;
        }

        public async Task FetchPlaysAsync(CancellationToken cancellationToken)
        {
            _selectedPlay.Value = null;

            const float CacheTimeSpan = 5;
            if ((DateTime.Now - _getPlaysTime).TotalSeconds < CacheTimeSpan)
            {
                return;
            }
            _getPlaysTime = DateTime.Now;

            var backs = await _back.GetPlaysAsync(cancellationToken);

            _plays.Value = backs
                .Select(x => (IWaitPlayFront)new WaitPlayFront(
                    x.Id,
                    x.Region,
                    x.City,
                    x.Number,
                    x.Address,
                    x.OpeningSoon,
                    x.Maintenance,
                    x.Busy))
                .ToDictionary(x => x.Id);

            var playServers = _plays.Value.Values
                .GroupBy(x => (x.Region, x.City))
                .Select(x => (IWaitPlayServerFront)new WaitPlayServerFront(
                    new(x.Key.Region, x.Key.City),
                    x
                        .Select(x => (IWaitPlayChannelFront)new WaitPlayChannelFront(
                            x.Number,
                            x.Id,
                            x.OpeningSoon,
                            x.Maintenance,
                            x.Busy,
                            x.IsFull))
                        .ToDictionary(x => x.Number)))
                .ToDictionary(x => x.Id);

            if (_selectedPlayServer.Value is not null)
            {
                _selectedPlayServer.Value = playServers.GetValueOrDefault(_selectedPlayServer.Value.Id);

                if (_selectedPlayServer.Value is not null)
                {
                    ((WaitPlayServerFront)_selectedPlayServer.Value).IsSelected = true;
                }
            }

            _playServers.Value = playServers;
        }

        public Task SelectPlayServerAsync(WaitPlayServerFrontId id, CancellationToken cancellationToken)
        {
            var user = _user.Value ?? throw new InvalidOperationException();
            var playServers = _playServers.Value ?? throw new InvalidOperationException();
            var playServer = playServers[id];

            if (playServer.OpeningSoon)
            {
                throw new WaitFrontException(WaitFrontError.ServerOpeningSoon);
            }

            if (playServer.Maintenance && !user.Manager)
            {
                throw new WaitFrontException(WaitFrontError.ServerMaintenance);
            }

            if (playServer.IsFull)
            {
                throw new WaitFrontException(WaitFrontError.ServerFull);
            }

            if (playServer == _selectedPlayServer.Value)
            {
                return Task.CompletedTask;
            }

            if (_selectedPlayServer.Value is not null)
            {
                ((WaitPlayServerFront)_selectedPlayServer.Value).IsSelected = false;
            }

            ((WaitPlayServerFront)playServer).IsSelected = true;
            _selectedPlayServer.Value = playServer;

            return Task.CompletedTask;
        }

        public Task SelectPlayChannelAsync(int number, CancellationToken cancellationToken)
        {
            var user = _user.Value ?? throw new InvalidOperationException();
            var plays = _plays.Value ?? throw new InvalidOperationException();
            var selectedPlayServer = _selectedPlayServer.Value ?? throw new InvalidOperationException();

            if (_selectedPlay.Value is not null)
            {
                throw new InvalidOperationException();
            }

            bool isAutomaticChannel = number == IWaitFront.AutomaticPlayChannelNumber;
            if (isAutomaticChannel)
            {
                const float C_Busy = 0.7f;
                number = selectedPlayServer.Channels.Values
                    .OrderBy(x => x.OpeningSoon ? 3 : x.Maintenance ? 2 : x.Busy < C_Busy ? C_Busy - x.Busy : x.Busy)
                    .First()
                    .Number;
            }

            var channel = selectedPlayServer.Channels[number];

            var play = plays[channel.PlayId];

            if (play.OpeningSoon)
            {
                throw new WaitFrontException(isAutomaticChannel ? WaitFrontError.ServerOpeningSoon : WaitFrontError.ChannelOpeningSoon);
            }

            if (play.Maintenance && !user.Manager)
            {
                throw new WaitFrontException(isAutomaticChannel ? WaitFrontError.ServerMaintenance : WaitFrontError.ChannelMaintenance);
            }

            if (play.IsFull)
            {
                throw new WaitFrontException(isAutomaticChannel ? WaitFrontError.ServerFull : WaitFrontError.ChannelFull);
            }

            _selectedPlay.Value = plays[channel.PlayId];

            return Task.CompletedTask;
        }

        public void Reset()
        {
            _user.Value = null;
            _plays.Value = null;
            _selectedPlay.Value = null;
            _playServers.Value = null;
            _selectedPlayServer.Value = null;
            _getPlaysTime = DateTime.MinValue;
        }

        private Task StartStoreAsync(CancellationToken cancellationToken)
        {
            return _store.StartAsync(cancellationToken);
        }

        public async Task BuyProductAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _store.BuyAsync(id, cancellationToken);

                if (product is null)
                {
                    _buyProductResult.Value = WaitBuyProductResult.Deferred;
                }
                else
                {
                    await PostBuyProductAsync(product, cancellationToken);
                    _buyProductResult.Value = WaitBuyProductResult.Purchased;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "");
                throw;
            }
        }

        private void Store_SingleBuyProcessed(IStoreProduct product)
        {
            Task.Run(async () =>
            {
                try
                {
                    await PostBuyProductAsync(product, _ctsOnDispose.Token);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, $"{nameof(Store_SingleBuyProcessed)}");
                }
            });
        }

        private async Task PostBuyProductAsync(IStoreProduct product, CancellationToken cancellationToken)
        {
            if (product.TransactionId is null)
            {
                throw new InvalidOperationException("TransactionId is null");
            }

            string userId = User.CurrentValue?.Id ?? throw new InvalidOperationException();
            int retryDelay = 0;

            while (true)
            {
                try
                {
                    await _back.BuyProductAsync(product.Id, product.TransactionId, userId, cancellationToken);

                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "");
                    await Task.Delay(retryDelay, cancellationToken);
                    if (retryDelay < 7000)
                    {
                        retryDelay += 1000;
                    }
                }
            }

            await _store.ConfirmBuyAsync(product.Id, cancellationToken);
        }

        public void ClearBuyProductResult()
        {
            _buyProductResult.Value = WaitBuyProductResult.None;
        }

        public async Task FetchPremiumLogsAsync(CancellationToken cancellationToken)
        {
            string userId = User.CurrentValue?.Id ?? throw new InvalidOperationException();
            var value = await _back.GetPremiumLogsAsync(userId, cancellationToken);
            _premiumLogs.Value = value;
        }
    }
}
