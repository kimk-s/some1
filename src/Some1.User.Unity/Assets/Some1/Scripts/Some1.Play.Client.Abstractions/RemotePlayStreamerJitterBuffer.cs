using System;

namespace Some1.Play.Client
{
    public class RemotePlayStreamerJitterBuffer
    {
        private float _value = 0.15f;

        public float Value
        {
            get => _value;

            set
            {
                if (value < 0 || value > 0.2f)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _value = value;
            }
        }
    }
}
