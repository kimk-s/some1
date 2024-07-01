using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class TalkMessageViewModel
    {
        public TalkMessageViewModel(TalkMessageInfo info)
        {
            Id = info.Id;
            Sender = info.Sender;
        }

        public string Id { get; }

        public CharacterId Sender { get; }
    }
}
