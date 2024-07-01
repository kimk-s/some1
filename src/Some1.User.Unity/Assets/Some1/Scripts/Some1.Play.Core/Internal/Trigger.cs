using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class Trigger : IDisposable
    {
        private readonly Func<IReadOnlyList<TriggerInfo>?> _getInfos;
        private readonly ITriggerEventManager _eventManager;
        private readonly ITriggerTargetHandler _targetHandler;
        private readonly ITriggerCommandHandler _commandHandler;
        private readonly DestinationCollection _destinations = new();
        private readonly HashSet<int>[] _destinationUniques;
        private bool _disposed;

        internal Trigger(
            Func<IReadOnlyList<TriggerInfo>?> getInfos,
            ITriggerEventManager eventManager,
            ObjectHierarchy hierarchy,
            Space space)
        {
            _getInfos = getInfos;
            _eventManager = eventManager;
            _eventManager.EventFired += EventManager_EventFired;
            _eventManager.ScopedReset += EventManager_ScopedReset;
            _targetHandler = new TriggerTargetHandler(_eventManager.State, hierarchy, space);
            _commandHandler = new TriggerCommandHandler(_eventManager.State, hierarchy.Self.Id);
            _destinationUniques = new HashSet<int>[EnumForUnity.GetValues<TriggerDestinationUniqueId>().Length];
            for (int i = 0; i < _destinationUniques.Length; i++)
            {
                _destinationUniques[i] = new();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _eventManager.EventFired -= EventManager_EventFired;
                _eventManager.Dispose();

                _disposed = true;
            }
        }

        internal bool ContainsDestinationUnique(TriggerDestinationUniqueId id, int objectId)
        {
            return _destinationUniques[(int)id].Contains(objectId);
        }

        internal void Reset()
        {
            _eventManager.State.Reset();
            foreach (var item in _destinationUniques.AsSpan())
            {
                item.Clear();
            }
        }

        private void EventManager_EventFired(object? _, ParallelToken e)
        {
            var infos = _getInfos();
            if (infos is null)
            {
                return;
            }

            int count = infos.Count;
            for (int i = 0; i < count; i++)
            {
                var info = infos[i];

                int contains = _eventManager.Contains(info.Event);
                if (contains < 1)
                {
                    continue;
                }

                if (!VerifyCondition(info.Condition))
                {
                    continue;
                }

                for (int j = 0; j < contains; j++)
                {
                    Handle(info, e);
                }
            }
        }

        private void Handle(TriggerInfo info, ParallelToken parallelToken)
        {
            int handled = Handle(info.DestinationUnique, info.Target, info.ObjectTarget, info.Command, parallelToken);

            var posts = info.Posts;
            if (handled > 0 && posts is not null)
            {
                int count = posts.Count;
                for (int i = 0; i < count; i++)
                {
                    var post = posts[i];

                    switch (post.Condition)
                    {
                        case TriggerPostConditionInfo.PerHandled:
                            {
                                for (int j = 0; j < handled; j++)
                                {
                                    Handle(post, parallelToken);
                                }
                            }
                            break;
                        case TriggerPostConditionInfo.Once:
                            {
                                Handle(post, parallelToken);
                            }
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        private void Handle(TriggerPostInfo info, ParallelToken parallelToken)
        {
            Handle(info.DestinationUnique, info.Target, info.Destination, info.Command, parallelToken);
        }

        private int Handle(
            TriggerDestinationUniqueInfo destinationUniqueInfo,
            ITriggerTargetInfo targetInfo,
            ObjectTargetInfo destinationInfo,
            ITriggerCommandInfo commandInfo,
            ParallelToken parallelToken)
        {
            using var destinationsOwner = GetDestinationCollectionOwner(destinationUniqueInfo);
            _targetHandler.Handle(targetInfo, destinationInfo, destinationsOwner.Collection, parallelToken);

            int result = 0;
            foreach (var @object in CollectionsMarshal.AsSpan(destinationsOwner.Collection.List))
            {
                if (_commandHandler.Handle(commandInfo, @object, parallelToken))
                {
                    ++result;
                }
            }
            return result;
        }

        private void EventManager_ScopedReset(object? _, EventArgs __)
        {
            foreach (var item in _destinationUniques.AsSpan())
            {
                item.Clear();
            }
        }

        private bool VerifyCondition(TriggerConditionInfo condition)
            => (((condition.Trait & _eventManager.State.Source.Trait) != Trait.None) || _eventManager.State.Source.Trait == Trait.None)
            && condition.NextTrait.HasFlag(_eventManager.State.Source.NextTrait)
            && condition.Probability > RandomForUnity.NextSingle();

        private DestinationCollectionOwner GetDestinationCollectionOwner(TriggerDestinationUniqueInfo uniqueInfo)
            => new(_destinations, _destinationUniques[(int)uniqueInfo.Id], uniqueInfo.Id == TriggerDestinationUniqueId.Transient);

        private readonly ref struct DestinationCollectionOwner
        {
            private readonly bool _clearUniqueOnDispose;

            internal DestinationCollectionOwner(
                DestinationCollection collection,
                HashSet<int> unique,
                bool clearUniqueOnDispose)
            {
                if (unique is null)
                {
                    throw new ArgumentNullException(nameof(unique));
                }

                collection.Unique = unique;
                Collection = collection;
                _clearUniqueOnDispose = clearUniqueOnDispose;
            }

            internal DestinationCollection Collection { get; }

            public readonly void Dispose()
            {
                if (_clearUniqueOnDispose)
                {
                    Collection.Unique!.Clear();
                }
                Collection.Unique = null;
                Collection.Clear();
            }
        }

        private sealed class DestinationCollection : ICollection<Object>
        {
            public int Count => List.Count;

            public bool IsReadOnly => ((ICollection<Object>)List).IsReadOnly;

            internal List<Object> List { get; } = new();

            internal HashSet<int>? Unique { get; set; }

            public void Add(Object item)
            {
                if (Unique is null)
                {
                    throw new InvalidOperationException();
                }

                if (Unique.Add(item.Id))
                {
                    List.Add(item);
                }
            }

            public void Clear() => List.Clear();

            public bool Contains(Object item) => List.Contains(item);

            public void CopyTo(Object[] array, int arrayIndex) => List.CopyTo(array, arrayIndex);

            public IEnumerator<Object> GetEnumerator() => List.GetEnumerator();

            public bool Remove(Object item) => List.Remove(item);

            IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();
        }
    }
}
