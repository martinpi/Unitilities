﻿/*
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

namespace Unitilities.UI {

	public class PercentFromParent : MonoBehaviour {

		private Text _label;
		private Image _image;
		private float _fillAmount = 0f;

		void Start() {
			_label = gameObject.GetComponent<Text>();
			_image = transform.parent.gameObject.GetComponent<Image>();
		}

		void Update() {
			float fillAmount = _image.fillAmount;
			if (Mathf.Abs(_fillAmount - fillAmount) > Mathf.Epsilon) {
				_label.text = _image.fillAmount * 100f + "%";
				_fillAmount = fillAmount;
			}

		}
	}
}