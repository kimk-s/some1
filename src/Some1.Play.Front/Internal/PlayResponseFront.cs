using System;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayResponseFront : ISyncReadable, ISyncPolatable, IDisposable
    {
        private readonly Readable _readable;
        private readonly PlayerFront _player;
        private readonly RankingGroupFront _rankings;
        private readonly ObjectGroupFront _objects;
        private readonly TimeFront _time;
        private double _previousTotalSeconds;

        public PlayResponseFront(PlayInfo info, IRegionGroupFront regions)
        {
            _time = new();
            _player = new(
                _time,
                info.Characters,
                info.CharacterAlives,
                info.CharacterCasts,
                info.CharacterSkills,
                info.CharacterSkillProps,
                info.CharacterEnergies,
                info.CharacterSkins,
                info.CharacterSkinEmojis,
                info.Specialties,
                info.Seasons,
                regions,
                _time);
            _rankings = new(_player, _time);
            _objects = new(_time, info, _player.Object);
            _readable = new(_player, _rankings, _objects, _time);
        }

        internal IPlayerFront Player => _player;

        internal IRankingGroupFront Rankings => _rankings;

        internal IObjectGroupFront Objects => _objects;

        internal ITimeFront Time => _time;

        public void Dispose()
        {
            _player.Dispose();
            _rankings.Dispose();
            _objects.Dispose();
            _time.Dispose();
        }

        public void Extrapolate()
        {
            _player.Extrapolate();
            _objects.Extrapolate();
            _time.Extrapolate();
        }

        public void Interpolate(float time)
        {
            _player.Interpolate(time);
            _objects.Interpolate(time);
            _time.Interpolate(time);

            Update();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            //_readable.Read(ref reader, mode);
            reader.ReadSyncWithCompression(_readable, mode);
        }

        private void Update()
        {
            var deltaSeconds = (float)(_time.TotalSeconds.CurrentValue - _previousTotalSeconds);
            if (deltaSeconds > 0)
            {
                _player.Update(deltaSeconds);
                _objects.Update(deltaSeconds);
            }

            _previousTotalSeconds = _time.TotalSeconds.CurrentValue;
        }

        private sealed class Readable : ISyncReadable
        {
            private readonly PlayerFront _player;
            private readonly RankingGroupFront _rankings;
            private readonly ObjectGroupFront _objects;
            private readonly TimeFront _time;

            public Readable(PlayerFront player, RankingGroupFront rankings, ObjectGroupFront objects, TimeFront time)
            {
                _player = player;
                _rankings = rankings;
                _objects = objects;
                _time = time;
            }

            public void Read(ref MemoryPackReader reader, SyncMode mode)
            {
                _time.Read(ref reader, mode);
                _objects.Read(ref reader, mode);
                _rankings.Read(ref reader, mode);
                reader.ReadDestinationDirtySafely(_player, mode);
            }
        }
    }
}
