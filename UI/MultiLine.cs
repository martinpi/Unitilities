using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


[AddComponentMenu("UI/MultiLine", 57)]
[RequireComponent (typeof (RectTransform))]
[ExecuteInEditMode]
public class MultiLine : Graphic 
{
	public float Width = 1f;

	[SerializeField]
	public List<Vector2> Points;

	protected override void OnFillVBO (List<UIVertex> vbo)
	{
		vbo.Clear();
		UIVertex vert = UIVertex.simpleVert;
		
		for (int i=1; i<Points.Count; ++i) {

			Vector2 corner1 = Points[i-1];
			Vector2 corner2 = Points[i];

			Vector2 normal = (corner2 - corner1).normalized;

			vert.position = new Vector2( corner1.x + normal.y * Width/2f, corner1.y - normal.x * Width/2f );
			vert.color = color;
			vbo.Add(vert);
			
			vert.position = new Vector2( corner1.x - normal.y * Width/2f, corner1.y + normal.x * Width/2f );
			vert.color = color;
			vbo.Add(vert);
			
			vert.position = new Vector2( corner2.x - normal.y * Width/2f, corner2.y + normal.x * Width/2f );
			vert.color = color;
			vbo.Add(vert);
			
			vert.position = new Vector2( corner2.x + normal.y * Width/2f, corner2.y - normal.x * Width/2f );
			vert.color = color;
			vbo.Add(vert);
		}
	}
}