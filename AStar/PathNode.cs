using UnityEngine;
using System.Collections;

namespace Pathfinding
{
	public class EPathNode : IPathNode<Object> {
		public int X { get; set; }
		public int Y { get; set; }
		public bool Walkable {get; set;}
		public bool IsWalkable(Object userContext)
		{
			return Walkable;
		}

		public float Heuristic(IPathNode<Object> toNode, Object userContext) {
			return (Mathf.Abs(X - toNode.X) + Mathf.Abs(Y - toNode.Y));
		}
	}
}