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

namespace Unitilities.Economy {

	public class Consumer : MonoBehaviour {

		private EconomyManager _economyManager = null;
		private Stock _stock;

		private int _maxGoods = 8;
		private int _numDemandedGoods;
		private Dictionary<string,Demand> _demand = null;

		private float _scale = 1f;

		public void InitDemand( float scale ) {
			_economyManager = GameObject.Find("Universe").GetComponent<EconomyManager>();
			_maxGoods = _economyManager.MaxGoods;
			_stock = GetComponent<Stock>();
			_demand = new Dictionary<string,Demand>();
			_scale = scale;
			_numDemandedGoods = Mathi.Clamp((int)(Random.value * scale), 2, _maxGoods);

			for (int i = 0; i < _numDemandedGoods; ++i) {
				Resource desc = _economyManager.GetRandomResource();
				Demand demand;
				demand.Description = desc;
				demand.Consumed = (_scale * Random.value * desc.Probability);

				if (!_demand.ContainsKey(demand.Description.ID))
					_demand.Add(demand.Description.ID, demand);
			}
		}

		void Consume() {
			foreach (Demand demand in _demand.Values) {
				_stock.Retrieve(demand.Description.ID, demand.Consumed);
			}
		}

		public string[] GetList() {
			string[] returnString = new string[_demand.Count];
			int i = 0;
			foreach (Demand demand in _demand.Values) {
				returnString[i++] = demand.Description.ID + " (-" + demand.Consumed + "/turn)";
			}
			return returnString;
		}

		public void TradeRound() {
			Consume();
		}
	}
}