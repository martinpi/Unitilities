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

using System.Collections.Generic;
using Utils;

namespace Unitilities.Simulation {

	public class NeuronNetwork {
		private Dictionary<string, Neuron> _neurons = new Dictionary<string,Neuron>();
		private List<TemporaryNeuronAffector> _affectors = new List<TemporaryNeuronAffector>();
		private double _seed = 0.0;

		public Dictionary<string, Neuron> Neurons { get { return _neurons; } }
		public double Seed { get { return _seed; }}

		public NeuronNetwork() { }
		public NeuronNetwork(double seed) { _seed = seed; }

		public void CreateNeuron(string id, bool additive = true) {
			Neuron n = new Neuron(id, additive);
			n.Host = this;
			_neurons.Add(n.ID, n);
		}
		public void CreateTemporaryAffector(string neuron, float duration, float scale) {
			UnityEngine.Debug.Log ("CreateTemporaryAffector '"+neuron+"'");

			_affectors.Add(new TemporaryNeuronAffector(duration, _neurons[neuron], scale));
		}

		public void Print() {
			foreach (Neuron n in _neurons.Values) 
				UnityEngine.Debug.Log("Neuron "+n.ID+" = "+n.Value);
		}

		public void Calculate(float scale) {

			foreach (Neuron n in _neurons.Values) 
				n.Prepare();

			List<TemporaryNeuronAffector> doneAffectors = new List<TemporaryNeuronAffector>();
			foreach (TemporaryNeuronAffector t in _affectors) {
				if (t.IsDone()) doneAffectors.Add(t);
				else t.Calculate(scale);
			}

			foreach (Neuron n in _neurons.Values) 
				foreach (NeuronInput i in n.Inputs) 
					i.Calculate(scale);

			foreach (Neuron n in _neurons.Values) 
				n.Calculate();

			foreach (Neuron n in _neurons.Values) 
				n.Conclude();

			foreach (TemporaryNeuronAffector t in doneAffectors)
				_affectors.Remove(t);
		}

		/*
		// for testing:
		public static NeuronNetwork Network = new NeuronNetwork();
		public static void Test() {
			Neuron n1 = new Neuron("n1");
			Neuron n2 = new Neuron("n2");
			Neuron n3 = new Neuron("n3");

			n3.Inputs.Add(new NeuronInput(n3, "n2", "0.12 + A * 0.03"));
			n2.Inputs.Add(new NeuronInput(n2, "n1", "0.14 - A * 0.22"));
			n1.Inputs.Add(new NeuronInput(n1, "n3", "0.66 * A ^ 0.2"));

			for (int i = 0; i<5; i++) {
				UnityEngine.Debug.Log("Simulation step "+i+":\n n1 = "+n1.Value+"\n n2 = "+n2.Value+"\n n3 = "+n3.Value);
				Network.Calculate();
			}
		}
		*/
	}

	public class NeuronInput {
		private string _formula;
		private string _sourceNeuronId;
		private MathParser _parser;
		private double _value;
		private Neuron _neuron;
		private double _fixedValue;
		public NeuronInput(Neuron neuron, string sourceNeuronId, string formula) { 
			_neuron = neuron;
			_formula = formula; 
			_sourceNeuronId = sourceNeuronId;
			_parser = new MathParser();
			_fixedValue = 0.0;

			if (_sourceNeuronId != null && !_neuron.Host.Neurons.ContainsKey(_sourceNeuronId))
				UnityEngine.Debug.Log("Key "+_sourceNeuronId+" not found for host "+_neuron.ID);
		}
		public void Calculate(float scale) { 
			if (_sourceNeuronId != null && _neuron.Host.Neurons.ContainsKey(_sourceNeuronId))
				_parser.Parameters[MathParser.Variables.A] = _neuron.Host.Neurons[_sourceNeuronId].Value;

			_parser.Parameters[MathParser.Variables.F] = _fixedValue;
			_parser.Parameters[MathParser.Variables.S] = _neuron.Host.Seed;
			_parser.Parameters[MathParser.Variables.R] = (double)UnityEngine.Random.value;
			_value = _parser.Calculate(_formula) * scale;
			_value = Math.Clamp(_value, 0.0, 1.0);

			// UnityEngine.Debug.Log("         "+_neuron.ID+" receives "+_value);

		}
		public Neuron Source {
			get { return _sourceNeuronId == null ? null : _neuron.Host.Neurons[_sourceNeuronId]; }
		}
		public double Value {
			get { return _value; }
		}
		public double FixedValue {
			get { return _fixedValue; }
			set { _fixedValue = value; }
		}
		public string Formula {
			get { return _formula; }
			set { _formula = value; } //UnityEngine.Debug.Log ("Setting "+_neuron.ID+" source formula to " + _formula); }
		}
	}

