using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unitilities.TimeDynamics {

	public class Playtime {

		private float _defaultScale = 1f;
		private float _maxUp = 1f;
		private float _maxDown = 1f;

		public float defaultScale { get { return _defaultScale; } }

		public Playtime(float defaultScale) {
			Reset(defaultScale);
		}

		public void Reset(float defaultScale = 1f) { 
			_maxUp = _maxDown = _defaultScale = defaultScale;
		}

		public void SetTimescaleModifier(float value) {
			if (value > _defaultScale) {
				_maxUp = Mathf.Max(value, _maxUp);
			} else {
				_maxDown = Mathf.Clamp(value, 0, _maxDown); // automatically takes the minimum
			}
		}

		public float GetTimescaleModifier() {
			float timescale = _defaultScale;
			if (_maxUp > _defaultScale && _maxDown < _defaultScale) {
				timescale = (_maxUp + _maxDown) / 2f;
			} else {
				if (_maxUp > _defaultScale) {
					timescale = _maxUp;
				}
				if (_maxDown < _defaultScale) {
					timescale = _maxDown;
				}
			}
			_maxUp = _maxDown = _defaultScale;
			return timescale;
		}
	}
}