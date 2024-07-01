using System.Collections.Generic;
using System.Linq;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class TalkDetailViewModel
    {
        public TalkDetailViewModel(TalkInfo info)
        {
            Id = info.Id;
            Messages = info.Messages.Values
                .Select(x => new TalkMessageViewModel(x))
                .ToDictionary(x => x.Id);
        }

        public TalkId Id { get; }
        public IReadOnlyDictionary<string, TalkMessageViewModel> Messages { get; }
    }
}
