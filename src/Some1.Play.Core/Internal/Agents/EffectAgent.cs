using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal.Agents
{
    internal sealed class EffectAgent : IAgent
    {
        private readonly Object _object;
        private readonly ITime _time;
        private double _setTime;

        internal EffectAgent(Object @object, ITime time)
        {
            _object = @object;
            _time = time;
        }

        public void Reset()
        {
            
        }

        public void Set(ICharacterAgentInfo? info)
        {
            _setTime = _time.TotalSeconds;
        }

        public void Set(Aim aim)
        {
            _object.SetAim(aim);
        }

        public void Stop()
        {
            
        }

        public void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            if (_time.TotalSeconds - _setTime > PlayConst.MaxEffectTime)
            {
                _object.ResetOrReserve(parallelToken);
            }
        }
    }
}
