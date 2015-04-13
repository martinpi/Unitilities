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
