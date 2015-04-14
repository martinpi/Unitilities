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

/*
 * Source: http://answers.unity3d.com/questions/637552/how-to-create-a-dynamic-2d-color-changing-hex-grid.html
 *
 * Material supplied has to support vertices and indizes. Best works with unlit vertex colored shaders.
 */


using UnityEngine;

public class HexGrid : MonoBehaviour {

	public int columns      = 10; 
	public int rows         = 10;
	public float cellRadius = 0.5f;
	public Material material;
	
	private Color[] colors;
	private Mesh mesh;

	void Start () {
		gameObject.AddComponent<MeshRenderer>();
		gameObject.GetComponent<MeshRenderer>().material = material;
		
		CreateHexMesh();
		
		// Create a colors array for the mesh
		colors = new Color[columns * rows * 6];
		for (int i = 0; i < colors.Length; i+=6) {
			Color color = new Color(Random.value, Random.value, Random.value, 1.0f);
			for (int j = 0; j < 6; j++) {
				colors[i+j] = color;
			}
		}
		mesh.colors = colors;
		
		SetColor(1,1,Color.red);
		SetColor(rows-1, columns-1, Color.black);
	}
	
	
	// Creates a mesh of hex objects 
	void CreateHexMesh() {
		MeshFilter mf = gameObject.AddComponent<MeshFilter>();
		mesh = new Mesh();
		mf.mesh = mesh;
		
		float vertSpacing = 2.0f * Mathf.Cos(30.0f * Mathf.Deg2Rad) * cellRadius;
		float horzSpacing = cellRadius + Mathf.Sin(30.0f * Mathf.Deg2Rad) * cellRadius;
		
		Vector3 currPos = new Vector3(-columns / 2.0f * horzSpacing, rows / 2.0f * vertSpacing, 0.0f); 
		
		// Layout vertices for a single hex cell
		Vector3[] hexVerts = new Vector3[6];
		hexVerts[0] = Vector3.zero;
		Vector3 v = Vector3.right * cellRadius;
		
		for (int i = 0; i < 6; i++) {
			hexVerts[i] = v;
			v = Quaternion.AngleAxis(60.0f, -Vector3.forward) * v;
		}
		
		// Create the vertices
		Vector3[] vertices = new Vector3[rows * columns * 6];
		int currVert = 0;
		for (int i = 0; i < columns; i++) {
			for (int j = 0; j < rows; j++) {
				for (int k = 0; k < hexVerts.Length; k++) {
					vertices[currVert++] = hexVerts[k] + currPos;
				}
				currPos.y -= vertSpacing;
			}
			currPos.x += horzSpacing;
			currPos.y = rows / 2.0f * vertSpacing;
			if (i % 2 == 0) 
				currPos.y -= vertSpacing / 2.0f;
		}
		
		mf.mesh.vertices = vertices;
		
		//Create the triangles
		int curr = 0;
		int[] triangles = new int[rows * columns * 4 * 3];
		for (int i = 0; i < columns * rows * 6; i += 6) {
			triangles[curr++] = i+0;
			triangles[curr++] = i+1;
			triangles[curr++] = i+5;
			
			triangles[curr++] = i+5;
			triangles[curr++] = i+1;
			triangles[curr++] = i+4;
			
			triangles[curr++] = i+4;
			triangles[curr++] = i+1;
			triangles[curr++] = i+2;
			
			triangles[curr++] = i+2;
			triangles[curr++] = i+3;
			triangles[curr++] = i+4;
		}
		
		mf.mesh.triangles = triangles;
		
		mf.mesh.colors = colors;
		mf.mesh.RecalculateBounds();
		mf.mesh.RecalculateNormals();
	}
	
	
	void SetColor(int row, int col, Color color) {
		if (row < 0 || row >= rows) return;
		if (col < 0 || col >= columns) return;
		
		int b = col * rows * 6 + row * 6;
		
		Color[] colors = mesh.colors;
		for (int i = b; i < b + 6; i++)
			colors[i] =  color;
		
		mesh.colors = colors;
	}
}
