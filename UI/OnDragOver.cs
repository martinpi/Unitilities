using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

namespace Unitilities.UI {

	[RequireComponent(typeof(RectTransform))]
	public class OnDragOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		//	private RectTransform m_RectTransform;
		//	public DragAble m_TargetDragAble = null;

		void Awake() {
			//		if (m_TargetDragAble != null) {
			//			m_TargetDragAble.onDrag.AddListener(OnDrag);
			//		}
			//		m_RectTransform = transform as RectTransform;
		}

		public virtual void OnPointerEnter(PointerEventData data) {
			if (data.dragging && data.pointerDrag != null) {
				Debug.Log("Dragged inside: " + data.pointerDrag.name);

				//			var currentOverGo = data.pointerCurrentRaycast.gameObject;

			}
		}
		public virtual void OnPointerExit(PointerEventData data) {
			if (data.dragging && data.pointerDrag != null) {
				Debug.Log("Dragged outside: " + data.pointerDrag.name);
			}

		}


		//	protected virtual void OnDrag(Vector2 location) {
		//		Debug.Log(location);
		//
		//		Vector3 localLocation = (m_RectTransform.worldToLocalMatrix * location.Vector3XY());
		//
		//		if (m_RectTransform.rect.Contains(localLocation.Vector2XY()))
		//			Debug.Log("inside");
		//
		//	}

		void Update() {

		}
	}
}