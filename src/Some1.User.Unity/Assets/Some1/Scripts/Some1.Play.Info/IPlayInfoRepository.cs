using System.Threading;
using System.Threading.Tasks;

namespace Some1.Play.Info
{
    public interface IPlayInfoRepository
    {
        PlayInfo Value { get; }

        Task LoadAsync(CancellationToken cancellationToken);
    }
}
