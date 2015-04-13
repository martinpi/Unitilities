using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[AddComponentMenu("UI/Line", 56)]
[RequireComponent (typeof (RectTransform))]
[ExecuteInEditMode]
public class Line : Graphic 
{
	public float Width = 1f;
	
	protected override void OnFillVBO (List<UIVertex> vbo)
	{
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

		vbo.Clear();
		
		UIVertex vert = UIVertex.simpleVert;
		
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