using System.Collections;
using System.Collections.Generic;

public class MultiMap<V>
{
    Dictionary<float, List<V>> _dictionary = new Dictionary<float, List<V>>();

    public void Add(float key, V value)
    {
		List<V> list;
		if (this._dictionary.TryGetValue(key, out list))
		{
		    list.Add(value);
		}
		else
		{
		    list = new List<V>();
		    list.Add(value);
		    this._dictionary[key] = list;
		}
    }
	
	public void Remove(float key) {
		this._dictionary.Remove(key);
	}

    public IEnumerable<float> Keys
    {
		get
		{
		    return this._dictionary.Keys;
		}
    }

    public List<V> this[float key]
    {
		get
		{
		    List<V> list;
		    if (!this._dictionary.TryGetValue(key, out list))
		    {
				list = new List<V>();
				this._dictionary[key] = list;
		    }
		    return list;
		}
    }
}