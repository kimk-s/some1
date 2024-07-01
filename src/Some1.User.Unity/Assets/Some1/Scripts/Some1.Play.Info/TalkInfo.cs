using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class TalkInfo
    {
        public TalkInfo(TalkId id, IEnumerable<TalkMessageInfo> messages)
        {
            Id = id;
            Messages = messages.ToDictionary(x => x.Id);
        }

        public TalkId Id { get; }
        public IReadOnlyDictionary<string, TalkMessageInfo> Messages { get; }
    }
}
