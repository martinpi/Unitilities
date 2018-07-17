using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowGridFollow : MonoBehaviour {

	public FlowGrid FlowGrid;
	public float Force = 1f;
	public bool RandomStartPosition = true;

	private bool _active = false;
	private Rigidbody2D _body2D;

	void Start () {
		_body2D = GetComponent<Rigidbody2D>();
		StartCoroutine(StartMoving(2f));
		if (RandomStartPosition) {
			Vector2 pos = Vector2.zero;
			pos.x = ((float)FlowGrid.Width) * Random.value;
			pos.y = ((float)FlowGrid.Height) * Random.value;
			// _body2D.MovePosition(pos);
			transform.position = pos.Vector3XY();
		}
	}
	
	void Update () {
		if (!_active) return;

		Vector3 dir = FlowGrid.getInterpolatedForces(transform.position);
		_body2D.AddForce(Force * dir.Vector2XY());
	}

	IEnumerator StartMoving(float delay) {
		yield return new WaitForSeconds(delay);
		_active = true;

	}

}
