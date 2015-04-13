using UnityEngine;
using System.Collections.Generic;

using Economy;

public class Consumer : MonoBehaviour {

	private EconomyManager _economyManager = null;
	private Stock _stock;

	private int _maxGoods = 8;
	private int _numDemandedGoods;
	private Dictionary<string,Demand> _demand = null;

	private float _scale = 1f;

	public void InitDemand(float scale) {
		_economyManager = GameObject.Find("Universe").GetComponent<EconomyManager>();
		_maxGoods = _economyManager.MaxGoods;
		_stock = GetComponent<Stock>();
		_demand = new Dictionary<string,Demand>();
		_scale = scale;
		_numDemandedGoods = Mathi.Clamp ((int)(Random.value * scale),2,_maxGoods);

		for (int i=0; i<_numDemandedGoods;++i) {
			Resource desc = _economyManager.GetRandomResource();
			Demand demand;
			demand.Description = desc;
			demand.Consumed = (_scale * Random.value * desc.Probability);

			if (!_demand.ContainsKey(demand.Description.ID))
				_demand.Add( demand.Description.ID, demand );
		}
	}

	void Consume() {
		foreach (Demand demand in _demand.Values) {
			_stock.Retrieve(demand.Description.ID, demand.Consumed);
		}
	}

	public string[] GetList() {
		string[] returnString = new string[_demand.Count];
		int i=0;
		foreach (Demand demand in _demand.Values) {
			returnString[i++] = demand.Description.ID +" (-"+ demand.Consumed+"/turn)";
		}
		return returnString;
	}

	public void TradeRound() {
		Consume();
	}
}
