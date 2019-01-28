/*
The MIT License

Copyright (c) 2018 Martin Pichlmair

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
			}
		}
		_grid.Recalculate();

		StartCoroutine(CastRays(frequency));
	}
}
