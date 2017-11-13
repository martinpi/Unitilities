/*
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
	public class WeightedPathFinder : MonoBehaviour {

		public int Width = 10;
		public int Height = 10;

		private WPathNode[,] _grid;
		private AStar<WPathNode, float[,]> _aStar;

		public void CreateGrid( int width, int height ) {
			_grid = new WPathNode[width, height];
		}

		public void SetPathNode( int x, int y, bool walkable ) {
			_grid[x, y] = new WPathNode() {
				Walkable = walkable,
				X = x,
				Y = y,
			};
		}

		public void AddPathNode( WPathNode node ) {
			_grid[node.X, node.Y] = node;
		}

		public void UpdateGrid() {
			_aStar = new AStar<WPathNode, float[,]>(_grid);
		}

		public LinkedList<WPathNode> FindPath( Vector2 from, Vector2 to, float[,] grid ) {
			return _aStar.Search(from, to, grid);
		}

		public bool IsReachable( Vector2 from, Vector2 to, int steps, float[,] grid ) {
			return (_aStar.Search(from, to, grid).Count <= steps);
		}

		void Test() {
			int w = Width, h = Height;
			CreateGrid(w, h);
			float[,] test = new float[w, h];
			for (int x = 0; x < w; ++x) {
				for (int y = 0; y < h; ++y) {
					SetPathNode(x, y, true);
					test[x, y] = 0.1f;
				}
			}
			UpdateGrid();
		}
	}
}