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


//https://gist.github.com/col000r/6658520
//but all the hard work done by mstevenson: https://gist.github.com/mstevenson/4050130

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShuffleBag<T> : ICollection<T>, IList<T> {
	private List<T> data = new List<T>();
	private int cursor = 0;
	private T last;

	/// <summary>
	/// Get the next value from the ShuffleBag
	/// </summary>
	public T Next() {
		if (cursor < 1) {
			cursor = data.Count - 1;
			if (data.Count < 1)
				return default(T);
			return data[0];
		}
		int grab = Mathf.FloorToInt(Random.value * (cursor + 1));
		T temp = data[grab];
		data[grab] = this.data[this.cursor];
		data[cursor] = temp;
		cursor--;
		return temp;
	}

	public T[] Data() {
		return data.ToArray();
	}
	public void FromArray(T[] values) {
		Clear();
		for (int i = 0; i < values.Length; i++)
			Add(values[i]);
	}
	public bool Empty() {
		return (cursor < 1);
	}

	//This Constructor will let you do this: ShuffleBag<int> intBag = new ShuffleBag<int>(new int[] {1, 2, 3, 4, 5});
	public ShuffleBag(T[] initialValues) {
		FromArray(initialValues);
	}
	public ShuffleBag() { } //Constructor with no values

	public void Add(T[] items) {
		for (int i = 0; i < items.Length; i++)
			Add(items[i]);
	}

	#region IList[T] implementation
	public int IndexOf(T item) {
		return data.IndexOf(item);
	}

	public void Insert(int index, T item) {
		cursor = data.Count;
		data.Insert(index, item);
	}

	public void RemoveAt(int index) {
		cursor = data.Count - 2;
		data.RemoveAt(index);
	}

	public T this[int index] {
		get {
			return data[index];
		}
		set {
			data[index] = value;
		}
	}
	#endregion

	#region IEnumerable[T] implementation
	IEnumerator<T> IEnumerable<T>.GetEnumerator() {
		return data.GetEnumerator();
	}
	#endregion

	#region ICollection[T] implementation
	public void Add(T item) {
		data.Add(item);
		cursor = data.Count - 1;
	}

	public int Count {
		get {
			return data.Count;
		}
	}

	public void Clear() {
		data.Clear();
	}

	public bool Contains(T item) {
		return data.Contains(item);
	}

	public void CopyTo(T[] array, int arrayIndex) {
		foreach (T item in data) {
			array.SetValue(item, arrayIndex);
			arrayIndex = arrayIndex + 1;
		}
	}

	public bool Remove(T item) {
		cursor = data.Count - 2;
		return data.Remove(item);
	}

	public T Pop() {
		T pop = Next();
		Remove(pop);
		return pop;
	}

	public bool IsReadOnly {
		get {
			return false;
		}
	}
	#endregion

	#region IEnumerable implementation
	IEnumerator IEnumerable.GetEnumerator() {
		return data.GetEnumerator();
	}
	#endregion

}