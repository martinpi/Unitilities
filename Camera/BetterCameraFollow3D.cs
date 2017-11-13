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

using System;
using UnityEngine;

namespace Unitilities
{
	public class BetterCameraFollow3D : MonoBehaviour
	{
		public float LookAheadFactor { get { return lookAheadFactor; } set { lookAheadFactor = value; } }
		public float Damping { get { return damping; } set { damping = value; } }

		public Transform target;
		public float damping = 1;
		public float lookAheadFactor = 3;
		public float lookAheadReturnSpeed = 0.5f;
		public float lookAheadMoveThreshold = 0.1f;
		public bool fixedX = false;
		public bool fixedY = false;

		// map min .. max to (current y) .. (current y + speedZoomMax)
		public float speedZoomMax = 0f;
		public float speedMin = 0f;
		public float speedMax = 1f;

		public bool unidirectionalX = false;
		public bool unidirectionalY = false;

		private Vector3 m_Offset;
		private Vector3 m_LastTargetPosition;
		private Vector3 m_CurrentVelocity;
		private Vector3 m_LookAheadPos;

		void Start() {
			m_LastTargetPosition = target.position;
			m_Offset = (transform.position - target.position);
			transform.parent = null;
		}

		public void JumpToTarget() {
			m_LookAheadPos = Vector3.zero;
			transform.position = m_LastTargetPosition = target.position;
		}

		void Update() {
			Vector3 moveDirection = (target.position - m_LastTargetPosition);
			bool updateLookAheadTarget = Mathf.Abs(moveDirection.magnitude) > lookAheadMoveThreshold;

			if (updateLookAheadTarget)
				m_LookAheadPos = lookAheadFactor*moveDirection;
			else
				m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);

			float speedZoom = Mathf.Clamp01(moveDirection.magnitude - speedMin)/(speedMax-speedMin) * speedZoomMax;

			Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.up * m_Offset.y + Vector3.up * speedZoom;

			Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping );

			if (fixedX) newPos.x = m_Offset.x;
			if (fixedY) newPos.y = m_Offset.y;

			if (unidirectionalX && newPos.x < transform.position.x) newPos.x = transform.position.x;
			if (unidirectionalY && newPos.y < transform.position.y) newPos.y = transform.position.y;

			transform.position = newPos;

			m_LastTargetPosition = target.position;
		}
	}
}
