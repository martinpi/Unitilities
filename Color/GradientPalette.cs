/*
The MIT License

Copyright (c) 2016 Martin Pichlmair

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

namespace Unitilities {

	namespace Color {
		
		public class GradientPalette : MonoBehaviour {

			public UnityEngine.Color start = UnityEngine.Color.white;
			public UnityEngine.Color finish = UnityEngine.Color.black;

			public int paletteSize = 10;
			public Color32[] palette;

			void Start () {
				Recreate();
			}

			public void SetFromTo(UnityEngine.Color start, UnityEngine.Color finish) {
				this.start = start;
				this.finish = finish;

				for (int i=0; i<paletteSize; ++i) {
					float t = ((float)i)/((float)paletteSize);
					palette[i] = UnityEngine.Color.Lerp(start, finish, t).ToColor32();
				}
			}

			public void Recreate() {
				palette = new Color32[paletteSize];
				SetFromTo(start,finish);
			}
		}

	}

}

