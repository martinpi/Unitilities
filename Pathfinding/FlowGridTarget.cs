using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowGridTarget : MonoBehaviour {

	public FlowGrid grid;

	void Start () {
		UpdateTarget();
	}

	public void UpdateTarget() {
		if (grid != null) {
			Vector2Int target = Vector2Int.zero;
			Vector3Int t = grid.getGridPosition(transform.position);
			target.x = t.x;
			target.y = t.y;
			grid.Target = target;
			grid.Recalculate();
		}
	}

}
