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
using System.Collections;

namespace Unitilities.UI {

	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class ImageAsLine : MonoBehaviour {

		[SerializeField]
		public GameObject Source = null;
		[SerializeField]
		public GameObject Destination = null;

		public float Width = 1f;

		void Update() {
			if (Source == null || Destination == null) return;

			RectTransform rectTransform = GetComponent<RectTransform>();
			Vector3 differenceVector = Destination.transform.position - Source.transform.position;
			rectTransform.sizeDelta = new Vector2(differenceVector.magnitude, Width);
			rectTransform.pivot = new Vector2(0, 0.5f);
			rectTransform.position = Source.transform.position;
			float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
			rectTransform.rotation = Quaternion.Euler(0, 0, angle);
		}
	}
}