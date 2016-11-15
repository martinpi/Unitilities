/*
The MIT License

Copyright (c) 2016 Martin Pichlmair

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


using UnityEngine;
using System.Collections;

namespace Unitilities.BoneSystem {

	[RequireComponent(typeof(Bone))]
	public class BoneDragger : MonoBehaviour {

		public float _MaxLength = 15f;
		public float _MinLength = 2f;

		private bool _dragging = false;
		public bool Dragging { 
			get { return _dragging; } 
			set { 
				_dragging = value;
				if (_dragging) {
					_originalLocation = _bone.To;
					if (_snapped != null) {
						_snapped.Events.Unsnap(this);
						if (_snapEvents != null)  _snapEvents.Unsnap(this);
					}
				} else {
					if (_snapped == null)
						_bone.To = _originalLocation;
					else {
						_snapped.Events.Snap(this);
						if (_snapEvents != null)  _snapEvents.Snap(this);
					}
				}
			} 
		}

		public bool _HideBelowMin = true;
		private bool _hidden = false;

		private Bone _bone;

		public float _SnapDistance = 0f;
		private BoneSnapTarget[] _boneSnapTargets;
		private BoneSnapTarget _snapped;
		private BoneSnapEvents _snapEvents;

		private Vector2 _originalLocation;

		void Awake() {
			_bone = GetComponent<Bone>();
			_snapEvents = GetComponent<BoneSnapEvents>();
			_boneSnapTargets = FindObjectsOfType<BoneSnapTarget>();
		}

		void FollowMouse() {

			_bone.To = Vector2.ClampMagnitude(Camera.main.ScreenToWorldPoint(Input.mousePosition).Vector2XY(), _MaxLength);
			_bone.To = _bone.Distance.magnitude < _MinLength ? (_bone.From + _bone.Distance.normalized * _MinLength) : _bone.To;

			if (_boneSnapTargets.Length > 0 && _SnapDistance > 0f) { 
				BoneSnapTarget closest = null;
				float closestDistance = _SnapDistance;
				foreach (BoneSnapTarget b in _boneSnapTargets) {
					if (b.transform == transform) continue; // omit self
					float dist = Vector2.Distance(b.transform.position.Vector2XY(), _bone.To);
					if (dist < closestDistance) {
						closestDistance = dist;
						closest = b;
					}
				}
				if (closest != null) {
					_bone.To = closest.transform.position.Vector2XY();
					closest.Events.CanSnap(this);
				}
				_snapped = closest;
			}

			_bone.To -= transform.position.Vector2XY();
		}

		void Update() {
			if (_dragging) 
				FollowMouse();

			bool oldHidden = _hidden;
			_hidden = _HideBelowMin && _bone.To.magnitude <= _MinLength;
			if (oldHidden != _hidden) GetComponent<Renderer>().enabled = !_hidden; // only execute getcomponent if visibility changes
		}
	}
}