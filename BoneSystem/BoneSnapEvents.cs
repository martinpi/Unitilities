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
using UnityEngine.Events;
using System.Collections;


namespace Unitilities.BoneSystem {

	public class BoneSnapEvents : MonoBehaviour {

		public UnityEvent Snapped;
		public UnityEvent CanSnapped;
		public UnityEvent Unsnapped;

		public delegate void SnapAction(BoneDragger dragger, BoneSnapEvents target);
		public event SnapAction OnSnap;
		public event SnapAction OnCanSnap;
		public event SnapAction OnUnsnap;

		public void CanSnap(BoneDragger dragger) {
			if (OnCanSnap != null) OnCanSnap(dragger, this);
			CanSnapped.Invoke();
		}

		public void Snap(BoneDragger dragger) {
			if (OnSnap != null) OnSnap(dragger, this);
			Snapped.Invoke();
		}

		public void Unsnap(BoneDragger dragger) {
			if (OnUnsnap != null) OnUnsnap(dragger, this);
			Unsnapped.Invoke();
		}
	}

}
