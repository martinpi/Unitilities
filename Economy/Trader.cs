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

	public class Trader : MonoBehaviour {

		//	private EconomyManager _economyManager = null;
		private Stock _stock;
		//	private Consumer _consumer;
		//	private Producer _producer;

		public int Cash = 1000;

		public void Start() {
//		_economyManager = GameObject.Find("Universe").GetComponent<EconomyManager>();
			_stock = GetComponent<Stock>();
//		_consumer = GetComponent<Consumer>();
//		_producer = GetComponent<Producer>();
		}
	
		//	public int Buy(StoredGoods goods) {
		//		StoredGoods demanded = _demandedGoodTypes[goods.Description.Name];
		//		int traded = Mathi.Clamp(demanded.Amount-goods.Amount, 0, _cash / goods.Price);
		//
		//		int value = goods.Price * traded;
		//		demanded.Amount -= traded;
		//		demanded.Price = (int)((float)goods.Description.BasePrice * 1f/goods.Description.Probability + 0.01f * (float)demanded.Amount);
		//		_cash -= value;
		//		return value;
		//	}

		public bool Offers( StoredResource goods ) {
			StoredResource available = _stock.Lookup(goods.Description.ID, goods.Amount);
			if (available.Amount > 0 && available.Price < goods.Price)
				return true;
			return false;
		}

		public StoredResource Sell( StoredResource goods, int price ) {
			StoredResource sold = _stock.Retrieve(goods.Description.ID, goods.Amount);
			Cash += (int)sold.Amount * price;

			Debug.Log(name + " selling for " + ((int)sold.Amount * price) + " -> Cash " + Cash);

			return sold;
		}

		public StoredResource Buy( StoredResource goods, int price ) {
			goods.Amount = (float)Mathi.Clamp((int)goods.Amount, 0, Cash / price);
			Cash -= (int)goods.Amount * price;

			Debug.Log(name + " buying for " + ((int)goods.Amount * price) + " -> Cash " + Cash);

			_stock.Add(goods);
			return goods;
		}

		public Stock GetStock() {
			return _stock;
		}

		public void TradeRound() {
		
		}
	}
}