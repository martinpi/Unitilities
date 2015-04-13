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

using Economy;

public class Producer : MonoBehaviour {

	private EconomyManager _economyManager = null;
	private Stock _stock;

	private int _maxGoods = 8;
	private List<Production> _producedGoodTypes = null;
	private float _capability = 1f;

	public void InitProduction(float capability) {
		_economyManager = GameObject.Find("Universe").GetComponent<EconomyManager>();
		_stock = GetComponent<Stock>();
		_maxGoods = _economyManager.MaxGoods;
		_producedGoodTypes = new List<Production>();
		_capability = capability;

		foreach (Resource desc in Resource.All.Values) {
			if (Random.value * (1-capability) <= desc.Probability) {
				Production prod;
				prod.Description = desc;
				prod.Produced = capability * desc.Probability * Random.value;
				_producedGoodTypes.Add(prod);
			}
		}

		while (_producedGoodTypes.Count > _maxGoods)
			_producedGoodTypes.RemoveAt((int)(Random.value)*_producedGoodTypes.Count);
	}

	void Produce() {
		foreach (Production prod in _producedGoodTypes) {
			Resource desc = prod.Description;
			
			StoredResource good;
			good.Description = desc;
			good.Amount = prod.Produced;
			good.Price = (int)(1f/desc.Probability * Random.value / _capability);
			good.PriceSale = (int)((float)good.Price * 1.2f);

//			Debug.Log("Produced +"+good.Amount+" "+good.Description.Name);

			_stock.Add(good);
		}
	}

	public string[] GetList() {
		string[] returnString = new string[_producedGoodTypes.Count];
		int i=0;
		foreach (Production prod in _producedGoodTypes) {
			returnString[i++] = prod.Description.ID +" (+"+prod.Produced+"/turn)";
		}
		return returnString;
	}

	public void TradeRound() {
		Produce();
	}
}
