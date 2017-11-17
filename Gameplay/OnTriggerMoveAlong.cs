using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unitilities.Gameplay {
	
	public class OnTriggerMoveAlong : MonoBehaviour {

		private Vector2 _offset;
		private Vector3 _lastPos;
		private Rigidbody2D _body = null;
		public float Force = 100f;

		void OnTriggerEnter2D(Collider2D col) {
			Rigidbody2D rb = col.attachedRigidbody;
			if (rb != null && col.tag == "Player") {
				_offset = rb.position - transform.position.Vector2XY();
				_body = rb;
				_body.isKinematic = true;

				Debug.Log("enter trigger");
			}
		}

		void OnTriggerExit2D(Collider2D col) {
			if (_body != null) {
				_body.isKinematic = false;
				_body = null;

				Debug.Log("exit trigger");
			}
		}

		void FixedUpdate() {
			if (_body != null && _body.isKinematic) {
				_body.MovePosition(transform.position.Vector2XY() + _offset);
			}
		}
	}
}

