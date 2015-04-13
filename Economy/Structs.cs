using UnityEngine;
using System.Collections;
using Simulation;

namespace Economy {
	public struct StoredResource {
		public Resource Description;
		public float Amount;
		public int Price;
		public int PriceSale;
	}
	
	public struct Demand {
		public Resource Description;
		public float Consumed;
	}
	
	public struct Production {
		public Resource Description;
		public float Produced;
	}
}



