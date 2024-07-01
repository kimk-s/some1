using System;
using System.Diagnostics;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class Leader : ILeader
    {
        private readonly Object _object;
        private readonly Space _space;
        private readonly LeaderToken _token = new();

        internal Leader(Object @object, Space space)
        {
            _object = @object;
            _space = space;
        }

        public event EventHandler<(LeaderStatus, ParallelToken)>? LeadCompleted;

        public bool IsEnabled { get; private set; }

        public LeaderStatus Status { get; private set; }

        public ILeaderToken Token => _token;

        internal void Enable()
        {
            if (IsEnabled)
            {
                throw new InvalidOperationException();
            }

            IsEnabled = true;
        }

        internal void Disable()
        {
            if (!IsEnabled)
            {
                return;
            }

            IsEnabled = false;
            Status = LeaderStatus.None;
        }

        internal bool Lead1(ParallelToken parallelToken)
        {
            if (!IsEnabled)
            {
                return false;
            }

            if (Status != LeaderStatus.None && Status != LeaderStatus.Lead3Completed)
            {
                throw new InvalidOperationException();
            }

            bool result = _object.Update(true, parallelToken);
            Debug.Assert(result);

            _token.SetArea(Area.Rectangle(_object.Transform.Position.CurrentValue.B, PlayConst.LeaderEyesightSize));
            _space.UpdateNonLeaderObjects(_token, parallelToken);

            Status = LeaderStatus.Lead1Completed;
            RaiseLeadCompleted(parallelToken);

            return true;
        }

        internal bool Lead2(ParallelToken parallelToken)
        {
            if (Status != LeaderStatus.Lead1Completed)
            {
                return false;
            }

            _space.UpdateBlocks(_token);
            _token.ResetArea();

            Status = LeaderStatus.Lead2Completed;
            RaiseLeadCompleted(parallelToken);

            return true;
        }

        internal bool Lead3(ParallelToken parallelToken)
        {
            if (Status != LeaderStatus.Lead2Completed)
            {
                return false;
            }

            Status = LeaderStatus.Lead3Completed;
            RaiseLeadCompleted(parallelToken);

            return true;
        }

        private void RaiseLeadCompleted(ParallelToken parallelToken)
        {
            LeadCompleted?.Invoke(this, (Status, parallelToken));
        }
    }

    internal static class LeaderExtensions
    {
        public static void Lead(this Leader leader, int leadNumber, ParallelToken parallelToken)
        {
            switch (leadNumber)
            {
                case 1: leader.Lead1(parallelToken); break;
                case 2: leader.Lead2(parallelToken); break;
                case 3: leader.Lead3(parallelToken); break;
                default: throw new InvalidOperationException();
            }
        }
    }
}
