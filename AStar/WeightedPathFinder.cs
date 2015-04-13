using UnityEngine;
using System.Collections.Generic;

using Pathfinding;

public class WeightedPathFinder : MonoBehaviour {

	public int Width = 10;
	public int Height = 10;

	private WPathNode[,] _grid;
	private AStar<WPathNode, float[,]> _aStar;
	
	public void CreateGrid (int width, int height) {
		_grid = new WPathNode[width, height];
	}
	
	public void SetPathNode(int x, int y, bool walkable) {
		_grid[x, y] = new WPathNode()
		{
			Walkable = walkable,
			X = x,
			Y = y,
		};
	}
	public void AddPathNode(WPathNode node) {
		_grid[node.X, node.Y] = node;
	}
	
	public void UpdateGrid() {
		_aStar = new AStar<WPathNode, float[,]>(_grid);
	}
	
	public LinkedList<WPathNode> FindPath (Vector2 from, Vector2 to, float[,] grid) {
		return _aStar.Search(from, to, grid);
	}
	
	public bool IsReachable(Vector2 from, Vector2 to, int steps, float[,] grid) {
		return (_aStar.Search(from, to, grid).Count <= steps);
	}

	void Start() {
		int w = Width, h = Height;
		CreateGrid(w,h);
		float[,] test = new float[w,h];
		for (int x=0; x<w; ++x) {
			for (int y=0; y<h; ++y) {
				SetPathNode(x,y,true);
				test[x,y] = 0.1f;
			}
		}
		UpdateGrid();
	}
}
