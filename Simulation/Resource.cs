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

using Unitilities.File;

namespace Unitilities.Simulation {

	public struct Bin {
		public string resource;
		public int capacity;
		public int value;

		public Bin(string name, int capacity) {
			this.resource = name;
			this.capacity = capacity;
			this.value = 0;
		}
		public void AddValue(int amount) {
			value += amount;
			value = Mathi.Min(value, capacity);
		}
		public void SubtractValue(int amount) {
			value -= amount;
			value = Mathi.Max(value, 0);
		}
	}

	public class Unit {
		public string name;
		public string description = "";

		public Vector3 position = Vector3.zero;
		public Vector3 size = Vector3.zero;
		
		public Dictionary<string, Bin> bins = new Dictionary<string, Bin>(); // mapped to resources
		public List<string> rules = new List<string>();

		public Unit(string name) {
			this.name = name;
		}
	}

	public class Map {
		public string name;
		public int width;
		public int height;

		public Dictionary<string, Bin>[,] bins;

	}

	public enum RuleTypes {
		RULE,
		UNIT_RULE,
		MAP_RULE
	}

	public class Rule {
		public string name;
		public RuleTypes type = RuleTypes.RULE;
		public int rate = 1;
		
		public int lastTick = 0;

		public Dictionary<string, int> inResourceGlobal = new Dictionary<string, int>();
		public Dictionary<string, int> outResourceGlobal = new Dictionary<string, int>();
		
		public List<string> onSucceeded = new List<string>(); // TODO add more kinds of feedback
		public List<string> onFailed = new List<string>(); // TODO add more kinds of feedback

		public Rule(string name) {
			this.name = name;
		}
		public bool Run(Sim sim) {
			bool success = true;

			// first check if we have everything we need
			foreach(KeyValuePair<string, int> entry in inResourceGlobal) 
				success &= (sim.globalBins[entry.Key].value >= entry.Value);
			
			if (success) {
				foreach(KeyValuePair<string, int> entry in inResourceGlobal) {
					sim.globalBins[entry.Key].SubtractValue(entry.Value);
				}
				foreach(KeyValuePair<string, int> entry in outResourceGlobal) {
					sim.globalBins[entry.Key].AddValue(entry.Value);
				}
			}

			return success;
		}

		public void PostRun(bool success) {
			if (success) {
				foreach (string s in onSucceeded) {
					Debug.Log(s);
				}
			} else {
				foreach (string s in onFailed) {
					Debug.Log(s);
				}
			}
		}
	}
	public class UnitRule : Rule {
		public Dictionary<string, int> inResourceLocal = new Dictionary<string, int>();
		public Dictionary<string, int> outResourceLocal = new Dictionary<string, int>();
		
		public string destination = "";
		public string prefab =  "Placeholder";
		public string createPrefab = null; // a unit this rule creates

		public UnitRule(string name):base(name) {
			this.type = RuleTypes.UNIT_RULE;
		}
		public bool Run(Sim sim, Unit unit) {
			bool success = base.Run(sim);

			// first check if we have everything we need
			foreach(KeyValuePair<string, int> entry in inResourceLocal) 
				success &= (unit.bins[entry.Key].value >= entry.Value);
			
			if (success) {
				foreach(KeyValuePair<string, int> entry in inResourceLocal) {
					unit.bins[entry.Key].SubtractValue(entry.Value);
				}
				foreach(KeyValuePair<string, int> entry in outResourceLocal) {
					unit.bins[entry.Key].AddValue(entry.Value);
				}
			}

			if (success && createPrefab != null) {
				Unitilities.Utils.Helpers.CreateObjectAt(sim.prefabs[createPrefab], unit.position);
			}

			return success;
		}
	}
	public class MapRule : Rule {
		public Dictionary<string, int> inResourceMap = new Dictionary<string, int>();
		public Dictionary<string, int> outResourceMap = new Dictionary<string, int>();
		public MapRule(string name):base(name) {
			this.type = RuleTypes.MAP_RULE;
		}
	}

	public class Sim {
		public string name;
		public int tick = 0;

		public HashSet<string> resources;

		public Dictionary<string, Bin> globalBins; // mapped to resources
		public List<Rule> rules; // global rules
		public Dictionary<string, UnitRule> unitRules;
		public List<MapRule> mapRules;
		
		public Dictionary<string, GameObject> prefabs;

		public List<Unit> units;

		System.Random random;

		public Sim(int seed) {
			globalBins = new Dictionary<string, Bin>();
			rules = new List<Rule>();
			unitRules = new Dictionary<string, UnitRule>();
			mapRules = new List<MapRule>();
			random = new System.Random(seed);
		}
		public void CheckResource(string name) {
			if (!resources.Contains(name)) resources.Add(name);
		}
		public Unit AddUnit(GameObject go) {
			string name = go.name.Trim() + " #" + random.Next();
			Unit unit = new Unit(name);
			units.Add(unit);
			return unit;
		}
		public void Tick() {
			tick++;
			foreach (Rule r in rules) {
				if (r.lastTick + tick >= r.lastTick + r.rate) {
					bool success = r.Run(this);
					r.PostRun(success);
					r.lastTick = 0;
				}
			}

			foreach (Unit u in units) {
				foreach (string rule in u.rules) {
					UnitRule r = unitRules[rule];
					if (r.lastTick + tick >= r.lastTick + r.rate) {
						bool success = r.Run(this, u);
						r.PostRun(success);
						r.lastTick = 0;
					}
				}
			}
		}
	}


