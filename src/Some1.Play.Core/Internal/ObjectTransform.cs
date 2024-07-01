using System.Buffers;
using MemoryPack;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectTransform : IObjectTransform, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<Vector2Wave> _position = new();
        private readonly ReactiveProperty<float> _rotation = new();
        private readonly IInternalObjectRegion _region;
        private readonly ObjectProperties _properties;
        private float? _shiftRotation;
        private float? _castRotation;
        private float? _walkRotation;

        internal ObjectTransform(IInternalObjectRegion region, ObjectProperties properties, ITime time)
        {
            _region = region;
            _properties = properties;
            _sync = new SyncArraySource(
                _position.ToWaveSource(time),
                _rotation.ToUnmanagedParticleSource());
        }

        public ReadOnlyReactiveProperty<Vector2Wave> Position => _position;

        public ReadOnlyReactiveProperty<float> Rotation => _rotation;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void SetPosition(Vector2Wave value)
        {
            _position.Value = value;

            _region.Set(value.B);
            _properties.SetAreaPosition(value.B);
        }

        internal void SetShiftRotation(float? value)
        {
            if (_shiftRotation == value)
            {
                return;
            }

            _shiftRotation = value;

            if (value is null)
            {
                if (_castRotation is not null)
                {
                    SetRotation(_castRotation.Value);
                }
                else if (_walkRotation is not null)
                {
                    SetRotation(_walkRotation.Value);
                }
            }
            else
            {
                SetRotation(value.Value);
            }
        }

        internal void SetCastRotation(float? value)
        {
            if (_castRotation == value)
            {
                return;
            }

            _castRotation = value;

            if (_shiftRotation is not null)
            {
                return;
            }

            if (value is null)
            {
                if (_walkRotation is not null)
                {
                    SetRotation(_walkRotation.Value);
                }
            }
            else
            {
                SetRotation(value.Value);
            }
        }

        internal void SetWalkRotation(float? value)
        {
            if (_walkRotation == value)
            {
                return;
            }

            _walkRotation = value;

            if (_shiftRotation is not null || _castRotation is not null)
            {
                return;
            }

            if (value is null)
            {
            }
            else
            {
                SetRotation(value.Value);
            }
        }

        internal void SetInstanceRotation(float value)
        {
            if (_shiftRotation is not null || _castRotation is not null || _walkRotation is not null)
            {
                return;
            }

            SetRotation(value);
        }

        private void SetRotation(float value)
        {
            value = Vector2Helper.From0To360(value);

            _rotation.Value = value;

            _properties.Rotation = value;
        }

        internal void Reset()
        {
            SetPosition(default);
            SetRotation(0);
            _shiftRotation = null;
            _castRotation = null;
            _walkRotation = null;
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }

        public void Dispose()
        {
            _sync.Dispose();
        }
    }
}
