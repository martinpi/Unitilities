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
