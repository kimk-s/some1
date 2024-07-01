using System;
using System.Collections.Generic;
using System.Diagnostics;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectCastItem : IObjectCastItem
    {
        private readonly int _objectId;
        private readonly CharacterCastInfoGroup _infos;
        private readonly ReactiveProperty<FloatWave> _loadWave = new();
        private readonly CharacterCastStatInfoGroup _statInfos;
        private readonly ObjectTransform _transform;
        private readonly IObjectProperties _properties;
        private readonly Space _space;
        private CharacterId? _characterId;

        internal ObjectCastItem(
            int objectId,
            CastId id,
            CharacterCastInfoGroup infos,
            CharacterCastStatInfoGroup statInfos,
            ObjectTransform transform,
            IObjectProperties properties,
            Space space)
        {
            _objectId = objectId;
            Id = id;
            _infos = infos;
            _statInfos = statInfos;
            _transform = transform;
            _properties = properties;
            _space = space;
        }

        public CastId Id { get; }

        public ReadOnlyReactiveProperty<FloatWave> LoadWave => _loadWave;

        public int LoadCount => (int)MathF.Truncate(_loadWave.Value.B);

        public int MaxLoadCount => Info?.LoadCount ?? 0;

        public bool AnyLoadCount => LoadCount > 0;

        public ObjectTarget ObjectTarget => Info is null
            ? ObjectTarget.None
            : new(Info.Aim.Target, _properties.Team.CurrentValue);

        internal bool Battle { get; set; }

        internal bool Idle { get; set; }

        internal CharacterCastInfo? Info { get; private set; }

        internal IReadOnlyList<CharacterCastStatInfo>? StatInfos { get; private set; }

        private bool Reloadable => Battle && Idle;

        internal void Set(CharacterId characterId)
        {
            if (_characterId == characterId)
            {
                return;
            }
            _characterId = characterId;

            _loadWave.Value = FloatWave.Zero;
            Info = _characterId is null ? null : _infos.ById.GetValueOrDefault(new(_characterId.Value, Id));
            StatInfos = _characterId is null ? null : _statInfos.ById.GetValueOrDefault(new(_characterId.Value, Id));
        }

        internal Aim GetAutoAim(ParallelToken parallelToken) => GetScanAim(parallelToken) ?? GetJustAim();

        internal Aim? GetScanAim(ParallelToken parallelToken) => Info is null
            ? null
            : Info.Aim.Type == CastAimType.Self
                ? GetJustAim()
                : _space.GetStrikeScanAim(
                    new(
                        _objectId,
                        Area.From(Info.Aim.Area, _properties.Area.Position, PlayConst.UnitStrikeScanAimScale),
                        Info.Aim.Length,
                        Info.Aim.Offset,
                        ObjectTarget,
                        Info.Aim.BumpLevel),
                    parallelToken);

        private Aim GetJustAim() => Info is null
            ? new(_transform.Rotation.CurrentValue, 0)
            : new(_transform.Rotation.CurrentValue, Info.AimLimit.MinLength + (Info.AimLimit.MaxLength - Info.AimLimit.MinLength) * 0.5f);

        internal bool TryConsume()
        {
            if (!CanConsume())
            {
                return false;
            }

            Debug.Assert(LoadCount > 0);
            _loadWave.Value = new(_loadWave.Value.B - 1);

            return true;
        }

        public bool CanConsume()
        {
            return Info is not null && AnyLoadCount;
        }

        internal void Update(float deltaSeconds)
        {
            Reload(deltaSeconds);
        }

        private void Reload(float deltaSeconds)
        {
            if (Info is null)
            {
                return;
            }

            if (Reloadable)
            {
                if (_loadWave.Value.B >= MaxLoadCount)
                {
                    if (_loadWave.Value.Delta != 0)
                    {
                        FillLoad();
                    }
                    return;
                }

                var value = _loadWave.Value.Flow(deltaSeconds / Info.LoadTime);
                if (value.B > MaxLoadCount)
                {
                    value = new(value.A, MaxLoadCount);
                }
                _loadWave.Value = value;
            }
            else
            {
                Stop();
            }
        }

        internal void Reset()
        {
            _characterId = null;
            ClearLoad();
            Battle = false;
            Idle = false;
            Info = null;
            StatInfos = null;
        }

        internal void Stop()
        {
            _loadWave.Value = new(_loadWave.Value.B);
        }

        internal void FillLoad()
        {
            _loadWave.Value = new(MaxLoadCount);
        }

        internal void ClearLoad()
        {
            _loadWave.Value = FloatWave.Zero;
        }
    }
}
