using System;

namespace Some1.Play.Info
{
    public readonly struct CharacterCastAimLimitInfo
    {
        public CharacterCastAimLimitInfo(bool useRotation, float minLength, float maxLength)
        {
            if (minLength > maxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(minLength));
            }

            UseRotation = useRotation;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public bool UseRotation { get; }

        public float MinLength { get; }

        public float MaxLength { get; }
    }
}
