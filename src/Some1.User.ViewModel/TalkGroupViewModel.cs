using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class TalkGroupViewModel
    {
        private readonly Func<TalkId, TalkDetailViewModel> _createDetail;

        public TalkGroupViewModel(IPlayInfoRepository repository)
        {
            var infos = repository.Value.Talks;

            Items = infos.ById.Values
                .Select(x => new TalkViewModel(x))
                .ToDictionary(x => x.Id);

            _createDetail = x => new TalkDetailViewModel(infos.ById[x]);
        }

        public IReadOnlyDictionary<TalkId, TalkViewModel> Items { get; }

        public TalkDetailViewModel CreateDetail(TalkId id) => _createDetail(id);
    }
}
