/*
The MIT License

Copyright (c) 2019 Martin Pichlmair

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
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace Unitilities.Pathfinding {

	public class TilemapPathFinder : MonoBehaviour {

		public Tilemap walkableMap;
		public Tilemap wallMap;

		private Vector3Int _offset;
		private Pathfinding.TilemapPathNode[,] _grid;
		private Pathfinding.AStar<Pathfinding.TilemapPathNode, Tilemap> _aStar;

		public void CreateGrid( int width, int height ) {
			_grid = new Pathfinding.TilemapPathNode[width, height];
		}

		public void SetPathNode( int x, int y, bool walkable ) {
			_grid[x, y] = new Pathfinding.TilemapPathNode() {
				Walkable = walkable,
				X = x+_offset.x,
				Y = y+_offset.y,
			};
		}

		public void AddPathNode( Pathfinding.TilemapPathNode node ) {
			_grid[node.X, node.Y] = node;
		}

		public void UpdateGrid() {
			_aStar = new Pathfinding.AStar<Pathfinding.TilemapPathNode, Tilemap>(_grid);
		}

		public LinkedList<Pathfinding.TilemapPathNode> FindPath( Vector3Int from, Vector3Int to ) {
			BoundsInt bounds = walkableMap.cellBounds;
			if (!bounds.Contains(from) || !bounds.Contains(to)) return null;
			return _aStar.Search(from.x-_offset.x, from.y-_offset.y, to.x-_offset.x, to.y-_offset.y, walkableMap);
		}

        public void GetWalkableAreas( ) {
			BoundsInt bounds = walkableMap.cellBounds;
            
			for (int x = bounds.min.x; x < bounds.max.x; ++x) {
				for (int y = bounds.min.y; y < bounds.max.y; ++y) {
                    Vector3Int pos = new Vector3Int(x,y,0);
					SetPathNode(x-bounds.min.x, y-bounds.min.y, walkableMap.HasTile(pos) && !wallMap.HasTile(pos));
				}
			}
			UpdateGrid();
		}

		private void OnEnable() {
			_offset = walkableMap.cellBounds.min;
			CreateGrid(walkableMap.cellBounds.size.x, walkableMap.cellBounds.size.y);
			GetWalkableAreas();
		}

	}
}
