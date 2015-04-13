using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Simulation {

	public class SimulationValue {

		private static Dictionary<string, SimulationValue> _allResources = new Dictionary<string, SimulationValue>();
		public static Dictionary<string, SimulationValue> All { get { return _allResources; } }

		private string _id;
		private string _symbol;
		private string _description;
		private bool _multiply;
		private string[] _formulas;

		public string ID { get { return _id; } } 
		public string Symbol { get { return _symbol; } } 
		public string Description { get { return _description; } } 
		public bool Multiply { get { return _multiply; } }
		public string[] Formulas { get { return _formulas; } }

		public SimulationValue(string id, string symbol, string description, bool multiply, string[] formulas) {
			_id = id; _symbol = symbol; _description = description; _multiply = multiply; _formulas = formulas;

			Debug.Log("New "+id +" with formula[0] "+formulas[0]);

		}
	}

	public static class SimulationValueLoader {

		public static void Load (string filename) {
			List<string> simulation = TextFileReader.LoadTextList(filename);
			foreach (string line in simulation) {
				string[] values = TextFileReader.SplitCsvLine(line);
				if (values == null || values[0]!="#") continue;

				bool multiplicative = float.Parse(values[4]) > 0.0f;

				string[] formulas = new string[values.Length-5];
				for (int i = 0; i<values.Length-5; ++i)
					formulas[i] = values[5+i];

				SimulationValue val = new SimulationValue(values[1], values[2], values[3], multiplicative, formulas);
				SimulationValue.All.Add(values[1], val);
			}
		}
	}
}

