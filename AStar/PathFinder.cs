using UnityEngine;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {

	private Pathfinding.EPathNode[,] _grid;
	private Pathfinding.AStar<Pathfinding.EPathNode, Object> _aStar;

	public void CreateGrid (int width, int height) {
		_grid = new Pathfinding.EPathNode[width, height];
	}

	public void SetPathNode(int x, int y, bool walkable) {
		_grid[x, y] = new Pathfinding.EPathNode()
		{
			Walkable = walkable,
			X = x,
			Y = y,
		};
	}
	public void AddPathNode(Pathfinding.EPathNode node) {
		_grid[node.X, node.Y] = node;
	}

	public void UpdateGrid() {
		_aStar = new Pathfinding.AStar<Pathfinding.EPathNode, Object>(_grid);
	}

	public LinkedList<Pathfinding.EPathNode> FindPath (Vector2 from, Vector2 to) {
		return _aStar.Search(from, to, null);
	}

	public bool IsReachable(Vector2 from, Vector2 to, int steps) {
		return (_aStar.Search(from, to, null).Count <= steps);
	}
}
