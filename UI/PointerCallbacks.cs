using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PointerCallbacks : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler {

	[Serializable]
	public class OnPointerEvent : UnityEvent<PointerCallbacks,PointerEventData> { }
	public OnPointerEvent onPointerEnter = new OnPointerEvent();
	public OnPointerEvent onPointerExit = new OnPointerEvent();
	public OnPointerEvent onPointerUp = new OnPointerEvent();
	public OnPointerEvent onPointerDown = new OnPointerEvent();

    public void OnPointerEnter(PointerEventData eventData) {
		onPointerEnter.Invoke(this, eventData);
    }
    public void OnPointerExit(PointerEventData eventData) {
		onPointerExit.Invoke(this, eventData);
    }
    public void OnPointerUp(PointerEventData eventData) {
		onPointerUp.Invoke(this, eventData);
    }
    public void OnPointerDown(PointerEventData eventData) {
		onPointerDown.Invoke(this, eventData);
    }
}
