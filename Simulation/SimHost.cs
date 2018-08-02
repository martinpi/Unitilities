using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unitilities.Simulation;

public class SimHost : MonoBehaviour {

	public Sim sim;

	public float Frequency = 1f;

	private float dt = 0f;
	
	void Start () {
		sim = new Sim(0);
	}
	
	void Update () {
		dt += Time.unscaledDeltaTime;
		if (dt > 1f/Frequency) {
			dt -= 1f/Frequency;

			sim.Tick();
		}
	}
}
