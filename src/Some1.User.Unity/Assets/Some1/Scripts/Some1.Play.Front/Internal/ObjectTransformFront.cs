using System;
using System.Diagnostics;
using System.Numerics;
using MemoryPack;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectTransformFront : IObjectTransformFront, ISyncDestination, ISyncPolatable
    {
        private const float RotationSeconds = 0.1f;
        private const float RotationSpeed = 180 / RotationSeconds;

        private readonly SyncArrayDestination _sync;
        private readonly Vector2WaveDestination _position;
        private readonly UnmanagedParticleDestination<float> _rotation = new();
        private readonly ReactiveProperty<float> _rotation2 = new();
        private readonly ReadOnlyReactiveProperty<int> _objectId;
        private int _previousObjectId;

        internal ObjectTransformFront(ISyncTime syncFrame, ReadOnlyReactiveProperty<int> objectId)
        {
            _sync = new(
                _position = new(syncFrame),
                _rotation);
            _objectId = objectId;
        }

        public ReadOnlyReactiveProperty<Vector2> Position => _position.InterpolatedValue;

        public ReadOnlyReactiveProperty<float> Rotation => _rotation2;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public void Dispose()
        {
            _sync.Dispose();
            _rotation2.Dispose();
        }

        public void Extrapolate()
        {
            _sync.Extrapolate();
        }

        public void Interpolate(float time)
        {
            _sync.Interpolate(time);
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _sync.Read(ref reader, mode);
        }

        public void Reset()
        {
            _sync.Reset();
        }

        internal void Update(float deltaSeconds)
        {
            float rot = _rotation.Value.CurrentValue;
            float rot2 = _rotation2.CurrentValue;

            if (_previousObjectId == _objectId.CurrentValue)
            {
                if (rot != rot2)
                {
                    float diff = rot - rot2;
                    if (diff > 180)
                    {
                        diff -= 360;
                    }
                    else if (diff < -180)
                    {
                        diff += 360;
                    }
                    Debug.Assert(diff <= 180 && diff >= -180);

                    float direction = diff < 0 ? -1 : 1;
                    float distance = Math.Abs(diff);
                    float deltaRotation = deltaSeconds * RotationSpeed;

                    _rotation2.Value = deltaRotation > distance
                        ? rot
                        : Vector2Helper.From0To360(_rotation2.CurrentValue + deltaRotation * direction);
                }
            }
            else
            {
                if (rot != rot2)
                {
                    _rotation2.Value = rot;
                }
            }

            _previousObjectId = _objectId.CurrentValue;
        }
    }
}
