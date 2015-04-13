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
using System.Collections.Generic;
using Simulation;
using Utils;

[RequireComponent (typeof (NeuronNetworkHost))]
public class NeuronLoader : MonoBehaviour {

	public string Configuration = "simulation";

	private NeuronNetwork _network = null;
	private bool _debug = true;

	public void Init() {
		_network = gameObject.GetComponent<NeuronNetworkHost>().Network;
		Load(Configuration);
	}

	/* Format
	 * # ID NAME MULT? 
	
	 */

	public void Load(string filename) {
		List<string> simulation = TextFileReader.LoadTextList(filename);
		int nr = 0;
		foreach (string line in simulation) {
			string[] items = line.Split('\t');
			
			if (items[0]!="#") continue;

			bool multiplicative = float.Parse(items[3]) > 0.0f;

			Neuron neu = new Neuron(items[1], "", _network, !multiplicative);

			DebugX.Log(_debug, "NeuronLoader adding neuron: "+neu.ID);

			for (int i=7; i<items.Length; ++i) {
				DebugX.Log(_debug, "NeuronLoader parsing "+items[i]);
				string[] keyFormula = items[i].Split(':');
				if (keyFormula.Length != 2) {
					DebugX.Assert(keyFormula.Length == 2, "NeuronLoader parsing error in line "+nr+": '"+items[i]+"' does not conform to 'key, value'");
					continue;
				}

				DebugX.Log(_debug, "NeuronLoader inputs to neuron "+neu.ID+" <- "+keyFormula[0].Trim() +", "+ keyFormula[1].Trim());

				neu.Inputs.Add( new NeuronInput(neu, keyFormula[0].Trim(), keyFormula[1].Trim()) );
			}

			nr++;
		}
	}
}
