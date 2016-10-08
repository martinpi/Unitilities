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
*/

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;

[AddComponentMenu("UI/DragAble", 55)]
[RequireComponent (typeof (RectTransform))]
public class DragAble : Selectable, IBeginDragHandler, IDragHandler, IEndDragHandler {

	private RectTransform HandleRect = null;
	private RectTransform ParentRect = null;

	private float MinValueX = 0f;
	private float MinValueY = 0f;
	private float MaxValueX = 1f;
	private float MaxValueY = 1f;

	private Vector2 _offset = Vector2.zero;
	private Vector3[] _worldCorners;

	private Vector2 _value;
	public Vector2 Value { get { return _value; } set { SetValue(value); } }

	[Serializable]
	public class DragEvent : UnityEvent<Vector2> { }
	public DragEvent onDrag = new DragEvent();
	public DragEvent onBeginDrag = new DragEvent();
	public DragEvent onEndDrag = new DragEvent();

	protected override void Start() {
//		base.Start();
		ParentRect = transform.parent.GetComponent<RectTransform>();
		HandleRect = gameObject.GetComponent<RectTransform>();
		_worldCorners = new Vector3[4];
	}

	public void OnBeginDrag(PointerEventData eventData) {
		Vector2 ppos = eventData.position;
		_offset.x = ppos.x - transform.position.x;
		_offset.y = ppos.y - transform.position.y;
		onBeginDrag.Invoke(_value);
		SetFromEvent(eventData);
	}
	public void OnEndDrag(PointerEventData eventData) {
		onEndDrag.Invoke(_value);
	}
	public void OnDrag(PointerEventData eventData) {
		SetFromEvent(eventData);
	}
	private void SetFromEvent(PointerEventData eventData) {
		Vector2 npos = eventData.position - _offset;
		ParentRect.GetWorldCorners(_worldCorners);

		npos.x = Mathf.Clamp(npos.x, 
			_worldCorners[0].x+HandleRect.rect.size.x, 
			_worldCorners[2].x-HandleRect.rect.size.x);
		npos.y = Mathf.Clamp(npos.y, 
			_worldCorners[0].y+HandleRect.rect.size.y, 
			_worldCorners[1].y-HandleRect.rect.size.y);
		
		transform.position = npos;
		SetValue(LocalToValue(transform.position));
	}

	private Vector2 ValueToLocal(Vector2 value) {
		value.x = Mathf.Clamp(value.x, MinValueX, MaxValueX);
		value.y = Mathf.Clamp(value.y, MinValueY, MaxValueY);

		Vector2 targetPosition;
		Vector2 size = ParentRect.rect.size;
		
		targetPosition.x = size.x * (((value.x - MinValueX)/(MaxValueX - MinValueX)) - 0.5f);
		targetPosition.y = size.y * (((value.y - MinValueY)/(MaxValueY - MinValueY)) - 0.5f);

		return targetPosition;
	}
	private Vector2 LocalToValue(Vector2 local) {
		Vector2 value;
		Vector2 size = ParentRect.rect.size;

		value.x = (local.x/size.x + 0.5f) * (MaxValueX - MinValueX) + MinValueX;
		value.y = (local.y/size.y + 0.5f) * (MaxValueY - MinValueY) + MinValueY;

		value.x = Mathf.Clamp(value.x, MinValueX, MaxValueX);
		value.y = Mathf.Clamp(value.y, MinValueY, MaxValueY);

		return value;
	}

	private void SetValue(Vector2 value) {
		_value = value;
		onDrag.Invoke(_value);
	}

}
