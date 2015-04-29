using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Interact with the global Pathfinder */

public class PathSeeker : MonoBehaviour {

	public PathFinder _pathFinder = null;
	public WeightedPathFinder _wPathFinder = null;
	public int Steps = 5;

	public float GridScale = 1f;
	public Vector3 GridPosition = Vector3.zero;

	public delegate void OnPathDelegate(LinkedList<Pathfinding.IPathNode<Object>> path);
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
	
	public void SetGrid(bool[,] grid) {
		int w = grid.GetLength(0);
		int h = grid.GetLength(1);
		for (int x=0; x<w; ++x) {
			for (int y=0; y<h; ++y) {
				if (_wPathFinder != null)
					_wPathFinder.SetPathNode(x,y,grid[x,y]);
				else
					_pathFinder.SetPathNode(x,y,grid[x,y]);
			}
		}
		if (_wPathFinder != null)
			_wPathFinder.UpdateGrid();
		else
			_pathFinder.UpdateGrid();
	}
}
