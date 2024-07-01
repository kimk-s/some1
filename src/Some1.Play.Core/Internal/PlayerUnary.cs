using System;
using System.Buffers;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using R3;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerUnary : IPlayerUnary, ISyncSource
    {
        private readonly NullablePackableParticleSource<UnaryResult> _sync;
        private readonly ReactiveProperty<UnaryResult?> _result = new();
        private readonly PlayerGameManager _gameManager;
        private readonly PlayerCharacterGroup _characters;
        private readonly PlayerEmoji _emoji;
        private readonly PlayerTitle _title;
        private readonly PlayerWelcome _welcome;
        private readonly Object _object;

        internal PlayerUnary(
            PlayerGameManager gameManager,
            PlayerCharacterGroup characters,
            PlayerTitle title,
            PlayerEmoji emoji,
            PlayerWelcome welcome,
            Object @object)
        {
            _gameManager = gameManager;
            _characters = characters;
            _title = title;
            _emoji = emoji;
            _welcome = welcome;
            _object = @object;
            _sync = _result.ToNullablePackableParticleSource();
        }

        public UnaryResult? Result { get => _result.Value; private set => _result.Value = value; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(Unary? unary)
        {
            if (Result?.Unary == unary)
            {
                return;
            }

            if (unary is null)
            {
                Result = new(null, true);
            }
            else
            {
                var u = unary.Value;

                Result = new(
                    u,
                    u.Type switch
                    {
                        UnaryType.SelectCharacter => SelectCharacter((CharacterId)u.Param1),
                        UnaryType.SetCharacterIsRandomSkin => SetCharacterIsRandomSkin((CharacterId)u.Param1, u.Param2 != 0),
                        UnaryType.SelectCharacterSkin => SelectCharacterSkin((CharacterId)u.Param1, (SkinId)u.Param2),
                        UnaryType.SetCharacterSkinIsRandomSelected => SetCharacterSkinIsRandomSelected((CharacterId)u.Param1, (SkinId)u.Param2, u.Param3 != 0),
                        UnaryType.SetEmoji => SetEmoji((EmojiId)u.Param1),
                        UnaryType.SetGameArgs => SetGameArgs((GameMode)u.Param1),
                        UnaryType.ReadyGame => ReadyGame(),
                        UnaryType.ReturnGame => ReturnGame(),
                        UnaryType.CancelGame => CancelGame(),
                        UnaryType.PinSpecialty => PinSpecialty((SpecialtyId)u.Param1, u.Param2 != 0),
                        UnaryType.SetWelcome => SetWelcome(),
                        _ => throw new InvalidOperationException()
                    });
            }
        }

        internal void Reset()
        {
            Result = null;
        }

        private bool SelectCharacter(CharacterId id)
        {
            _characters.Select(id);
            return true;
        }

        private bool SetCharacterIsRandomSkin(CharacterId id, bool value)
        {
            _characters.SetIsRandomSkin(id, value);
            return true;
        }

        private bool SelectCharacterSkin(CharacterId id, SkinId skinId)
        {
            _characters.SelectSkin(id, skinId);
            return true;
        }

        private bool SetCharacterSkinIsRandomSelected(CharacterId id, SkinId skinId, bool value)
        {
            _characters.SetSkinIsRandomSelected(id, skinId, value);
            return true;
        }

        private bool SetEmoji(EmojiId id)
        {
            _emoji.Set(id);
            return true;
        }

        private bool SetGameArgs(GameMode args)
        {
            _gameManager.SelectArgs(args);
            return true;
        }

        private bool ReadyGame()
        {
            _gameManager.Ready();
            return true;
        }

        private bool ReturnGame()
        {
            _gameManager.Return();
            return true;
        }

        private bool CancelGame()
        {
            _gameManager.Cancel();
            return true;
        }

        private bool PinSpecialty(SpecialtyId id, bool value)
        {
            _object.PinSpecialty(id, value);
            return true;
        }

        private bool SetWelcome()
        {
            _welcome.Welcome = true;
            return true;
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
