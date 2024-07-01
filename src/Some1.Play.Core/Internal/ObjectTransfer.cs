using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectTransfer : IObjectTransfer
    {
        private readonly Object _object;
        private readonly Space _space;
        private readonly ObjectTransferMoveFailed _moveFailed;
        private CharacterMoveInfo? _moveInfo;
        private CharacterWalkInfo? _walkInfo;
        private bool _isMoveBlocked;
        private Vector2 _moveDelta;

        internal ObjectTransfer(Object @object, Space space)
        {
            _object = @object;
            _space = space;
            _moveFailed = new(this);
        }

        public Vector2 LastWarpPosition { get; private set; }

        public bool IsMoveBlocked
        {
            get => _isMoveBlocked;
            internal set
            {
                MoveDelta = Vector2.Zero;
                _isMoveBlocked = value;
            }
        }

        public Vector2 MoveDelta
        {
            get => _moveDelta;
            internal set
            {
                if (_moveDelta == value)
                {
                    return;
                }
                if (IsMoveBlocked)
                {
                    return;
                }
                _moveDelta = value;
            }
        }

        public IObjectTransferMoveFailed MoveFailed => _moveFailed;

        internal void Set(CharacterMoveInfo? moveInfo, CharacterWalkInfo? walkInfo)
        {
            _moveInfo = moveInfo;
            _walkInfo = walkInfo;
        }

        internal void Set(Vector2 position, ParallelToken? parallelToken)
        {
            _space.SetObjectPosition(_object, new(position), parallelToken);
            IsMoveBlocked = true;
        }

        internal void Teleport(Vector2 delta, ParallelToken? parallelToken)
        {
            InternalTeleport(delta, parallelToken);
            IsMoveBlocked = true;
        }

        internal void Warp(Vector2 position, ParallelToken? parallelToken)
        {
            InternalWarp(position, parallelToken);
            IsMoveBlocked = true;
            LastWarpPosition = position;
        }

        internal void Update(ParallelToken parallelToken)
        {
            var b = _object.Transform.Position.CurrentValue.B;

            MovesWithEvent(MoveDelta, parallelToken);

            if (b == _object.Transform.Position.CurrentValue.B)
            {
                _space.SetObjectPosition(_object, new(_object.Transform.Position.CurrentValue.B), parallelToken);
            }

            IsMoveBlocked = false;
        }

        internal void Reset(ParallelToken? parallelToken)
        {
            Set(Vector2.Zero, parallelToken);
            IsMoveBlocked = false;
            _moveInfo = null;
            _walkInfo = null;
        }

        private bool InternalTeleport(Vector2 delta, ParallelToken? parallelToken)
        {
            if (delta.Length() > PlayConst.MaxTeleportDeltaLength)
            {
                throw new ArgumentOutOfRangeException(nameof(delta));
            }

            if (delta == Vector2.Zero)
            {
                return true;
            }

            _space.SetObjectPosition(_object, new(_object.Transform.Position.CurrentValue.B + delta), parallelToken);

            return false;
        }

        private bool InternalWarp(Vector2 position, ParallelToken? parallelToken)
        {
            _space.SetObjectPosition(_object, new(position), parallelToken);

            return false;
        }

        private void MovesWithEvent(Vector2 delta, ParallelToken parallelToken)
        {
            if (delta == Vector2.Zero)
            {
                return;
            }

            if (!MoveOrSnap(delta, parallelToken))
            {
                _moveFailed.InvokeEventFired(parallelToken);
                _moveFailed.InvokeStopped();
            }
        }

        private bool MoveOrSnap(Vector2 delta, ParallelToken? parallelToken)
        {
            if (_moveInfo is null)
            {
                return false;
            }

            if (delta == Vector2.Zero)
            {
                return true;
            }

            if (Move(delta, parallelToken))
            {
                return true;
            }

            Snap(delta, parallelToken);
            return false;
        }

        private void Snap(Vector2 delta, ParallelToken? parallelToken)
        {
            if (Snap(delta, false, true, parallelToken))
            {
                return;
            }

            if (Snap(delta, true, false, parallelToken))
            {
                return;
            }

            Snap(delta, true, true, parallelToken);
        }

        private bool Snap(Vector2 delta, bool snapX, bool snapY, ParallelToken? parallelToken)
        {
            // --------------------------
            // Works only positive area
            // --------------------------

            var source = _object.Properties.Area;

            float x = delta.X;
            if (snapX)
            {
                if (delta.X < 0)
                {
                    float a = source.Left;
                    float b = a + delta.X;

                    if (GetIntegerPart(a) != GetIntegerPart(b))
                    {
                        x = MathF.Floor(source.Left) - source.Left;
                    }
                }
                else if (delta.X > 0)
                {
                    float a = source.Right;
                    float b = a + delta.X;

                    if (GetDecimalPart(a) == 0)
                    {
                        x = 0;
                    }
                    else if (GetIntegerPart(a) != GetIntegerPart(b))
                    {
                        x = MathF.Ceiling(source.Right) - source.Right;
                    }
                }
            }

            float y = delta.Y;
            if (snapY)
            {
                if (delta.Y < 0)
                {
                    float a = source.Top;
                    float b = a + delta.Y;

                    if (GetIntegerPart(a) != GetIntegerPart(b))
                    {
                        y = MathF.Floor(source.Top) - source.Top;
                    }
                }
                else if (delta.Y > 0)
                {
                    float a = source.Bottom;
                    float b = a + delta.Y;

                    if (GetDecimalPart(a) == 0)
                    {
                        y = 0;
                    }
                    else if (GetIntegerPart(a) != GetIntegerPart(b))
                    {
                        y = MathF.Ceiling(source.Bottom) - source.Bottom;
                    }
                }
            }

            return Move(new(x, y), parallelToken);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static int GetIntegerPart(float value) => (int)MathF.Truncate(value);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static float GetDecimalPart(float value) => value - GetIntegerPart(value);
        }

        //private void SnapOldVersion(Vector2 delta, ParallelToken? parallelToken)
        //{
        //    const float Adjust = 0.01f;

        //    if (MathF.Abs(delta.X) > Adjust)
        //    {
        //        if (Move(new(Adjust * (delta.X < 0 ? -1 : 1), delta.Y), parallelToken))
        //        {
        //            return;
        //        }
        //    }

        //    if (MathF.Abs(delta.Y) > Adjust)
        //    {
        //        if (Move(new(delta.X, Adjust * (delta.Y < 0 ? -1 : 1)), parallelToken))
        //        {
        //            return;
        //        }
        //    }

        //    if (Move(new(0, delta.Y), parallelToken))
        //    {
        //        return;
        //    }

        //    if (Move(new(delta.X, 0), parallelToken))
        //    {
        //        return;
        //    }

        //    if (MathF.Abs(delta.X) > Adjust && MathF.Abs(delta.Y) > Adjust)
        //    {
        //        if (Move(new(Adjust * (delta.X < 0 ? -1 : 1), Adjust * (delta.Y < 0 ? -1 : 1)), parallelToken))
        //        {
        //            return;
        //        }
        //    }
        //}

        private bool Move(Vector2 delta, ParallelToken? parallelToken)
        {
            if (_moveInfo is null)
            {
                return false;
            }

            var bumpLevel = !_object.Alive.Alive.CurrentValue
                ? (BumpLevel?)null
                : _object.Shift.Shift.CurrentValue?.Id.GetBumpLevel() ?? _moveInfo.BumpLevel;

            var move = new Move(
                _object.Properties.Area,
                delta,
                bumpLevel,
                _walkInfo is not null);

            Debug.Assert((move.Source.Position - _object.Transform.Position.CurrentValue.B).LengthSquared() < MathF.Pow(0.001f, 2));

            if (!_space.CheckMove(move))
            {
                return false;
            }

            var position = _object.Transform.Position.CurrentValue.Flow(delta);
            Debug.Assert((move.Destination.Position - position.B).LengthSquared() < MathF.Pow(0.001f, 2));
            _space.SetObjectPosition(_object, position, parallelToken);
            return true;
        }

        private sealed class ObjectTransferMoveFailed : IObjectTransferMoveFailed
        {
            private readonly object _sender;

            internal ObjectTransferMoveFailed(object sender)
            {
                _sender = sender;
            }

            public event EventHandler<(EmptyTriggerEventArgs e, ParallelToken parallelToken)>? EventFired;
            public event EventHandler? ScopedReset;

            internal void InvokeEventFired(ParallelToken e)
            {
                EventFired?.Invoke(_sender, (default, e));
            }

            internal void InvokeStopped()
            {
                ScopedReset?.Invoke(_sender, EventArgs.Empty);
            }
        }
    }
}
