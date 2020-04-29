using UnityEngine;

// from: https://wiki.unity3d.com/index.php/Singleton

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>

/* this singleton is not persistent */

namespace Unitilities.Utils {

	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
		// Check to see if we're about to be destroyed.
		private static bool m_ShuttingDown = false;
		private static object m_Lock = new object();
		private static T m_Instance;

		private void Awake() {
			if (m_Instance == null) {
				m_Instance = (T)FindObjectOfType(typeof(T));
			// 	DontDestroyOnLoad(m_Instance.gameObject);
			// } else {
			// 	Destroy(m_Instance);
			}
		}

		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		public static T instance {
			get {
				if (m_ShuttingDown) {
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
						"' already destroyed. Returning null.");
					return null;
				}

				lock (m_Lock) {
					if (m_Instance == null) {
						// Search for existing instance.
						m_Instance = (T)FindObjectOfType(typeof(T));

						// Create new instance if one doesn't already exist.
						if (m_Instance == null) {
							// Need to create a new GameObject to attach the singleton to.
							var singletonObject = new GameObject();
							m_Instance = singletonObject.AddComponent<T>();
							singletonObject.name = typeof(T).ToString() + " (Singleton)";

							// DontDestroyOnLoad(singletonObject);
						}
					}

					return m_Instance;
				}
			}
		}

		private void OnApplicationQuit() {
			m_ShuttingDown = true;
		}

	}

}