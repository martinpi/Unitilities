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
 * Material supplied has to support vertices and indizes. Best works with unlit vertex colored shaders.
 */

using UnityEngine;

public class QuadGrid : MonoBehaviour {
	
	public int columns      = 10; 
	public int rows         = 10;
	public float cellRadius = 0.5f;
	public Material material;
	
	private Color[] colors;
	private Mesh mesh;
	
	void Start () {
		MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();
		if (rend == null)
			rend = gameObject.AddComponent<MeshRenderer>();
		rend.material = material;
		
		CreateQuadMesh();
		Color color = Color.white;
		colors = new Color[columns * rows * 4];
		for (int i = 0; i < colors.Length; i+=4) {
			for (int j = 0; j < 4; j++) {
				colors[i+j] = color;
			}
		}
		mesh.colors = colors;

//		Random.seed = 0;
//		float[,] grid = new float[columns, rows];
//		for (int i = 0; i < grid.GetLength(0); ++i)
//			for (int j = 0; j < grid.GetLength(1); ++j)
//				grid[i,j] = Random.value;
//		grid = GridUtils.average(grid, 1, 0.1f);
//		this.SetColorFromGrid(grid, Color.cyan);
	}	
	
	void CreateQuadMesh() {
		MeshFilter mf = gameObject.GetComponent<MeshFilter>();
		if (mf == null)
			mf = gameObject.AddComponent<MeshFilter>();
		mesh = new Mesh();
		mf.mesh = mesh;
		
		float vertSpacing = 2.0f * cellRadius;
		float horzSpacing = 2.0f * cellRadius;
		
		Vector3 currPos = new Vector3(-columns / 2.0f * horzSpacing, 0.0f, rows / 2.0f * vertSpacing); 
		
		Vector3[] quadVerts = new Vector3[4];
		quadVerts[0] = Vector3.zero;
		quadVerts[1] = Vector3.forward * horzSpacing;
		quadVerts[2] = Vector3.right * vertSpacing + Vector3.forward * horzSpacing;
		quadVerts[3] = Vector3.right * vertSpacing;

		Vector3[] vertices = new Vector3[rows * columns * 4];
		int currVert = 0;
		for (int i = 0; i < columns; i++) {
			for (int j = 0; j < rows; j++) {
				for (int k = 0; k < quadVerts.Length; k++) {
					vertices[currVert++] = quadVerts[k] + currPos;
				}
				currPos.z -= vertSpacing;
			}
			currPos.x += horzSpacing;
			currPos.z = rows / 2.0f * vertSpacing;
		}
		
		//Create the triangles
		int curr = 0;
		int[] triangles = new int[rows * columns * 4 * 3];
		for (int i = 0; i < columns * rows * 4; i += 4) {
			triangles[curr++] = i+0;
			triangles[curr++] = i+1;
			triangles[curr++] = i+2;
			
			triangles[curr++] = i+0;
			triangles[curr++] = i+2;
			triangles[curr++] = i+3;
		}
		
		mf.mesh.vertices = vertices;
		mf.mesh.triangles = triangles;
		mf.mesh.colors = colors;
		mf.mesh.RecalculateBounds();
		mf.mesh.RecalculateNormals();
	}
	
	
	void SetColor(int row, int col, Color color) {
		if (row < 0 || row >= rows) return;
		if (col < 0 || col >= columns) return;
		
		int b = col * rows * 4 + row * 4;
		
		Color[] colors = mesh.colors;
		for (int i = b; i < b + 4; i++)
			colors[i] =  color;
		
		mesh.colors = colors;
	}

	void SetColorFromGrid(float[,] grid, Color mask) {
		if (grid.GetLength(0) != columns || grid.GetLength(1) != rows)
			return;

		colors = new Color[columns * rows * 4];
		for (int y = 0; y < rows; ++y) {
			for (int x = 0; x < columns; ++x) {
				Color color = new Color(mask.r * grid[x,y], mask.g * grid[x,y], mask.b * grid[x,y], 1f);
				for (int j = 0; j < 4; j++) {
					colors[(x+y*columns)*4 + j] = color;
				}
			}
		}
		mesh.colors = colors;
	}
}
