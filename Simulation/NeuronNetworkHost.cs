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
