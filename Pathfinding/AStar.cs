/*
The MIT License

Copyright (c) 2015 Martin Pichlmair
Copyright (c) 2010 Christoph Husse

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using UnityEngine;
using System.Collections.Generic;
using Unitilities.Collections.Generic;

namespace Unitilities.Pathfinding {
	
    public interface IPathNode<TUserContext>
    {
		int X {get;}
		int Y {get;}
		bool IsWalkable(TUserContext userContext);
		float Heuristic(IPathNode<TUserContext> toNode, TUserContext userContext);
	}

    /// <summary>
    /// Uses about 50 MB for a 1024x1024 grid.
    /// </summary>
	public class AStar<TPathNode, TUserContext> where TPathNode : IPathNode<TUserContext>
    {
		private OpenCloseMap m_ClosedSet;
		private OpenCloseMap m_OpenSet;
		private PriorityQueue<PathNode> m_OrderedOpenSet;
		private PathNode[,] m_CameFrom;
		private OpenCloseMap m_RuntimeGrid;
		private PathNode[,] m_SearchSpace;
		private bool m_fourWay = true;

        public TPathNode[,] SearchSpace { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        protected class PathNode : IPathNode<TUserContext>, IComparer<PathNode>, IIndexedObject
        {
            public static readonly PathNode Comparer = new PathNode(0, 0, default(TPathNode));

            public TPathNode UserContext { get; internal set; }
            public float G { get; internal set; }
            public float H { get; internal set; }
            public float F { get; internal set; }
            public int Index { get; set; }

            public bool IsWalkable(TUserContext inContext)
            {
                return UserContext.IsWalkable(inContext);
            }

			public float Heuristic(IPathNode<TUserContext> toNode, TUserContext userContext) {
				return Mathf.Sqrt((float)((X - toNode.X) * (X - toNode.X + (Y - toNode.Y) * (Y - toNode.Y))));
			}

            public int X { get; internal set; }
            public int Y { get; internal set; }

			public int Compare(PathNode x, PathNode y)
            {
                if (x.F < y.F)
                    return -1;
                else if (x.F > y.F)
                    return 1;

                return 0;
            }

            public PathNode(int inX, int inY, TPathNode inUserContext)
            {
                X = inX;
                Y = inY;
                UserContext = inUserContext;
            }
        }

        public AStar(TPathNode[,] inGrid)
        {
            SearchSpace = inGrid;
            Width = inGrid.GetLength(0);
            Height = inGrid.GetLength(1);
            m_SearchSpace = new PathNode[Width, Height];
            m_ClosedSet = new OpenCloseMap(Width, Height);
            m_OpenSet = new OpenCloseMap(Width, Height);
            m_CameFrom = new PathNode[Width, Height];
            m_RuntimeGrid = new OpenCloseMap(Width, Height);
			m_OrderedOpenSet = new PriorityQueue<PathNode>(PathNode.Comparer);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (inGrid[x, y] == null)
                        throw new System.ArgumentNullException("Grid not specified at "+x+"/"+y);

                    m_SearchSpace[x, y] = new PathNode(x, y, inGrid[x, y]);
                }
            }
        }

//		protected virtual float Heuristic(PathNode inStart, PathNode inEnd, TUserContext inUserContext)
//        {
//            return Mathf.Sqrt((inStart.X - inEnd.X) * (inStart.X - inEnd.X) + (inStart.Y - inEnd.Y) * (inStart.Y - inEnd.Y));
//        }

        private static readonly float SQRT_2 = Mathf.Sqrt(2);

		protected virtual float NeighborDistance(PathNode inStart, PathNode inEnd)
        {
            int diffX = Mathf.Abs(inStart.X - inEnd.X);
            int diffY = Mathf.Abs(inStart.Y - inEnd.Y);

            switch (diffX + diffY)
            {
                case 1: return 1f;
				case 2: return SQRT_2;
                case 0: return 0f;
                default:
					return 0f;
            }
        }

        //private List<Int64> elapsed = new List<long>();

        /// <summary>
        /// Returns null, if no path is found. Start- and End-Node are included in returned path. The user context
        /// is passed to IsWalkable().
        /// </summary>
		public LinkedList<TPathNode> Search(Vector2 inStartNode, Vector2 inEndNode, TUserContext inUserContext)
        {
			inStartNode.x = Mathf.Clamp(inStartNode.x, 0f, Width-1);
			inStartNode.y = Mathf.Clamp(inStartNode.y, 0f, Height-1);
			inEndNode.x = Mathf.Clamp(inEndNode.x, 0f, Width-1);
			inEndNode.y = Mathf.Clamp(inEndNode.y, 0f, Height-1);

			PathNode startNode = m_SearchSpace[(int)inStartNode.x, (int)inStartNode.y];
			PathNode endNode = m_SearchSpace[(int)inEndNode.x, (int)inEndNode.y];

            //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            //watch.Start();

            if (startNode == endNode)
                return new LinkedList<TPathNode>(new TPathNode[] { startNode.UserContext });

            PathNode[] neighborNodes = new PathNode[8];

            m_ClosedSet.Clear();
            m_OpenSet.Clear();
            m_RuntimeGrid.Clear();
            m_OrderedOpenSet.Clear();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    m_CameFrom[x, y] = null;
                }
            }

            startNode.G = 0;
			startNode.H = startNode.UserContext.Heuristic(endNode, inUserContext); //Heuristic(startNode, endNode, inUserContext);
            startNode.F = startNode.H;

            m_OpenSet.Add(startNode);
            m_OrderedOpenSet.Push(startNode);

            m_RuntimeGrid.Add(startNode);

            int nodes = 0;


            while (!m_OpenSet.IsEmpty)
            {
                PathNode x = m_OrderedOpenSet.Pop();

                if (x == endNode)
                {
                   // watch.Stop();

                    //elapsed.Add(watch.ElapsedMilliseconds);

                    LinkedList<TPathNode> result = ReconstructPath(m_CameFrom, m_CameFrom[endNode.X, endNode.Y]);
                    result.AddLast(endNode.UserContext);
                    return result;
                }

                m_OpenSet.Remove(x);
                m_ClosedSet.Add(x);

                StoreNeighborNodes(x, neighborNodes);

                for (int i = 0; i < neighborNodes.Length; i++)
                {
                    PathNode y = neighborNodes[i];
                    bool tentative_is_better;

                    if (y == null)
                        continue;

                    if (!y.UserContext.IsWalkable(inUserContext))
                        continue;

                    if (m_ClosedSet.Contains(y))
                        continue;

                    nodes++;

                    float tentative_g_score = m_RuntimeGrid[x].G + NeighborDistance(x, y);
                    bool wasAdded = false;

                    if (!m_OpenSet.Contains(y))
                    {
                        m_OpenSet.Add(y);
                        tentative_is_better = true;
                        wasAdded = true;
                    }
                    else if (tentative_g_score < m_RuntimeGrid[y].G)
                    {
                        tentative_is_better = true;
                    }
                    else
                    {
                        tentative_is_better = false;
                    }

                    if (tentative_is_better)
                    {
                        m_CameFrom[y.X, y.Y] = x;

                        if (!m_RuntimeGrid.Contains(y))
                            m_RuntimeGrid.Add(y);

                        m_RuntimeGrid[y].G = tentative_g_score;
						m_RuntimeGrid[y].H = y.UserContext.Heuristic(endNode, inUserContext); //Heuristic(y, endNode, inUserContext);
                        m_RuntimeGrid[y].F = m_RuntimeGrid[y].G + m_RuntimeGrid[y].H;

                        if (wasAdded)
                            m_OrderedOpenSet.Push(y);
                        else
                            m_OrderedOpenSet.Update(y);
                    }
                }
            }

            return null;
        }

        private LinkedList<TPathNode> ReconstructPath(PathNode[,] came_from, PathNode current_node)
        {
            LinkedList<TPathNode> result = new LinkedList<TPathNode>();

            ReconstructPathRecursive(came_from, current_node, result);

            return result;
        }

        private void ReconstructPathRecursive(PathNode[,] came_from, PathNode current_node, LinkedList<TPathNode> result)
        {
            PathNode item = came_from[current_node.X, current_node.Y];

            if (item != null)
            {
                ReconstructPathRecursive(came_from, item, result);

                result.AddLast(current_node.UserContext);
            }
            else
                result.AddLast(current_node.UserContext);
        }

        private void StoreNeighborNodes(PathNode inAround, PathNode[] inNeighbors)
        {
            int x = inAround.X;
            int y = inAround.Y;

			if ((x > 0) && (y > 0) && !m_fourWay)
                inNeighbors[0] = m_SearchSpace[x - 1, y - 1];
            else
                inNeighbors[0] = null;

            if (y > 0)
                inNeighbors[1] = m_SearchSpace[x, y - 1];
            else
                inNeighbors[1] = null;

			if ((x < Width - 1) && (y > 0) && !m_fourWay)
                inNeighbors[2] = m_SearchSpace[x + 1, y - 1];
            else
                inNeighbors[2] = null;

            if (x > 0)
                inNeighbors[3] = m_SearchSpace[x - 1, y];
            else
                inNeighbors[3] = null;

            if (x < Width - 1)
                inNeighbors[4] = m_SearchSpace[x + 1, y];
            else
                inNeighbors[4] = null;

			if ((x > 0) && (y < Height - 1) && !m_fourWay)
                inNeighbors[5] = m_SearchSpace[x - 1, y + 1];
            else
                inNeighbors[5] = null;

            if (y < Height - 1)
                inNeighbors[6] = m_SearchSpace[x, y + 1];
            else
                inNeighbors[6] = null;

			if ((x < Width - 1) && (y < Height - 1) && !m_fourWay)
                inNeighbors[7] = m_SearchSpace[x + 1, y + 1];
            else
                inNeighbors[7] = null;
        }

        private class OpenCloseMap
        {
            private PathNode[,] m_Map;
            public int Width { get; private set; }
            public int Height { get; private set; }
            public int Count { get; private set; }

			public PathNode this[int x, int y]
			{
				get
                {
                    return m_Map[x, y];
                }
            }

            public PathNode this[PathNode node]
            {
                get
                {
                    return m_Map[node.X, node.Y];
                }

            }

            public bool IsEmpty
            {
                get
                {
                    return Count == 0;
                }
            }

            public OpenCloseMap(int inWidth, int inHeight)
            {
                m_Map = new PathNode[inWidth, inHeight];
                Width = inWidth;
                Height = inHeight;
            }

            public void Add(PathNode inValue)
            {
#if DEBUG
				PathNode item = m_Map[inValue.X, inValue.Y];
                if (item != null)
                    throw new System.ApplicationException();
#endif

                Count++;
                m_Map[inValue.X, inValue.Y] = inValue;
            }

            public bool Contains(PathNode inValue)
            {
                PathNode item = m_Map[inValue.X, inValue.Y];

                if (item == null)
                    return false;

#if DEBUG
                if (!inValue.Equals(item))
					throw new System.ApplicationException();
#endif

                return true;
            }

            public void Remove(PathNode inValue)
            {
#if DEBUG
				PathNode item = m_Map[inValue.X, inValue.Y];
				if (!inValue.Equals(item))
                    throw new System.ApplicationException();
#endif

                Count--;
                m_Map[inValue.X, inValue.Y] = null;
            }

            public void Clear()
            {
                Count = 0;

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        m_Map[x, y] = null;
                    }
                }
            }
        }
    }
}