	public static class Loader {

		public static void LoadScript(Sim sim, string filename) {
			List<string> simulation = TextFileReader.LoadTextList(filename);
			bool inRule = false;
			int lineNr = 0;
			Rule currentRule = new Rule("invalid rule - check description");
			foreach (string line in simulation) {
				string l = line.Trim();
				if (l[0]!='#') {
					lineNr++;
					continue;
				}

				string[] tokens = l.Split('\t');

				switch (tokens[0])
				{
					case "sim": 
						sim.name = tokens[1];
					break;
					case "resource":
						sim.CheckResource(tokens[1]);
					break;
					case "globalBin":
						sim.CheckResource(tokens[1]);
						sim.globalBins[tokens[1]] = new Bin(tokens[1], int.Parse(tokens[2]));
					break;
					case "rule":
						Debug.Assert(!inRule, "LoadScript warning on line "+lineNr+": Rule definition while in rule: '"+tokens[0]+"'");
						inRule = true;
						currentRule = new Rule(tokens[1]);
					break;
					case "unitRule":
						Debug.Assert(!inRule, "LoadScript warning on line "+lineNr+": Rule definition while in rule: '"+tokens[0]+"'");
						inRule = true;
						currentRule = new UnitRule(tokens[1]);
					break;
					case "mapRule":
						Debug.Assert(!inRule, "LoadScript warning on line "+lineNr+": Rule definition while in rule: '"+tokens[0]+"'");
						inRule = true;
						currentRule = new MapRule(tokens[1]);
					break;

					// in rule parsing
					case "rate":
						Debug.Assert(inRule, "LoadScript warning on line "+lineNr+": Spurious symbol '"+tokens[0]+"'");
						currentRule.rate = int.Parse(tokens[1]);
					break;
					case "global":
						Debug.Assert(inRule, "LoadScript warning on line "+lineNr+": Spurious symbol '"+tokens[0]+"'");
						if (tokens[2] == "in")
							currentRule.inResourceGlobal[tokens[1]] = int.Parse(tokens[3]);
						else
							currentRule.outResourceGlobal[tokens[1]] = int.Parse(tokens[3]);
					break;
					case "local":
						Debug.Assert(inRule, "LoadScript warning on line "+lineNr+": Spurious symbol '"+tokens[0]+"'");
						if (tokens[2] == "in")
							(currentRule as UnitRule).inResourceLocal[tokens[1]] = int.Parse(tokens[3]);
						else
							(currentRule as UnitRule).outResourceLocal[tokens[1]] = int.Parse(tokens[3]);
					break;
					case "map":
						Debug.Assert(inRule, "LoadScript warning on line "+lineNr+": Spurious symbol '"+tokens[0]+"'");
						if (tokens[2] == "in")
							(currentRule as MapRule).inResourceMap[tokens[1]] = int.Parse(tokens[3]);
						else
							(currentRule as MapRule).outResourceMap[tokens[1]] = int.Parse(tokens[3]);
					break;
					case "onSucceeded":
						Debug.Assert(inRule, "LoadScript warning on line "+lineNr+": Spurious symbol '"+tokens[0]+"'");
						currentRule.onSucceeded.Add(tokens[1]);
					break;
					case "onFailed":
						Debug.Assert(inRule, "LoadScript warning on line "+lineNr+": Spurious symbol '"+tokens[0]+"'");
						currentRule.onFailed.Add(tokens[1]);
					break;

					case "behaviour":
						Debug.Assert(inRule, "LoadScript warning on line "+lineNr+": Spurious symbol '"+tokens[0]+"'");
						Debug.Assert(currentRule.type == RuleTypes.UNIT_RULE, "LoadScript warning on line "+lineNr+": Rule '"+tokens[0]+"' not unit rule but has behaviour");
						if (tokens[1] == "destination")
							(currentRule as UnitRule).destination = tokens[2];
						if (tokens[1] == "prefab")
							(currentRule as UnitRule).prefab = tokens[2];
					break;

					case "create": // only units can create units for now
						Debug.Assert(inRule, "LoadScript warning on line "+lineNr+": Spurious symbol '"+tokens[0]+"'");
						Debug.Assert(currentRule.type == RuleTypes.UNIT_RULE, "LoadScript warning on line "+lineNr+": Rule '"+tokens[0]+"' not unit rule but has behaviour");
						if (tokens[1] == "prefab")
							(currentRule as UnitRule).createPrefab = tokens[2];
					break;


					case "end":
						Debug.Assert(inRule, "LoadScript warning on line "+lineNr+": Spurious symbol '"+tokens[0]+"'");
						
						if (currentRule.type == RuleTypes.RULE) sim.rules.Add(currentRule);
							else if (currentRule.type == RuleTypes.UNIT_RULE) sim.unitRules[currentRule.name] = currentRule as UnitRule;
							else if (currentRule.type == RuleTypes.MAP_RULE) sim.mapRules.Add(currentRule as MapRule);
						inRule = false;
					break;
					default: 
						Debug.LogWarning("LoadScript warning on line "+lineNr+": Ignoring line! No parsing rule for "+tokens[0]);
					break;
				}

				// Resource res = new Resource(items[1], items[2], items[3], System.Single.Parse(items[4]), System.Boolean.Parse(items[5]));
//				Debug.Log("Adding resource "+res.ID+" ("+res.Symbol+")");

				lineNr++;
			}
		}
	}

}
