using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using R3;
using Some1.Wait.Back;

namespace Some1.Wait.Front
{
    public interface IWaitFront
    {
        const int AutomaticPlayChannelNumber = -1;

        ReadOnlyReactiveProperty<IWaitUserFront?> User { get; }
        ReadOnlyReactiveProperty<IWaitWaitFront?> Wait { get; }
        ReadOnlyReactiveProperty<IReadOnlyDictionary<string, IWaitPlayFront>?> Plays { get; }
        ReadOnlyReactiveProperty<IWaitPlayFront?> SelectedPlay { get; }
        ReadOnlyReactiveProperty<IReadOnlyDictionary<WaitPlayServerFrontId, IWaitPlayServerFront>?> PlayServers { get; }
        ReadOnlyReactiveProperty<IWaitPlayServerFront?> SelectedPlayServer { get; }
        IReadOnlyDictionary<string, IStoreProduct> Products { get; }
        ReadOnlyReactiveProperty<WaitBuyProductResult> BuyProductResult { get; }
        ReadOnlyReactiveProperty<WaitPremiumLogBack[]?> PremiumLogs { get; }

        Task StartAsync(CancellationToken cancellationToken);
        Task FetchUserAsync(CancellationToken cancellationToken);
        Task FetchWaitAsync(CancellationToken cancellationToken);
        Task FetchPlaysAsync(CancellationToken cancellationToken);
        Task SelectPlayServerAsync(WaitPlayServerFrontId id, CancellationToken cancellationToken);
        Task SelectPlayChannelAsync(int number, CancellationToken cancellationToken);
        void Reset();
        Task BuyProductAsync(string id, CancellationToken cancellationToken);
        void ClearBuyProductResult();
        Task FetchPremiumLogsAsync(CancellationToken cancellationToken);
    }
}
