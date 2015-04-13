using UnityEngine;
using System.Collections;

[RequireComponent (typeof (RectTransform))]
[ExecuteInEditMode]
public class ImageAsLine : MonoBehaviour {

	[SerializeField]
	public GameObject Source = null;
	[SerializeField]
	public GameObject Destination = null;
	
	public float Width = 1f;

	void Update() {
		if (Source==null || Destination==null) return;

		RectTransform rectTransform = GetComponent<RectTransform>();
		Vector3 differenceVector = Destination.transform.position-Source.transform.position;
		rectTransform.sizeDelta = new Vector2( differenceVector.magnitude, Width);
		rectTransform.pivot = new Vector2(0, 0.5f);
		rectTransform.position = Source.transform.position;
		float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
		rectTransform.rotation = Quaternion.Euler(0,0, angle);
	}
}
