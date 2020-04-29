﻿/*
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

	[AddComponentMenu("UI/Hexagon", 56)]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class Hexagon : Graphic {
		protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs) {
			UIVertex[] vbo = new UIVertex[4];
			for (int i = 0; i < vertices.Length; i++) {
				var vert = UIVertex.simpleVert;
				vert.color = color;
				vert.position = vertices[i];
				vert.uv0 = uvs[i];
				vbo[i] = vert;
			}
			return vbo;
		}

		protected override void OnPopulateMesh(VertexHelper vh) {

			float r = rectTransform.rect.width / 2f;
			float h = Mathf.Sin(2f * Mathf.PI / 6f) * 2f * r;
			float t = h / 2f;

			float rr = 0.5f;
			float p = h / 2f - rr;
			float s = 3f / 2f * rr;
			Vector2[] uv = new Vector2[] {
			new Vector2(2f * rr - s,    p),
			new Vector2(0f,             rr),
			new Vector2(2f * rr - s,    2f * rr - p),
			new Vector2(s,              2f * rr - p),
			new Vector2(1f,             rr),
			new Vector2(s,              p)
		};

			Vector2[] verts = new Vector2[] {
			new Vector2(-r/2f, -t),
			new Vector2(-r, 0f),
			new Vector2(-r/2f,  t),
			new Vector2( r/2f,  t),
			new Vector2( r, 0f),
			new Vector2( r/2f, -t)
		};

			vh.Clear();
			vh.AddUIVertexQuad(SetVbo(
				new[] { verts[0], verts[1], verts[2], verts[3] },
				new[] { uv[0], uv[1], uv[2], uv[3] }));
			vh.AddUIVertexQuad(SetVbo(
				new[] { verts[0], verts[3], verts[4], verts[5] },
				new[] { uv[0], uv[3], uv[4], uv[5] }));

		}
	}
}