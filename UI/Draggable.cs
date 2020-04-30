/*
The MIT License

Copyright (c) 2015 Martin Pichlmair

2019: Rewrote based on https://forum.unity.com/threads/drag-window-script-with-window-clamped-within-canvas.453340/

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
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Unitilities.UI {

	[AddComponentMenu("UI/Draggable", 55)]
	[RequireComponent(typeof(RectTransform))]
	public class Draggable : Selectable, IBeginDragHandler, IDragHandler, IEndDragHandler {

		private RectTransform _rect = null;
		private RectTransform _parentRect = null;

		private Vector2 _offset = Vector2.zero;

		[Serializable]
		public class DragEvent : UnityEvent<Draggable> { }
		public DragEvent onDrag = new DragEvent();
		public DragEvent onBeginDrag = new DragEvent();
		public DragEvent onEndDrag = new DragEvent();

		protected override void Start() {
			_parentRect = transform.parent.GetComponent<RectTransform>();
			_rect = gameObject.GetComponent<RectTransform>();
		}

		public void OnBeginDrag(PointerEventData eventData) {
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, eventData.position, eventData.pressEventCamera, out _offset);
			onBeginDrag.Invoke(this);
		}
		public void OnEndDrag(PointerEventData eventData) {
			onEndDrag.Invoke(this);
		}
		public void OnDrag(PointerEventData eventData) {
			Vector2 localPointerPosition;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRect, eventData.position, eventData.pressEventCamera, out localPointerPosition)) {

				Vector2 clampedPosition = localPointerPosition - _offset;
				// clampedPosition.x = (ParentRect.rect.width * 0.5f) - (HandleRect.rect.width * (1 - HandleRect.pivot.x));
				// clampedPosition.x = (-ParentRect.rect.width * 0.5f) + (HandleRect.rect.width * HandleRect.pivot.x);
				// clampedPosition.y = (ParentRect.rect.height * 0.5f) - (HandleRect.rect.height * (1 - HandleRect.pivot.y));
				// clampedPosition.y = (-ParentRect.rect.height * 0.5f) + (HandleRect.rect.height * HandleRect.pivot.y);
				
				_rect.localPosition = clampedPosition;
			}

			onDrag.Invoke(this);
		}
	}
}
