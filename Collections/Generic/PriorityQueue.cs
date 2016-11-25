//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER 
//  REMAINS UNCHANGED.
//
//  Email:  gustavo_franco@hotmail.com
//
//  Copyright (C) 2015 Martin Pichlmair
//  Copyright (C) 2006 Franco, Gustavo 
//
// EDIT 2010 by Christoph Husse: Update() method didn't work correctly. Also
// each item is now carrying an index, so that updating can be performed
// efficiently.
//

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

	public interface IIndexedObject {
		int Index { get; set; }
	}

	class PriorityQueue<T> where T : IIndexedObject {
		protected List<T> InnerList = new List<T>();
		protected IComparer<T> mComparer;

		public PriorityQueue () {
			mComparer = Comparer<T>.Default;
		}

		public PriorityQueue ( IComparer<T> comparer ) {
			mComparer = comparer;
		}

		public PriorityQueue ( IComparer<T> comparer, int capacity ) {
			mComparer = comparer;
			InnerList.Capacity = capacity;
		}

		protected void SwitchElements( int i, int j ) {
			T h = InnerList[i];
			InnerList[i] = InnerList[j];
			InnerList[j] = h;

			InnerList[i].Index = i;
			InnerList[j].Index = j;
		}

		protected virtual int OnCompare( int i, int j ) {
			return mComparer.Compare(InnerList[i], InnerList[j]);
		}

		/// <summary>
		/// Push an object onto the PQ
		/// </summary>
		/// <param name="O">The new object</param>
		/// <returns>The index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.</returns>
		public int Push( T item ) {
			int p = InnerList.Count, p2;
			item.Index = InnerList.Count;
			InnerList.Add(item); // E[p] = O

			do {
				if (p == 0)
					break;
				p2 = (p - 1) / 2;
				if (OnCompare(p, p2) < 0) {
					SwitchElements(p, p2);
					p = p2;
				} else
					break;
			} while (true);
			return p;
		}

		/// <summary>
		/// Get the smallest object and remove it.
		/// </summary>
		/// <returns>The smallest object</returns>
		public T Pop() {
			T result = InnerList[0];
			int p = 0, p1, p2, pn;

			InnerList[0] = InnerList[InnerList.Count - 1];
			InnerList[0].Index = 0;

			InnerList.RemoveAt(InnerList.Count - 1);

			result.Index = -1;

			do {
				pn = p;
				p1 = 2 * p + 1;
				p2 = 2 * p + 2;
				if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
                    p = p1;
				if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
                    p = p2;

				if (p == pn)
					break;
				SwitchElements(p, pn);
			} while (true);

			return result;
		}

		/// <summary>
		/// Notify the PQ that the object at position i has changed
		/// and the PQ needs to restore order.
		/// </summary>
		public void Update( T item ) {
			int count = InnerList.Count;

			// usually we only need to switch some elements, since estimation won't change that much.
			while ((item.Index - 1 >= 0) && (OnCompare(item.Index - 1, item.Index) > 0)) {
				SwitchElements(item.Index - 1, item.Index);
			}

			while ((item.Index + 1 < count) && (OnCompare(item.Index + 1, item.Index) < 0)) {
				SwitchElements(item.Index + 1, item.Index);
			}
		}

		/// <summary>
		/// Get the smallest object without removing it.
		/// </summary>
		/// <returns>The smallest object</returns>
		public T Peek() {
			if (InnerList.Count > 0)
				return InnerList[0];
			return default(T);
		}

		public void Clear() {
			InnerList.Clear();
		}

		public int Count {
			get { return InnerList.Count; }
		}
	}
}

