using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Some1.Data.InMemory;

namespace Some1.Play.Data.InMemory
{
    public sealed class InMemoryPlayRepository : IPlayRepository
    {
        private readonly InMemoryRepository _repository;

        public InMemoryPlayRepository(InMemoryRepository repository)
        {
            _repository = repository;
        }

        public Task<PlayUserData[]> LoadUserAsync(IReadOnlyList<string> auths, string playId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.LoadPlayUsers(auths, playId));
        }

        public Task<PlaySaveUserResult[]> SaveUserAsync(IReadOnlyList<IPlayUserData> players, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.SavePlayUsers(players));
        }

        public Task<PlayPlayData> GetPlayAsync(string id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.GetPlayPlay(id));
        }

        public Task SetPlayAsync(PlayPlayData play, CancellationToken cancellationToken)
        {
            _repository.SetPlayPlay(play);
            return Task.CompletedTask;
        }
    }
}
