using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Unitilities.UI {

	[AddComponentMenu("UI/Drag Sensor", 55)]
	[RequireComponent(typeof(RectTransform))]
	public class DragSensor : MonoBehaviour {

		public bool activateOnAwake = false;

		private RectTransform _rect = null;
		private Draggable[] _allDraggables;
		private List<Draggable> _insideDraggables = new List<Draggable>();

		[Serializable]
		public class DragEvent : UnityEvent<Draggable, DragSensor> { }
		public DragEvent onDropped = new DragEvent();
		public DragEvent onOver = new DragEvent();
		public DragEvent onEnter = new DragEvent();
		public DragEvent onExit = new DragEvent();

		private bool IsInside(Vector2 point) {
			return _rect.rect.Contains(point - _rect.anchoredPosition);
		}

		private void OnDrag(Draggable d) {
			if (IsInside(d.GetComponent<RectTransform>().anchoredPosition)) {
				onOver.Invoke(d, this);
				if (!_insideDraggables.Contains(d)) {
					_insideDraggables.Add(d);
					onEnter.Invoke(d, this);
				}
			} else {
				if (_insideDraggables.Contains(d)) {
					_insideDraggables.Remove(d);
					onExit.Invoke(d, this);
				}
			}
		}
		private void OnDrop(Draggable d) {
			if (IsInside(d.GetComponent<RectTransform>().anchoredPosition)) {
				onDropped.Invoke(d, this);
				if (_insideDraggables.Contains(d))
					_insideDraggables.Remove(d);
			}
		}

		public void ActivateSensor() {
			_allDraggables = FindObjectsOfType<Draggable>();
			foreach (Draggable d in _allDraggables) {
				d.onDrag.AddListener(OnDrag);
				d.onEndDrag.AddListener(OnDrop);
			}
		}
		public void DeactivateSensor() {
			foreach (Draggable d in _allDraggables) {
				d.onDrag.RemoveListener(OnDrag);
				d.onEndDrag.RemoveListener(OnDrop);
			}
		}

		private void Awake() {
			_rect = gameObject.GetComponent<RectTransform>();
			if (activateOnAwake)
				ActivateSensor();
		}

	}
}