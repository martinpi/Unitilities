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
using UnityEngine.UI;
using System.Collections.Generic;

namespace Unitilities.UI {

	[AddComponentMenu("UI/Line", 56)]
	[RequireComponent (typeof (RectTransform))]
	[ExecuteInEditMode]
	public class Line : Graphic {
		public float Width = 1f;

		protected override void OnPopulateMesh(VertexHelper vh) {
			Vector2 corner1 = Vector2.zero;
			Vector2 corner2 = Vector2.one;

			corner1.x -= rectTransform.pivot.x;
			corner1.y -= rectTransform.pivot.y;
			corner2.x -= rectTransform.pivot.x;
			corner2.y -= rectTransform.pivot.y;

			corner1.x *= rectTransform.rect.width;
			corner1.y *= rectTransform.rect.height;
			corner2.x *= rectTransform.rect.width;
			corner2.y *= rectTransform.rect.height;

			Vector2 normal = (corner2 - corner1).normalized;

			var uv = new Vector4(0f, 0f, 1f, 1f);

			var color32 = color;

			vh.AddVert(new Vector3(corner1.x + normal.y * Width / 2f, corner1.y - normal.x * Width / 2f), color32, new Vector2(uv.x, uv.y));
			vh.AddVert(new Vector3(corner1.x - normal.y * Width / 2f, corner1.y + normal.x * Width / 2f), color32, new Vector2(uv.x, uv.w));
			vh.AddVert(new Vector3(corner2.x - normal.y * Width / 2f, corner2.y + normal.x * Width / 2f), color32, new Vector2(uv.z, uv.w));
			vh.AddVert(new Vector3(corner2.x + normal.y * Width / 2f, corner2.y - normal.x * Width / 2f), color32, new Vector2(uv.z, uv.y));

			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}
	}
}
