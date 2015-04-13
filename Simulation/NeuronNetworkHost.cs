using System;
using UnityEngine;
using System.Collections.Generic;

using Simulation;

public class NeuronNetworkHost : MonoBehaviour {

	public bool Realtime = true;
	public int WarmUpIterations = 10;

	private NeuronNetwork _ai;
	public NeuronNetwork Network { get { return _ai; } }

	private bool _debug = false;

	public void Calculate(float scale = 1f) {
		_ai.Calculate(scale);
	}

	void Start () {
		_ai = new NeuronNetwork();

		NeuronLoader nl = gameObject.GetComponent<NeuronLoader>();
		if (nl != null)
			nl.Init();

		for (int i = 0; i<WarmUpIterations; i++) {
			_ai.Calculate(1f);
			if (_debug) DebugPrint(i);
		}
	}
	
	void Update () {
		if (Realtime) 
			Calculate(Time.deltaTime);
	}

	public void DebugPrint(int step) {
		string str = "Simulation step "+step+":";

		foreach (KeyValuePair<string, Neuron> n in _ai.Neurons) {
			str += "\n "+n.Key+" = "+n.Value.Value;
		}
		Debug.Log(str);
	}
}
