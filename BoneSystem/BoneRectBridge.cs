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
using UnityEngine.UI;
using System.Collections;

namespace Unitilities.BoneSystem {

	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Bone))]
	public class BoneRectBridge : MonoBehaviour {

		private RectTransform _rectTransform;
		private Bone _bone;

		private Vector2 _worldPivot;
		private Vector2 _originalPivot;

		void Awake() {
			_rectTransform = this.transform as RectTransform;
			_bone = GetComponent<Bone>();
			Init();
		}

		public void Init() {
			_originalPivot = _rectTransform.pivot;
			_worldPivot = TransformVector(_originalPivot);

			_bone.From = _rectTransform.anchoredPosition;
			_bone.To = _bone.From + Vector2.up * _rectTransform.sizeDelta.y - 2f * _worldPivot.y * Vector2.up;
		}
	
		private Vector2 TransformVector(Vector2 vec) {
			vec.x *= _rectTransform.sizeDelta.x;
			vec.y *= _rectTransform.sizeDelta.y;
			return vec;
		}

		void FixedUpdate() {

			// pivot remains unchanged, bone is just distance between two pivots
			Vector2 targetSize = Vector2.zero;
			targetSize.x = _rectTransform.sizeDelta.x;
			targetSize.y = 2f * _worldPivot.y + _bone.Distance.magnitude;
			_rectTransform.anchoredPosition = _bone.From;
			_rectTransform.sizeDelta = targetSize;

			// pivot is relative, so put it back
			Vector2 pivot = _rectTransform.pivot;
			pivot.y = _worldPivot.y / targetSize.y;
			_rectTransform.pivot = pivot;

			// set angle
			Vector2 targetDirection = _bone.Distance;
			_rectTransform.localRotation = Quaternion.AngleAxis(90f - Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.back);
		}
	}
}