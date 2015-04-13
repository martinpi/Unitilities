using UnityEngine;
using System.Collections.Generic;

using Economy;

public class Stock : MonoBehaviour {

	private EconomyManager _economyManager = null;
	private Dictionary<string,StoredResource> _stock = null;
	private Dictionary<string,float> _stockLimits = null;

	public void Init() {
		_economyManager = GameObject.Find("Universe").GetComponent<EconomyManager>();
		_stock = new Dictionary<string,StoredResource>();
		_stockLimits = new Dictionary<string, float>();
	}

	public Dictionary<string,StoredResource> GetStock() { return _stock; }

	public void EnableAllGoods() {
		foreach (Resource r in Resource.All.Values) {

			StoredResource stored;
			stored.Description = _economyManager.GetResource(stored.Description.ID);
			stored.Amount = 0;
			stored.Price = 0;
			stored.PriceSale = 0;

			_stock.Add(r.ID, stored);
			_stockLimits.Add(stored.Description.ID, 10f);
		}

	}

	public void Add (StoredResource goods) {
		if (goods.Amount <= 0f) return;
		StoredResource stored;
		if (!_stock.TryGetValue(goods.Description.ID, out stored)) {
			_stock.Add(goods.Description.ID, goods);
			_stockLimits.Add(goods.Description.ID, 10f);
		} else {
			float stockAmount = Mathf.Min(goods.Amount + stored.Amount, _stockLimits[goods.Description.ID]);

			//TODO: fixe pricing changes
			stored.Price = (int)(((float)stored.Price * stored.Amount + (float)goods.Price * goods.Amount)/(stored.Amount + goods.Amount));
			stored.PriceSale = (int)(((float)stored.PriceSale * stored.Amount + (float)goods.PriceSale * goods.Amount)/(stored.Amount + goods.Amount));
			stored.Amount = stockAmount;
			_stock[goods.Description.ID] = stored;
		}
		//		Debug.Log(stored.Description.ID + " now in stock: " + stored.Amount);
	}

	public StoredResource Lookup( string goodsName, float amount ) {
		StoredResource returnGoods;
		returnGoods.Description = _economyManager.GetResource(goodsName);
		returnGoods.Amount = 0f;
		returnGoods.Price = 0;
		returnGoods.PriceSale = 0;
		
		StoredResource stored;
		if (_stock.TryGetValue(goodsName, out stored)) {
			returnGoods.Price = stored.Price;
			returnGoods.PriceSale = stored.PriceSale;
			returnGoods.Amount = (float)Mathi.Min((int)amount, (int)stored.Amount);
		}
		return returnGoods;
	}

	public StoredResource Retrieve( string goodsName, float amount ) {
		StoredResource returnGoods;
		returnGoods.Description = _economyManager.GetResource(goodsName);
		returnGoods.Amount = 0;
		returnGoods.Price = 0;
		returnGoods.PriceSale = 0;

		StoredResource stored;
		if (_stock.TryGetValue(goodsName, out stored)) {
			returnGoods.Price = stored.Price;
			returnGoods.PriceSale = stored.PriceSale;
			returnGoods.Amount = (float)Mathi.Min((int)amount, (int)stored.Amount);
			stored.Amount -= returnGoods.Amount;
			stored.Price = (int)(1f/stored.Description.Probability - 0.01f * (float)stored.Amount);
			_stock[goodsName] = stored;
		}
		return returnGoods;
	}

	public string[] GetList() {
		string[] returnString = new string[_stock.Count];
		int i=0;
		foreach (KeyValuePair<string, StoredResource> entry in _stock) {
			returnString[i++] = entry.Key +" ("+(int)entry.Value.Amount+" stored a "+entry.Value.Price+")";
		}
		return returnString;
	}
}

