using System;
using System.Collections.Generic;
using System.Numerics;
using MemoryPack;
using R3;

namespace Some1.Sync.Destinations
{
    public class SyncWaveDestination<TValue, TWaveValue> : ISyncDestination, ISyncPolatable
        where TValue : unmanaged, IWave<TValue, TWaveValue>
        where TWaveValue : unmanaged, IEquatable<TWaveValue>
    {
        private readonly ReactiveProperty<TValue> _value = new();
        private readonly ReactiveProperty<float> _interpolatedTime = new();
        private readonly ISyncTime _time;
        private int _readFrameCount;

        public SyncWaveDestination(ISyncTime time)
        {
            InterpolatedValue = _value
                .CombineLatest(_interpolatedTime, (value, interpolatedTime) => value.Interpolate(interpolatedTime))
                .ToReadOnlyReactiveProperty();

            IsDefault = _value
                .Select(x => EqualityComparer<TValue>.Default.Equals(x, default))
                .ToReadOnlyReactiveProperty();

            _time = time;
        }

        public ReadOnlyReactiveProperty<TValue> Value => _value;

        public ReadOnlyReactiveProperty<TWaveValue> InterpolatedValue { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault { get; }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            byte code = reader.ReadUnmanaged<byte>();
            if (code == 0)
            {
                Reset();
            }
            else if (code == 1)
            {
                _value.Value = _value.Value.Flow(reader.ReadUnmanaged<TWaveValue>());
            }
            else if (code == 2)
            {
                _value.Value = _value.Value.Hold(reader.ReadUnmanaged<TWaveValue>());
            }
            else if (code == 3)
            {
                _value.Value = reader.ReadUnmanaged<TValue>();
            }
            else
            {
                throw new InvalidOperationException();
            }

            _readFrameCount = _time.FrameCount;
        }

        public void Reset()
        {
            _value.Value = default;
        }

        public void Extrapolate()
        {
            if (_readFrameCount == _time.FrameCount)
            {
                return;
            }

            _value.Value = _value.Value.ExtrapolateByDeltaTime(_time.DeltaTime);
        }

        public void Interpolate(float time)
        {
            if (time < 0 || time > 1)
            {
                //throw new ArgumentOutOfRangeException(nameof(time)); // Thrown on User.CLI
                time = Math.Clamp(time, 0, 1);
            }

            _interpolatedTime.Value = time;
        }

        public void Dispose()
        {
            _value.Dispose();
            _interpolatedTime.Dispose();
        }
    }

    public sealed class FloatWaveDestination : SyncWaveDestination<FloatWave, float>
    {
        public FloatWaveDestination(ISyncTime time) : base(time)
        {
        }
    }

    public sealed class DoubleWaveDestination : SyncWaveDestination<DoubleWave, double>
    {
        public DoubleWaveDestination(ISyncTime time) : base(time)
        {
        }
    }

    public sealed class Vector2WaveDestination : SyncWaveDestination<Vector2Wave, Vector2>
    {
        public Vector2WaveDestination(ISyncTime time) : base(time)
        {
        }
    }
}
