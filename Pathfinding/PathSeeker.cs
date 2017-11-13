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
using System.Collections;
using System.Collections.Generic;

/* Interact with the global Pathfinder */
namespace Unitilities.Pathfinding {

	public class PathSeeker : MonoBehaviour {

		public PathFinder _pathFinder = null;
		public WeightedPathFinder _wPathFinder = null;
		public int Steps = 5;

		public float GridScale = 1f;
		public Vector3 GridPosition = Vector3.zero;

		public delegate void OnPathDelegate ( LinkedList<Pathfinding.IPathNode<Object>> path );

		public OnPathDelegate pathCallback;


		void Reset() {
			if (_pathFinder == null)
				_pathFinder = GetComponent<PathFinder>();		
			if (_wPathFinder == null)
				_wPathFinder = GetComponent<WeightedPathFinder>();
		}

		//	public LinkedList<Pathfinding.IPathNode<Object>> Path(Vector3 worldPos, Vector3 targetWorldPos) {
		//		Vector3 gp = worldPos * GridScale - GridPosition;
		//		Vector3 tgp = targetWorldPos * GridScale - GridPosition;
		//		Vector2i gridPos = new Vector2i(gp.x, gp.y);
		//		Vector2i targetGridPos = new Vector2i(tgp.x, tgp.y);
		//		return Path(gridPos, targetGridPos);
		//	}
	
		//	public LinkedList<Pathfinding.IPathNode<Object>> Path(Vector2i gridPos, Vector2i targetGridPos) {
		//		LinkedList<Pathfinding.IPathNode<Object>> path;
		//
		//		if (_wPathFinder != null)
		//			path = _wPathFinder.FindPath(gridPos, targetGridPos);
		//		else
		//			path = _pathFinder.FindPath(gridPos, targetGridPos);
		//		pathCallback(path);
		//
		//		return path;
		//	}
	
		public void SetGrid( bool[,] grid ) {
			int w = grid.GetLength(0);
			int h = grid.GetLength(1);
			for (int x = 0; x < w; ++x) {
				for (int y = 0; y < h; ++y) {
					if (_wPathFinder != null)
						_wPathFinder.SetPathNode(x, y, grid[x, y]);
					else
						_pathFinder.SetPathNode(x, y, grid[x, y]);
				}
			}
			if (_wPathFinder != null)
				_wPathFinder.UpdateGrid();
			else
				_pathFinder.UpdateGrid();
		}
	}
}