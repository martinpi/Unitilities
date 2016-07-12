using System;
using UnityEngine;

namespace Unitilities
{
	public class BetterCameraFollow2D : MonoBehaviour
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

		private Vector3 m_Offset;
		private Vector3 m_LastTargetPosition;
		private Vector3 m_CurrentVelocity;
		private Vector3 m_LookAheadPos;

		void Start() {
			m_LastTargetPosition = target.position;
			m_Offset = (transform.position - target.position);
			transform.parent = null;
		}

		void Update() {
			Vector3 moveDirection = (target.position - m_LastTargetPosition);
			bool updateLookAheadTarget = Mathf.Abs(moveDirection.magnitude) > lookAheadMoveThreshold;

			if (updateLookAheadTarget)
				m_LookAheadPos = lookAheadFactor*moveDirection;
			else
				m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);

			Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_Offset.z;
			Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping );

			if (fixedX) newPos.x = m_Offset.x;
			if (fixedY) newPos.y = m_Offset.y;

			transform.position = newPos;

			m_LastTargetPosition = target.position;
		}
	}
}
