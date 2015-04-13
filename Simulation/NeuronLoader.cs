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
