using UnityEngine;
using System.Collections;

namespace Pathfinding
{
	public class WPathNode : IPathNode<float[,]> {
		public int X { get; set; }
		public int Y { get; set; }
		public bool Walkable {get; set;}
		public bool IsWalkable(float[,] userContext)
		{
			return userContext[X,Y]<1f;
		}

		public float Heuristic(IPathNode<float[,]> toNode, float[,] userContext) {
			return (Mathf.Abs(X - toNode.X) + Mathf.Abs(Y - toNode.Y))*userContext[X,Y];
		}
	}
}
