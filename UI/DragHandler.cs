/*
The MIT License

Copyright (c) 2015 Martin Pichlmair

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

Based on this tutorial: https://www.youtube.com/watch?v=c47QYgsJrWc&app=desktop
*/

using UnityEngine;
using UnityEngine.EventSystems;

namespace Unitilities.UI {
	public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

		public static GameObject DraggedObject = null;

		private Vector3 _offset;
		private Vector3 _originalPosition;
		private Transform _originalParent = null;

		public void OnBeginDrag(PointerEventData eventData) {
			DraggedObject = gameObject;
			Vector2 ppos = eventData.position;
			_offset = new Vector3(ppos.x - transform.position.x,
								  ppos.y - transform.position.y,
								  0);
			_originalPosition = transform.position;
			_originalParent = transform.parent;

			transform.SetParent(transform.root);

			GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		public void OnDrag(PointerEventData eventData) {
			transform.position = Input.mousePosition - _offset;
		}
		public void OnEndDrag(PointerEventData eventData) {
			DraggedObject = null;

			if (transform.parent == transform.root) {
				transform.SetParent(_originalParent);
				transform.position = _originalPosition;
			}
			GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}
}