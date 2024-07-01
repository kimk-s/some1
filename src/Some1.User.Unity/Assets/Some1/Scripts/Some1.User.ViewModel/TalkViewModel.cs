using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class TalkViewModel
    {
        public TalkViewModel(TalkInfo info)
        {
            Id = info.Id;
        }

        public TalkId Id { get; }
    }
}
