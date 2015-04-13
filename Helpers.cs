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

namespace Utils {

	public static class Helpers {
		public static GameObject CreateObjectAt(GameObject theObject, Vector3 position, GameObject parent = null) {
			GameObject newTile = (GameObject)Object.Instantiate(theObject);
			newTile.transform.position = position + newTile.transform.localPosition;
			if (parent != null)
				newTile.transform.parent = parent.transform;
			return newTile;
		}
		public static GameObject CreateObjectAtLocal(GameObject theObject, Vector3 position, GameObject parent = null) {
			GameObject newTile = (GameObject)Object.Instantiate(theObject);
			newTile.transform.localPosition = position;
			if (parent != null)
				newTile.transform.parent = parent.transform;
			return newTile;
		}

		public static GameObject CreateUIObjectAtLocal(GameObject theObject, Vector3 position, GameObject parent = null) {
			GameObject newTile = (GameObject)Object.Instantiate(theObject);
			newTile.transform.position = position;
			if (parent != null)
				newTile.transform.SetParent(parent.transform, false);
			return newTile;
		}

	}

}
