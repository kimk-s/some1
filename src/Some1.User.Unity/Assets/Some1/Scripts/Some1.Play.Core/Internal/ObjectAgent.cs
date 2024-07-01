using System;
using System.Collections.Generic;
using System.Linq;
using Some1.Play.Core.Internal.Agents;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectAgent
    {
        private readonly Dictionary<Type, IAgent> _agents;
        private readonly Object _object;
        private IAgent? _agent;

        internal ObjectAgent(Object @object, Space space, ITime time)
        {
            _agents = new IAgent[]
            {
                new BattleAgent(@object, time),
                new EffectAgent(@object, time),
                new DefaultAgent(@object),
            }.ToDictionary(x => x.GetType());
            _object = @object;
        }

        internal void Set(ICharacterAgentInfo? info)
        {
            _agent?.Reset();
            _agent = _agents[GetAgentType(info)];
            _agent.Set(info);
        }

        internal void Set(Aim aim)
        {
            _agent?.Set(aim);
        }

        internal void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            _agent?.Update(deltaSeconds, parallelToken);
        }

        internal void Stop()
        {
            _agent?.Stop();
        }

        internal void Reset()
        {
            _agent?.Reset();
            _agent = null;
        }

        private Type GetAgentType(ICharacterAgentInfo? info) => info switch
        {
            BattleCharacterAgentInfo _ => typeof(BattleAgent),
            null => _object.Character.Info!.Type == CharacterType.Effect ? typeof(EffectAgent) : typeof(DefaultAgent),
            _ => throw new NotImplementedException(),
        };
    }
}
