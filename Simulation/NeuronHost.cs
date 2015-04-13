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
using System.Collections;

using Simulation;

public class NeuronHost : MonoBehaviour {

	private Neuron _neuron = null;
	public NeuronNetworkHost Host = null;

	public bool Autocreate = false;
	public bool Additive = true;
	public string ID = null;
	// public string Formula { 
	// 	get { if (_neuron != null) return _neuron.Formula; else return ""; }
	// 	set { if (_neuron != null) _neuron.ParseFormula(value); } 
	// }
	public double Value { get { if (_neuron != null) return _neuron.Value; else return -1.0; } }

	public Neuron Neuron {
		set { _neuron = value; }
		get { return _neuron; }
	}

	void Start () {
		Init();
	}

	public void Init() {
		if (Host != null && ID != null) {
			if (Host.Network.Neurons.ContainsKey(ID))
				_neuron = Host.Network.Neurons[ID];

			if (_neuron == null && Autocreate)
				_neuron = new Neuron(ID, "", Host.Network, Additive);
		}
	}
}
