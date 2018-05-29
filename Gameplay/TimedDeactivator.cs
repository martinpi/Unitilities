using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unitilities.Gameplay {

	public class TimedDeactivator : MonoBehaviour {

		public float Delay = 1f;

        private IEnumerator Deactivate(float delay) {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }

		void Start() {
			StartCoroutine(Deactivate(Delay));
		}
	}
}