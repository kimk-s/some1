using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Some1.Play.Data
{
    public interface IPlayRepository
    {
        Task<PlayUserData[]> LoadUserAsync(IReadOnlyList<string> auths, string playId, CancellationToken cancellationToken);
        Task<PlaySaveUserResult[]> SaveUserAsync(IReadOnlyList<IPlayUserData> users, CancellationToken cancellationToken);
        Task<PlayPlayData> GetPlayAsync(string id, CancellationToken cancellationToken);
        Task SetPlayAsync(PlayPlayData play, CancellationToken cancellationToken);
    }
}
