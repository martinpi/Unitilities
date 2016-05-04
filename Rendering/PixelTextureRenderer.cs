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
	namespace Rendering {

		[ExecuteInEditMode]
		[RequireComponent(typeof(Renderer))]
		public class PixelTextureRenderer : MonoBehaviour {

			private Texture2D _texture = null;

		//	public Palette palette;

			void Awake() {
		//		palette = GetComponent<Palette>();
			}

			public void Recreate(int width, int height) {
				_texture = new Texture2D(width, height);
				GetComponent<Renderer>().sharedMaterial.mainTexture = _texture;
				_texture.filterMode = FilterMode.Point;
				_texture.wrapMode = TextureWrapMode.Clamp;
				_texture.Apply();
			}

			public void SetPixel(int x, int y, UnityEngine.Color color) {
				if (_texture != null) _texture.SetPixel(x,y,color);
			}

		//	public void SetPixel(int x, int y, int index) {
		//		if (_texture != null && palette != null) _texture.SetPixel(x,y,palette.palette[index % palette.paletteSize].ToColor());
		//	}

			public void SetPixels32(Color32[] colors) {
				if (_texture != null) _texture.SetPixels32(colors);
			}

			public void Clear() {
				int textureSize = _texture.width * _texture.height;
				Color32[] clear = new Color32[textureSize];
				for (int i=0; i<textureSize; ++i) clear[i] = new Color32(0,0,0,255);
				_texture.SetPixels32(clear);
				_texture.Apply();
			}

			public void Apply() {
				_texture.Apply();
			}
		}

	}
}
