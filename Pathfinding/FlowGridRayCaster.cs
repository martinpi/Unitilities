using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlowGrid))]
public class FlowGridRayCaster : MonoBehaviour {

	public float Frequency = 1f;
	public float Altitude = 5f;

	private FlowGrid _grid;

	void Start () {
		_grid = GetComponent<FlowGrid>();
		StartCoroutine(CastRays(Frequency));
	}

	// void OnDrawGizmosSelected() {
    // 	Gizmos.color = Color.white;
	// 	Vector3Int gridPos = Vector3Int.zero;
	// 	// Vector3 halfSize = (_grid.getWorldPosition(new Vector3Int(1,1,0)) - transform.position)/2f;
	// 	for (gridPos.x = 0; gridPos.x < _grid.Width; gridPos.x++) {
	// 		for (gridPos.y = 0; gridPos.y < _grid.Height; gridPos.y++) {

	// 			Gizmos.DrawRay(
	// 				_grid.getWorldPosition(gridPos), Vector3.back
	// 			);
	// 		}
	// 	}
	// }
	
	private IEnumerator CastRays(float frequency) {
		yield return new WaitForSecondsRealtime(frequency);

		RaycastHit hit;
		Vector3Int gridPos = Vector3Int.zero;
		gridPos.z = (int)(transform.position.z + Altitude);

		// Vector3 halfSize = _grid.getWorldPosition(new Vector3Int(1,1,0)) - transform.position;

		for (gridPos.x = 0; gridPos.x < _grid.Width; gridPos.x++) {
			for (gridPos.y = 0; gridPos.y < _grid.Height; gridPos.y++) {
				bool wall = Physics.Raycast(_grid.getWorldPosition(gridPos), Vector3.back, out hit, Altitude+1f, 1<<gameObject.layer);
				_grid.setWall(gridPos, wall);

				// if (wall) Debug.Log("wall at "+gridPos);

			}
		}
		_grid.Recalculate();

		StartCoroutine(CastRays(frequency));
	}
}
