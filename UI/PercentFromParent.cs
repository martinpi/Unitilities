using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PercentFromParent : MonoBehaviour {

	private Text _label;
	private Image _image;
	private float _fillAmount = 0f;

	void Start () {
		_label = gameObject.GetComponent<Text>();
		_image = transform.parent.gameObject.GetComponent<Image>();
	}
	
	void Update () {
		float fillAmount = _image.fillAmount;
		if (Mathf.Abs(_fillAmount - fillAmount) > Mathf.Epsilon) {
			_label.text = _image.fillAmount*100f + "%";
			_fillAmount = fillAmount;
		}

	}
}
