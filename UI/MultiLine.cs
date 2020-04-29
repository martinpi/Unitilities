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
using System.Collections;

namespace Unitilities.UI {


	[AddComponentMenu("UI/MultiLine", 57)]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class MultiLine : Graphic {
		public float Width = 1f;

		[SerializeField]
		public List<Vector2> Points;

		protected override void OnPopulateMesh(VertexHelper vh) {
			vh.Clear();
			UIVertex vert = UIVertex.simpleVert;

			for (int i = 1; i < Points.Count; ++i) {

				Vector2 corner1 = Points[i - 1];
				Vector2 corner2 = Points[i];

				Vector2 normal = (corner2 - corner1).normalized;

				vert.position = new Vector2(corner1.x + normal.y * Width / 2f, corner1.y - normal.x * Width / 2f);
				vert.color = color;
				vh.AddVert(vert);

				vert.position = new Vector2(corner1.x - normal.y * Width / 2f, corner1.y + normal.x * Width / 2f);
				vert.color = color;
				vh.AddVert(vert);

				vert.position = new Vector2(corner2.x - normal.y * Width / 2f, corner2.y + normal.x * Width / 2f);
				vert.color = color;
				vh.AddVert(vert);

				vert.position = new Vector2(corner2.x + normal.y * Width / 2f, corner2.y - normal.x * Width / 2f);
				vert.color = color;
				vh.AddVert(vert);
			}
		}
	}
}