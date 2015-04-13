using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Economy;

public class EconomyManager : MonoBehaviour {

	public float RoundDuration = 10f;
	public int MaxGoods = 4;
	public GameObject Host;

	private List<string> _resourceNames;

	private int _initRounds = 10;

	private List<Producer> _producers;
	private List<Trader> _traders;
	private List<Consumer> _consumers;

	public void Init () {
		_traders = new List<Trader>();
		_producers = new List<Producer>();
		_consumers = new List<Consumer>();
		_resourceNames = new List<string>();

		foreach (Resource r in Resource.All.Values) {
			_resourceNames.Add(r.ID);
		}

		for (int i=0;i<Host.transform.childCount; ++i) {
			GameObject ob = Host.transform.GetChild(i).gameObject;
			Trader trader = ob.GetComponent<Trader>();
			if (trader != null) _traders.Add(trader);

			Producer producer = ob.GetComponent<Producer>();
			if (producer != null) {
				_producers.Add(producer);
				producer.InitProduction(Random.value);
			}

			Consumer consumer = ob.GetComponent<Consumer>();
			if (consumer != null) {
				_consumers.Add(consumer);
				consumer.InitDemand(Random.value);
			}

			Stock stock = ob.GetComponent<Stock>();
			if (stock != null) {
				stock.Init();
			}
		}

		for (int i=0; i<_initRounds; ++i) {
			TickEconomy();
		}

		StartCoroutine(TradeRound());
	}

	public Resource GetResource(string id) {
		return Resource.All[id];
	}
	public int GetNumResources() {
		return Resource.All.Count;
	}
	public Resource GetRandomResource() {
		return Resource.All[_resourceNames[(int)(Random.value * _resourceNames.Count)]];
	}

	void TickEconomy() {
		foreach (Producer p in _producers) p.TradeRound();
		foreach (Consumer c in _consumers) c.TradeRound();
		foreach (Trader t in _traders) t.TradeRound();
	}

	IEnumerator TradeRound() {
		yield return new WaitForSeconds(RoundDuration);
		TickEconomy();
		StartCoroutine(TradeRound());
	}
}
