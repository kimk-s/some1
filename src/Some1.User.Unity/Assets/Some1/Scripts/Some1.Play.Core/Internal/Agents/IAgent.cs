using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal.Agents
{
    internal interface IAgent
    {
        void Set(ICharacterAgentInfo? info);
        void Set(Aim aim);
        void Update(float deltaSeconds, ParallelToken parallelToken);
        void Stop();
        void Reset();
    }
}
