using System;
using System.Buffers;
using System.Numerics;
using MemoryPack;
using R3;

namespace Some1.Sync.Sources
{
    public class SyncWaveSource<TValue, TWaveValue> : ISyncSource
        where TValue : unmanaged, IWave<TValue, TWaveValue>
        where TWaveValue : unmanaged, IEquatable<TWaveValue>
    {
        private readonly ReadOnlyReactiveProperty<bool> _isDefault;
        private readonly ReactiveProperty<SyncWaveDirty> _dirty = new();
        private readonly IDisposable _subscription;
        private readonly ISyncTime _time;
        private readonly TWaveValue _tolerance;
        private TValue _previousValue;
        private int _extrapolatedFrameCount;

        public SyncWaveSource(ReadOnlyReactiveProperty<TValue> value, ISyncTime time, TWaveValue tolerance)
        {
            Value = value;
            _time = time;
            _tolerance = tolerance;
            _isDefault = value.Select(x => x.Equals(default)).ToReadOnlyReactiveProperty();
            Dirty = _dirty.Select(x => x != SyncWaveDirty.None).ToReadOnlyReactiveProperty();

            _subscription = value.Subscribe(Value_OnNext);

            ClearDirty();
        }

        private void Value_OnNext(TValue value)
        {
            var dirty = _dirty;
            var time = _time;
            var previousValue = _previousValue;
            _previousValue = value;

            if (dirty.Value == SyncWaveDirty.Full)
            {
                return;
            }

            if (dirty.Value == SyncWaveDirty.Half)
            {
                dirty.Value = SyncWaveDirty.Full;
                return;
            }

            if (_extrapolatedFrameCount == time.FrameCount)
            {
                dirty.Value = SyncWaveDirty.Full;
                return;
            }

            if (!value.CanExtrapolateByFps(time.FPS, previousValue, _tolerance))
            {
                dirty.Value = !value.Delta.Equals(default) && value.A.Equals(previousValue.B)
                    ? SyncWaveDirty.Half
                    : SyncWaveDirty.Full;
                return;
            }

            _extrapolatedFrameCount = time.FrameCount;
        }

        public ReadOnlyReactiveProperty<bool> IsDefault => _isDefault;

        public ReadOnlyReactiveProperty<bool> Dirty { get; }

        protected ReadOnlyReactiveProperty<TValue> Value { get; }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            var dirty = _dirty.Value;
            if (mode == SyncMode.Dirty && dirty == SyncWaveDirty.None)
            {
                throw new InvalidOperationException();
            }

            if (IsDefault.CurrentValue)
            {
                writer.WriteUnmanaged((byte)0);
            }
            else if (mode == SyncMode.Dirty && dirty == SyncWaveDirty.Half)
            {
                writer.WriteUnmanaged((byte)1);
                writer.WriteUnmanaged(Value.CurrentValue.Delta);
            }
            else if (Value.CurrentValue.Delta.Equals(default))
            {
                writer.WriteUnmanaged((byte)2);
                writer.WriteUnmanaged(Value.CurrentValue.B);
            }
            else
            {
                writer.WriteUnmanaged((byte)3);
                writer.WriteUnmanaged(Value.CurrentValue);
            }
        }

        public void ClearDirty()
        {
            _dirty.Value = SyncWaveDirty.None;
        }

        public void Dispose()
        {
            _isDefault.Dispose();
            _dirty.Dispose();
            _subscription.Dispose();
        }
    }

    public sealed class FloatWaveSource : SyncWaveSource<FloatWave, float>
    {
        public const float DefaultTolerance = 0.001f;

        public FloatWaveSource(ReadOnlyReactiveProperty<FloatWave> value, ISyncTime time) : this(value, time, DefaultTolerance)
        {
        }

        public FloatWaveSource(ReadOnlyReactiveProperty<FloatWave> value, ISyncTime time, float tolerance) : base(value, time, tolerance)
        {
        }
    }

    public sealed class DoubleWaveSource : SyncWaveSource<DoubleWave, double>
    {
        public const double DefaultTolerance = 0.001d;

        public DoubleWaveSource(ReadOnlyReactiveProperty<DoubleWave> value, ISyncTime time) : this(value, time, DefaultTolerance)
        {
        }

        public DoubleWaveSource(ReadOnlyReactiveProperty<DoubleWave> value, ISyncTime time, double tolerance) : base(value, time, tolerance)
        {
        }
    }

    public sealed class Vector2WaveSource : SyncWaveSource<Vector2Wave, Vector2>
    {
        public static readonly Vector2 DefaultTolerance = new(0.001f, 0.001f);

        public Vector2WaveSource(ReadOnlyReactiveProperty<Vector2Wave> value, ISyncTime time) : this(value, time, DefaultTolerance)
        {
        }

        public Vector2WaveSource(ReadOnlyReactiveProperty<Vector2Wave> value, ISyncTime time, Vector2 tolerance) : base(value, time, tolerance)
        {
        }
    }
}
