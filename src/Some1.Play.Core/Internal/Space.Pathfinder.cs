using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed partial class Space
    {
        private Pathfinder GetPathfinder(Area area)
        {
            var state = s_pathfinderState.Value!;
            state.Use();
            state.Space = this;
            state.Area = area;

            return new(state);
        }

        private readonly ref struct Pathfinder
        {
            private readonly PathfinderState _state;

            public Pathfinder(PathfinderState state) => _state = state;

            public void Dispose() => _state.Unuse();

            internal Aim? Find(Move move) => _state.Find(move);
        }

        private sealed class PathfinderState
        {
            private readonly Stack<Node> _nodePool = new();
            private readonly Dictionary<Point, Node> _closeList = new();
            private readonly Dictionary<Point, Node> _openList = new();
            private bool _used;
            private int _lastSequence;

            public Space? Space { get; set; }
            public Area? Area { get; set; }

            public void Use()
            {
                if (_used)
                {
                    throw new InvalidOperationException();
                }
                _used = true;
            }

            public void Unuse()
            {
                if (!_used)
                {
                    return;
                }
                _used = false;
                Space = null;
                Area = null;
            }

            public Aim? Find(Move move)
            {
                if (!_used)
                {
                    throw new InvalidOperationException();
                }

                if (move.Source.Size.Width <= 0
                    || move.Source.Size.Width > 1
                    || move.BumpLevel is null
                    || !move.Walk
                    || move.Delta == Vector2.Zero)
                {
                    throw new ArgumentOutOfRangeException(nameof(move), $"Move is {move}.");
                }

                var startPosition = move.Source.Position;
                var endPosition = move.Destination.Position;
                var startNodeId = ToNodeId(startPosition);
                var endNodeId = ToNodeId(endPosition);

                var startNode = New(startNodeId, endNodeId);
                _openList.Add(startNode.Id, startNode);

                Node? current;
                while (true)
                {
                    current = MoveNext(endNodeId, move.BumpLevel.Value);
                    if (current is null || current.IsEnd)
                    {
                        break;
                    }
                }

                //var aim = current?.GetAim(startPosition);
                var aim = current?.GetAim(move.Source, Space!, move.BumpLevel.Value);
                Clear();
                return aim;
            }

            private static Point ToNodeId(Vector2 position) => new((int)MathF.Floor(position.X), (int)MathF.Floor(position.Y));
            private static Vector2 ToVector2(Point nodeId) => new(nodeId.X, nodeId.Y);
            private static Point Left(Point nodeId) => new(nodeId.X - 1, nodeId.Y);
            private static Point Right(Point nodeId) => new(nodeId.X + 1, nodeId.Y);
            private static Point Down(Point nodeId) => new(nodeId.X, nodeId.Y - 1);
            private static Point Up(Point nodeId) => new(nodeId.X, nodeId.Y + 1);
            private static Point LeftDown(Point nodeId) => new(nodeId.X - 1, nodeId.Y - 1);
            private static Point LeftUp(Point nodeId) => new(nodeId.X - 1, nodeId.Y + 1);
            private static Point RightDown(Point nodeId) => new(nodeId.X + 1, nodeId.Y - 1);
            private static Point RightUp(Point nodeId) => new(nodeId.X + 1, nodeId.Y + 1);

            private void Clear()
            {
                foreach (var item in _closeList.Values)
                {
                    item.Reset();
                    _nodePool.Push(item);
                }
                _closeList.Clear();
                foreach (var item in _openList.Values)
                {
                    item.Reset();
                    _nodePool.Push(item);
                }
                _openList.Clear();
                _lastSequence = 0;
            }

            private Node? MoveNext(Point endNodeId, BumpLevel bumpLevel)
            {
                var node = GetNext();

                if (node is null || node.IsEnd)
                {
                    return node;
                }

                _openList.Remove(node.Id);
                _closeList.Add(node.Id, node);

                Go(Left(node.Id), node, endNodeId, bumpLevel);
                Go(Right(node.Id), node, endNodeId, bumpLevel);
                Go(Down(node.Id), node, endNodeId, bumpLevel);
                Go(Up(node.Id), node, endNodeId, bumpLevel);
                Go(LeftDown(node.Id), node, endNodeId, bumpLevel);
                Go(LeftUp(node.Id), node, endNodeId, bumpLevel);
                Go(RightDown(node.Id), node, endNodeId, bumpLevel);
                Go(RightUp(node.Id), node, endNodeId, bumpLevel);

                return node;
            }

            private Node? GetNext()
            {
                Node? result = null;
                foreach (var item in _openList.Values)
                {
                    if (result is null
                        || result.F_Score > item.F_Score
                        || (result.F_Score == item.F_Score && result.Sequence < item.Sequence))
                    {
                        result = item;
                    }
                }
                return result;
            }

            private void Go(Point id, Node parent, Point endNodeId, BumpLevel bumpLevel)
            {
                if (Space is null || Area is null)
                {
                    throw new InvalidOperationException();
                }

                if (_closeList.ContainsKey(id))
                {
                    return;
                }

                if (_openList.TryGetValue(id, out var openNode))
                {
                    openNode.SetParent(parent);
                    return;
                }

                var nodeArea = Play.Info.Area.Rectangle(ToVector2(id).ToPointF(), new SizeF(1, 1));

                if (!Area.Value.IntersectsWith(nodeArea))
                {
                    return;
                }

                if (Space.Bump(id, bumpLevel))
                {
                    return;
                }

                bool diagonal = parent.Id.X != id.X && parent.Id.Y != id.Y;
                if (diagonal)
                {
                    if (Space.Bump(new(parent.Id.X, id.Y), bumpLevel))
                    {
                        return;
                    }

                    if (Space.Bump(new(id.X, parent.Id.Y), bumpLevel))
                    {
                        return;
                    }
                }

                var node = New(id, endNodeId);
                node.SetParent(parent);
                _openList.Add(node.Id, node);
            }

            private Node New(Point id, Point endNodeId)
            {
                if (!_nodePool.TryPop(out var node))
                {
                    node = new();
                }

                node.Setup(id, endNodeId, ++_lastSequence);
                return node;
            }

            private sealed class Node
            {
                private bool _setup;

                public Point Id { get; private set; }
                public Area Area => Area.Rectangle(ToVector2(Id).ToPointF(), new SizeF(1, 1));
                public int Sequence { get; private set; }
                public Node? Parent { get; private set; }
                public float F_Score => G_Score + H_Score;
                public float G_Score { get; private set; }
                public float H_Score { get; private set; }
                public bool IsStart => _setup && Parent is null;
                public bool IsEnd => _setup && H_Score == 0;

                public void Setup(Point id, Point endNodeId, int sequence)
                {
                    if (_setup)
                    {
                        throw new InvalidOperationException();
                    }

                    _setup = true;
                    Id = id;
                    Sequence = sequence;
                    H_Score = Math.Abs(endNodeId.X - id.X) + Math.Abs(endNodeId.Y - id.Y);
                }

                public void SetParent(Node parent)
                {
                    if (!_setup)
                    {
                        throw new InvalidOperationException();
                    }

                    if (Parent == parent)
                    {
                        return;
                    }

                    Debug.Assert(Id != parent.Id && (Math.Abs(Id.X - parent.Id.X) <= 1) && (Math.Abs(Id.Y - parent.Id.Y) <= 1));

                    float g = parent.G_Score + (Id.X == parent.Id.X || Id.Y == parent.Id.Y ? 1 : 1.414f);

                    if (Parent is not null && g > G_Score)
                    {
                        return;
                    }

                    Parent = parent;
                    G_Score = g;
                }

                public Aim? GetAim(Vector2 startPosition)
                {
                    if (Parent is null)
                    {
                        return null;
                    }

                    var root = this;
                    Node? rootNext = null;
                    while (root.Parent is not null)
                    {
                        rootNext = root;
                        root = root.Parent;
                    }

                    return rootNext is null
                        ? null
                        : new(Vector2Helper.AngleBetween(startPosition, rootNext.Area.Position), G_Score);
                }

                public Aim? GetAim(Area startArea, Space space, BumpLevel bumpLevel)
                {
                    if (Parent is null)
                    {
                        return null;
                    }

                    var root = this;
                    Node? rootNext = null;
                    while (root.Parent is not null)
                    {
                        rootNext = root;
                        root = root.Parent;
                    }

                    if (rootNext is null)
                    {
                        return null;
                    }

                    if (space.CheckMove(new(startArea, rootNext.Area.Position - startArea.Position, bumpLevel, true)))
                    {
                        return new(Vector2Helper.AngleBetween(startArea.Position, rootNext.Area.Position), G_Score);
                    }

                    return new(Vector2Helper.AngleBetween(startArea.Position, root.Area.Position), G_Score);
                }

                public void Reset()
                {
                    _setup = false;
                    Id = Point.Empty;
                    Sequence = 0;
                    Parent = null;
                    G_Score = 0;
                    H_Score = 0;
                }
            }
        }
    }
}
