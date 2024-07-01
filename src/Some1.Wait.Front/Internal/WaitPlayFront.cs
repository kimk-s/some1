namespace Some1.Wait.Front.Internal
{
    internal sealed class WaitPlayFront : IWaitPlayFront
    {
        internal WaitPlayFront(
            string id,
            string region,
            string city,
            int number,
            string address,
            bool openingSoon,
            bool maintenance,
            float busy)
        {
            Id = id;
            Region = region;
            City = city;
            Number = number;
            Address = address;
            OpeningSoon = openingSoon;
            Maintenance = maintenance;
            Busy = busy;
        }

        public string Id { get; }

        public string Region { get; }

        public string City { get; }

        public int Number { get; }

        public string Address { get; }

        public bool OpeningSoon { get; }

        public bool Maintenance { get; }

        public float Busy { get; }

        public bool IsFull => Busy >= 1;
    }
}
