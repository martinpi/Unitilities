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

using System.Collections;
using System.Collections.Generic;

namespace Unitilities.Collections.Generic {
	public class MultiMap<V> {
		Dictionary<float, List<V>> _dictionary = new Dictionary<float, List<V>>();

		public void Add( float key, V value ) {
			List<V> list;
			if (this._dictionary.TryGetValue(key, out list)) {
				list.Add(value);
			} else {
				list = new List<V>();
				list.Add(value);
				this._dictionary[key] = list;
			}
		}

		public void Remove( float key ) {
			this._dictionary.Remove(key);
		}

		public IEnumerable<float> Keys {
			get {
				return this._dictionary.Keys;
			}
		}

		public List<V> this [float key] {
			get {
				List<V> list;
				if (!this._dictionary.TryGetValue(key, out list)) {
					list = new List<V>();
					this._dictionary[key] = list;
				}
				return list;
			}
		}
	}
}