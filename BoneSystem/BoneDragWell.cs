/*
The MIT License

Copyright (c) 2016 Martin Pichlmair

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
using Unitilities;

namespace Unitilities.BoneSystem {

	public class BoneDragWell : MonoBehaviour {

		public GameObject _Prefab;
		private GameObject _object;
		public BoneDragger Dragger { get { return _object.GetComponent<BoneDragger>(); } }

		void Awake() {
			TryCreate();
			_object.SetActive(false);
		}

		void TryCreate() {
			if (_Prefab != null && _object == null) {
				_object = Utils.Helpers.CreateObjectAt(_Prefab, Vector3.zero, gameObject);
			}
		}

		public void CreateDragger() {
			TryCreate();
			if (_object == null) return;

			_object.transform.position = transform.position;
			_object.GetComponent<BoneRectBridge>().Init();
			_object.SetActive(true);
		}
	}

}