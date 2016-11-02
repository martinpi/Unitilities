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
using System.Collections.Generic;

namespace Unitilities.Simulation {

	public class Resource {
		private static Dictionary<string, Resource> _allResources = new Dictionary<string, Resource>();
		public static Dictionary<string, Resource> All { get { return _allResources; } }

		private string _id;
		private string _symbol;
		private string _description;
		private float _probability;
		private bool _natural;

		public string ID { get { return _id; } } 
		public string Symbol { get { return _symbol; } } 
		public string Description { get { return _description; } } 
		public float Probability { get { return _probability; } } 
		public bool Natural { get { return _natural; } }

		public Resource(string id, string symbol, string description, float probability, bool natural) {
			_id = id; _symbol = symbol; _description = description; _probability = probability; _natural = natural;
		}
	}

	public class ResourceStorage {
		private Resource _resource;
		private double _amount;

		public ResourceStorage( Resource resource ) {
			_resource = resource;
			_amount = 0;
		}

		public Resource Resource { get { return _resource; } } 
		public double Amount { get { return _amount; } set { _amount = Utils.Math.Max(value, 0); } }
	}

	// public class ResourceAccumulator {
	// 	private ResourceStorage _resourceStorage;
	// 	private Neuron _neuron;
	// 	private double _factor;

	// 	/* positive factor -> producer // negative factor -> consumer */
	// 	public ResourceAccumulator( ResourceStorage resourceStorage, Neuron neuron, double factor ) {
	// 		_resourceStorage = resourceStorage;
	// 		_neuron = neuron;
	// 		_factor = factor;
	// 	}

	// 	public ResourceStorage ResourceStorage { get { return _resourceStorage; } } 
	// 	public Neuron Neuron { get { return _neuron; } }

	// 	public void Calculate() {
	// 		_resourceStorage.Amount += _factor * _neuron.Value;
	// 	}
	// }

	public static class ResourceLoader {

		public static void LoadDescriptions(string filename) {
			List<string> simulation = TextFileReader.LoadTextList(filename);
			foreach (string line in simulation) {
				string[] items = line.Split('\t');
				
				if (items[0]!="#") continue;

				Resource res = new Resource(items[1], items[2], items[3], System.Single.Parse(items[4]), System.Boolean.Parse(items[5]));

//				Debug.Log("Adding resource "+res.ID+" ("+res.Symbol+")");

				Resource.All.Add(res.ID, res);
			}
		}
	}
}
