namespace Some1.Play.Info
{
    public sealed class TalkMessageInfo
    {
        public TalkMessageInfo(string id, CharacterId sender)
        {
            Id = id;
            Sender = sender;
        }

        public string Id { get; }
        public CharacterId Sender { get; }
    }
}
