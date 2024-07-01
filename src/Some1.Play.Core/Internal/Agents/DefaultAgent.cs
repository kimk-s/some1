using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal.Agents
{
    internal sealed class DefaultAgent : IAgent
    {
        private readonly Object _object;

        internal DefaultAgent(Object @object)
        {
            _object = @object;
        }

        public void Set(ICharacterAgentInfo? info)
        {
        }

        public void Set(Aim aim)
        {
            _object.SetAim(aim);
        }

        public void Update(float deltaSeconds, ParallelToken parallelToken)
        {
        }

        public void Stop()
        {
        }

        public void Reset()
        {
        }
    }
}
