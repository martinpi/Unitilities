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
		_label = transform.Find("Percent").GetComponent<Text>();
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
