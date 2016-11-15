/*
The MIT License

Copyright (c) 2016 Martin Pichlmair
Based on http://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8 by Scott Sewell

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

/*
	Stores the transforms for an object after the two most recent fixed steps, and interpolates the object between them using the global interpolation factor. It also ensures that the object is placed back where it was last fixed step before the current fixed step executes, instead of where it was interpolated to last. This means that any scripts moving the transform are working from the correct state. If you teleport an object and want to prevent interpolation, call the ForgetPreviousTransforms method after moving the object. This script should be attached to any objects moved during a FixedUpdate.
*/

namespace Unitilities.FrameInterpolater {

	[RequireComponent(typeof(InterpolatedTransformUpdater))]
	public class InterpolatedTransform : MonoBehaviour {
		private TransformData[] m_lastTransforms;
		private int m_newTransformIndex;

		void OnEnable() {
			ForgetPreviousTransforms();
		}

		public void ForgetPreviousTransforms() {
			m_lastTransforms = new TransformData[2];
			TransformData t = new TransformData(
				                  transform.localPosition,
				                  transform.localRotation,
				                  transform.localScale);
			m_lastTransforms[0] = t;
			m_lastTransforms[1] = t;
			m_newTransformIndex = 0;
		}

		void FixedUpdate() {
			TransformData newestTransform = m_lastTransforms[m_newTransformIndex];
			transform.localPosition = newestTransform.position;
			transform.localRotation = newestTransform.rotation;
			transform.localScale = newestTransform.scale;
		}

		public void LateFixedUpdate() {
			m_newTransformIndex = OldTransformIndex();
			m_lastTransforms[m_newTransformIndex] = new TransformData(
				transform.localPosition,
				transform.localRotation,
				transform.localScale);
		}

		void Update() {
			TransformData newestTransform = m_lastTransforms[m_newTransformIndex];
			TransformData olderTransform = m_lastTransforms[OldTransformIndex()];

			transform.localPosition = Vector3.Lerp(
				olderTransform.position, 
				newestTransform.position, 
				InterpolationController.InterpolationFactor);
			transform.localRotation = Quaternion.Slerp(
				olderTransform.rotation, 
				newestTransform.rotation, 
				InterpolationController.InterpolationFactor);
			transform.localScale = Vector3.Lerp(
				olderTransform.scale, 
				newestTransform.scale,
				InterpolationController.InterpolationFactor);
		}

		private int OldTransformIndex() {
			return (m_newTransformIndex == 0 ? 1 : 0);
		}

		private struct TransformData {
			public Vector3 position;
			public Quaternion rotation;
			public Vector3 scale;

			public TransformData ( Vector3 position, Quaternion rotation, Vector3 scale ) {
				this.position = position;
				this.rotation = rotation;
				this.scale = scale;
			}
		}
	}
}