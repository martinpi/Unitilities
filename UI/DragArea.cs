using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;

[AddComponentMenu("UI/Drag Area", 51)]
[RequireComponent (typeof (RectTransform))]
public class DragArea : Selectable, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public RectTransform HandleRect = null;

	[SerializeField]
	public float MinValueX = 0f;
	[SerializeField]
	public float MinValueY = 0f;
	[SerializeField]
	public float MaxValueX = 1f;
	[SerializeField]
	public float MaxValueY = 1f;

	private Vector2 _value;
	public Vector2 Value { get { return _value; } set { SetValue(value); } }

	[Serializable]
	public class DragEvent : UnityEvent<Vector2> { }
	public DragEvent onValueChanged = new DragEvent();


	public void OnBeginDrag(PointerEventData eventData) {
		SetFromEvent(eventData);
	}
	public void OnEndDrag(PointerEventData eventData) {
	}
	public void OnDrag(PointerEventData eventData) {
		SetFromEvent(eventData);
	}
	private void SetFromEvent(PointerEventData eventData) {
		Vector2 ppos = eventData.position;
		Vector3 fpos = HandleRect.transform.parent.GetComponent<RectTransform>().position;
		Vector2 wpos = new Vector2(ppos.x - fpos.x, ppos.y - fpos.y);
		
		SetValue(LocalToValue(wpos));
	}

	private Vector2 ValueToLocal(Vector2 value) {
		value.x = Mathf.Clamp(value.x, MinValueX, MaxValueX);
		value.y = Mathf.Clamp(value.y, MinValueY, MaxValueY);

		Vector2 targetPosition;
		Vector2 size = HandleRect.transform.parent.GetComponent<RectTransform>().rect.size;
		
		targetPosition.x = size.x * (((value.x - MinValueX)/(MaxValueX - MinValueX)) - 0.5f);
		targetPosition.y = size.y * (((value.y - MinValueY)/(MaxValueY - MinValueY)) - 0.5f);

		return targetPosition;
	}
	private Vector2 LocalToValue(Vector2 local) {
		Vector2 value;
		Vector2 size = HandleRect.transform.parent.GetComponent<RectTransform>().rect.size;

		value.x = (local.x/size.x + 0.5f) * (MaxValueX - MinValueX) + MinValueX;
		value.y = (local.y/size.y + 0.5f) * (MaxValueY - MinValueY) + MinValueY;

		value.x = Mathf.Clamp(value.x, MinValueX, MaxValueX);
		value.y = Mathf.Clamp(value.y, MinValueY, MaxValueY);

		return value;
	}

	private void SetValue(Vector2 value) {
		_value = value;
		HandleRect.localPosition = ValueToLocal(_value);
		onValueChanged.Invoke(_value);
	}
}
