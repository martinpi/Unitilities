using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PercentButton : MonoBehaviour {

	private Text _label;
	private Image _image;

	public bool ActivateOnFull = false;

	private float _percent = 0f;
	public float Percent { get { return _percent; } 
						   set { SetPercent(value); } }

	void Start () {
		_label = transform.FindChild("Percent").GetComponent<Text>();
		_image = gameObject.GetComponent<Image>();

		CheckInteractive();

		gameObject.GetComponent<Button>().interactable = true;
	}

	void CheckInteractive() {
		if (ActivateOnFull) 
		{
			if (_percent >= 0.999f && !gameObject.GetComponent<Button>().interactable)
				gameObject.GetComponent<Button>().interactable = true;
			else if (_percent < 1f && gameObject.GetComponent<Button>().interactable)
				gameObject.GetComponent<Button>().interactable = false;
		}
	}

	private void SetPercent(float val) {
		_percent = Mathf.Clamp01(val);
		_image.fillAmount = Mathf.Max(0.0001f, _percent);
		_label.text = Mathf.Floor(_percent*100f)+"%";

		CheckInteractive();
	}

	void Update() {
//		Percent = Mathf.Clamp01(Time.time/5f);
	}
}
