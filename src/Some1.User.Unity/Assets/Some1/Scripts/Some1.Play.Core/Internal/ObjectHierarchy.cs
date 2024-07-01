using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectHierarchy : IObjectHierarchy
    {
        private readonly List<Object> _objects = new();

        internal ObjectHierarchy(Object self, Object? @parent, ObjectChildGroup children)
        {
            Self = self;
            Parent = parent;
            Root = Parent is null ? self : Parent.Hierarchy.Root;
            Children = children;
        }

        IObject? IObjectHierarchy.Parent => Parent;

        IObject IObjectHierarchy.Root => Root;

        internal Object? Parent { get; }

        internal Object Root { get; }

        internal ObjectChildGroup Children { get; }

        internal Object Self { get; }

        internal void GetObjects(HierarchyTarget target, ObjectTarget objectTarget, ICollection<Object> results)
        {
            if (target.HasFlag(HierarchyTarget.Self))
            {
                if (!(target.HasFlag(HierarchyTarget.Siblings) && Parent is not null))
                {
                    AddObject(objectTarget, Self, results);
                }
            }

            if (target.HasFlag(HierarchyTarget.Ancestors))
            {
                AddParent(objectTarget, Parent, results, true);
            }
            else
            {
                if (target.HasFlag(HierarchyTarget.Root))
                {
                    if (!(target.HasFlag(HierarchyTarget.Self) && Self == Root))
                    {
                        AddObject(objectTarget, Root, results);
                    }
                }

                if (target.HasFlag(HierarchyTarget.Parant))
                {
                    if (!(target.HasFlag(HierarchyTarget.Root) && Root == Parent))
                    {
                        AddParent(objectTarget, Parent, results, false);
                    }
                }
            }

            if (target.HasFlag(HierarchyTarget.Descendants))
            {
                AddChildren(objectTarget, Children, results, true);
            }
            else
            {
                if (target.HasFlag(HierarchyTarget.Children))
                {
                    AddChildren(objectTarget, Children, results, false);
                }
            }

            if (target.HasFlag(HierarchyTarget.Siblings))
            {
                if (Parent is not null)
                {
                    AddChildren(objectTarget, Parent.Hierarchy.Children, results, false);
                }
            }

            static void AddParent(ObjectTarget objectTarget, Object? parent, ICollection<Object> results, bool recursively)
            {
                if (parent is not null)
                {
                    AddObject(objectTarget, parent, results);

                    if (recursively)
                    {
                        AddParent(objectTarget, parent.Hierarchy.Parent, results, recursively);
                    }
                }
            }

            static void AddChildren(ObjectTarget objectTarget, ObjectChildGroup children, ICollection<Object> results, bool recursively)
            {
                for (int i = 0; i < children.All.Count; i++)
                {
                    var child = children.All[i].Object;

                    AddObject(objectTarget, child, results);

                    if (recursively)
                    {
                        AddChildren(objectTarget, child.Hierarchy.Children, results, recursively);
                    }
                }
            }

            static void AddObject(ObjectTarget objectTarget, Object @object, ICollection<Object> results)
            {
                if (@object.IsTarget(objectTarget))
                {
                    results.Add(@object);
                }
            }
        }

        internal void Reset()
        {
            _objects.Clear();
        }
    }
}
