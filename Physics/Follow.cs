using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform Target = null;
	private Rigidbody2D _body2D = null;
	private Rigidbody _body = null;

	public float Factor = 15f;

	void Start() {
		if (_body2D == null) 
			_body2D = GetComponent<Rigidbody2D>();
		if (_body == null) 
			_body = GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
		if (Target != null) {
			Vector3 distance = Target.position - transform.position;
			if (_body2D != null) {
				_body2D.velocity = distance.Vector2XY() * _body2D.drag * Factor;
			}
			if (_body != null) {
				_body.velocity = distance * _body.drag * Factor;
			}
		}
	}
}
