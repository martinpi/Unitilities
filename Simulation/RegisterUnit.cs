using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unitilities.Simulation;

public class RegisterUnit : MonoBehaviour {

	private SimHost simHost;
	private Unit unit;
	
	void Start () {
		simHost = FindObjectOfType<SimHost>();
		unit = simHost.sim.AddUnit(gameObject);
	}
	
	void Update () {
		transform.position = unit.position;
	}
}
