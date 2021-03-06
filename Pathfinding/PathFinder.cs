﻿/*
The MIT License

Copyright (c) 2015 Martin Pichlmair

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

namespace Unitilities.Pathfinding {

	public class PathFinder : MonoBehaviour {

		private Pathfinding.EPathNode[,] _grid;
		private Pathfinding.AStar<Pathfinding.EPathNode, Object> _aStar;

		public void CreateGrid( int width, int height ) {
			_grid = new Pathfinding.EPathNode[width, height];
		}

		public void SetPathNode( int x, int y, bool walkable ) {
			_grid[x, y] = new Pathfinding.EPathNode() {
				Walkable = walkable,
				X = x,
				Y = y,
			};
		}

		public void AddPathNode( Pathfinding.EPathNode node ) {
			_grid[node.X, node.Y] = node;
		}

		public void UpdateGrid() {
			_aStar = new Pathfinding.AStar<Pathfinding.EPathNode, Object>(_grid);
		}

		public LinkedList<Pathfinding.EPathNode> FindPath( Vector2 from, Vector2 to ) {
			return _aStar.Search(from, to, null);
		}

		public bool IsReachable( Vector2 from, Vector2 to, int steps ) {
			return (_aStar.Search(from, to, null).Count <= steps);
		}
	}
}
