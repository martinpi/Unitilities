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
using Unitilities.Collections.Generic;

namespace Unitilities.Pathfinding {
	public class TilemapPathNode : IPathNode<Tilemap> {
		public int X { get; set; }
		public int Y { get; set; }
		// public bool IsWalkable(Tilemap userContext) {
		// 	return userContext.HasTile(new Vector3Int(X, Y, 0));
		// }
		public bool Walkable {get; set;}
		public bool IsWalkable(Tilemap userContext) {
			return Walkable;
		}
		public Vector3Int position { get { return new Vector3Int(X, Y, 0); } }

		public float Heuristic(IPathNode<Tilemap> toNode, Tilemap userContext) {
			return (Mathf.Abs(X - toNode.X) + Mathf.Abs(Y - toNode.Y));
		}
	}
}