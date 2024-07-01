using System;
using System.Collections.Generic;
using System.Linq;

namespace Some1.Wait.Front.Internal
{
    internal sealed class WaitPlayServerFront : IWaitPlayServerFront
    {
        public WaitPlayServerFront(WaitPlayServerFrontId id, IReadOnlyDictionary<int, IWaitPlayChannelFront> channels)
        {
            if (channels.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(channels));
            }

            Id = id;
            OpeningSoon = channels.Values.All(x => x.OpeningSoon);
            Maintenance = channels.Values.Where(x => !x.OpeningSoon).All(x => x.Maintenance);
            IsFull = channels.Values.Where(x => !x.OpeningSoon && !x.Maintenance).All(x => x.IsFull);
            Channels = channels;
        }

        public WaitPlayServerFrontId Id { get; }

        public IReadOnlyDictionary<int, IWaitPlayChannelFront> Channels { get; }

        public bool OpeningSoon { get; }

        public bool Maintenance { get; }

        public bool IsFull { get; }

        public bool IsSelected { get; internal set; }
    }
}