	public class TemporaryNeuronAffector {
		private float _duration;
		private Neuron _neuron;
		private float _scale;
		public TemporaryNeuronAffector(float duration, Neuron neuron, float scale) { 
			_duration = duration;
			_neuron = neuron;
			_scale = scale;
		}

		public void Calculate(float scale = 1f) { 
			if (_duration < scale) scale = _duration;
			_duration -= scale;
			_neuron.Value += _scale * scale;

			// UnityEngine.Debug.Log("Adding "+(_scale * scale)+" to "+_neuron.ID);
		}

		public bool IsDone() { return (_duration <= 0f); }
	}


	public class Neuron {
		private List<NeuronInput> _neuronInputs  = new List<NeuronInput>();
		protected double _value;
		protected double _nextValue;
		private string _id;
		private NeuronNetwork _host = null;
		private bool _additive;
		public Neuron(string id, string formula, NeuronNetwork host = null, bool additive = true) {
			_id = id;
			_additive = additive;
			_nextValue = _value = 0f;
			AddToHost(host);
			AddFormula(formula);
		}
		public Neuron(string id, bool additive = true) {
			_id = id;
			_additive = additive;
			_host = null;
		}
		public List<NeuronInput> Inputs {
			get { return _neuronInputs; }
		}

		private void AddToHost(NeuronNetwork host) {
			if (_host != null) 
				_host.Neurons.Remove(_id);
			_host = host;
			if (_host != null)
				_host.Neurons.Add(_id, this);

			UnityEngine.Debug.Log("Added neuron "+_id);
		}
		public NeuronNetwork Host { 
			get { return _host; } 
			set { _host=value; } 
		}
		public string ID { get { return _id; } } 
		public virtual double Value { get { return _value; } set { _nextValue=value; } }

		public void AddFormula(string formula) {
			if (formula.Length == 0) return;

			string[] formulae = formula.Split(';');

			foreach (string frm in formulae) {
				//DebugX.Log(_debug, "NeuronLoader parsing "+items[i]);
				string[] keyFormula = frm.Split(':');
				if (keyFormula.Length != 2) {
					DebugX.Assert(keyFormula.Length == 2, "NeuronLoader parsing error: '"+frm+"' does not conform to 'key, value'");
					continue;
				}

				// DebugX.Log(true, "NeuronLoader inputs to neuron "+ID+" <- "+keyFormula[0].Trim() +", "+ keyFormula[1].Trim());

				Inputs.Add( new NeuronInput(this, keyFormula[0].Trim(), keyFormula[1].Trim()) );
			}
		}

		public void Prepare () {
			_nextValue = _value;
		}

		public void Calculate () {
			foreach (NeuronInput i in _neuronInputs)
				if (_additive) 
					_nextValue += i.Value;
				else
					_nextValue *= i.Value;

			_nextValue = Math.Clamp(_nextValue, 0.0, 1.0);

//			UnityEngine.Debug.Log("Updating "+ID+" to "+_value+" additive = "+_additive);

		}

		public void Conclude() {
			_value = _nextValue;
		}


	}
}

