using System;

namespace Some1.Play.Data
{
    public readonly struct PlaySaveUserResult
    {
        public PlaySaveUserResult(string id, DateTime? premium)
        {
            Id = id;
            Premium = premium;
        }

        public string Id { get; }
        public DateTime? Premium { get; }
    }
}
