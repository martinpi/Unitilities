using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unitilities.Gameplay {

	public class TimedTransformReset : MonoBehaviour {

		public Transform Target;
		public float Timeout = 1f;
		public bool Deactivate = false;
		float _startTime;

		Vector3 _position;
		Vector3 _scale;
		Quaternion _rotation;
		bool _active;

		void OnEnable() {
			_startTime = UnityEngine.Time.time;
			_position = Target.position;
			_scale = Target.localScale;
			_rotation = Target.rotation;
		}

		void Update() {
			if (UnityEngine.Time.time - _startTime > Timeout) {
				Target.position = _position;
				Target.localScale = _scale;
				Target.rotation = _rotation;
				if (Deactivate)
					Target.gameObject.SetActive(false);
			}
		}
	}
}