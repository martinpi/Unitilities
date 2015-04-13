using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Interact with the global Pathfinder */

public class PathSeeker : MonoBehaviour {

	public PathFinder _pathFinder = null;
	public int Steps = 5;

	void Reset() {
		if (_pathFinder == null)
			_pathFinder = GameObject.Find("Game").GetComponent<PathFinder>();		
	}

	public LinkedList<Pathfinding.EPathNode> Path(Vector2i gridPos, Vector2i targetGridPos) {
		return _pathFinder.FindPath(gridPos, targetGridPos);
	}
	
	public void SetGrid(bool[,] grid) {
		int w = grid.GetLength(0);
		int h = grid.GetLength(1);
		for (int x=0; x<w; ++x) {
			for (int y=0; y<h; ++y) {
				_pathFinder.SetPathNode(x,y,grid[x,y]);
			}
		}
		_pathFinder.UpdateGrid();
	}
}
