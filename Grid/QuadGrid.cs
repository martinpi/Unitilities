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

[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class QuadGrid : MonoBehaviour {
	
	public int columns      = 10; 
	public int rows         = 10;
	public float cellRadius = 0.5f;

	private Mesh mesh;
	private Vector3[] vertices;

	void Awake () {
		CreateGrid(rows, columns);
	}	

	public void CreateGrid(int rows, int columns) {
		this.columns = columns;
		this.rows = rows;
		CreateQuadMesh();
	}

	void CreateQuadMesh() {
		GetComponent<MeshFilter>().sharedMesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";

		vertices = new Vector3[(columns + 1) * (rows + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
		for (int i = 0, y = 0; y <= rows; y++) {
			for (int x = 0; x <= columns; x++, i++) {
				vertices[i] = new Vector3(x, y);
				uv[i] = new Vector2((float)x / columns, (float)y / rows);
				tangents[i] = tangent;
			}
		}

		Color colorT = Color.red;
		Color[] colors = new Color[vertices.Length];
		for (int i = 0; i < colors.Length; i++) {
			if ((i % 3) == 0)
				colorT = new Color(Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), 1.0f);
			colors[i] = colorT;
		}

		int[] triangles = new int[columns * rows * 6];
		for (int ti = 0, vi = 0, y = 0; y < rows; y++, vi++) {
			for (int x = 0; x < columns; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + columns + 1;
				triangles[ti + 5] = vi + columns + 2;
			}
		}
		mesh.vertices = vertices;
		mesh.colors = colors;
		mesh.uv = uv;
		mesh.tangents = tangents;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

	}
	
	
	public void SetColor(int row, int col, Color color) {
		if (row < 0 || row >= rows) return;
		if (col < 0 || col >= columns) return;
		
		int b = col * rows * 4 + row * 4;
		
		Color[] colors = mesh.colors;
		for (int i = b; i < b + 4; i++)
			colors[i] =  color;
		
		mesh.colors = colors;
	}

	public void SetBrightnessFromGrid(float[,] grid, Color mask) {
		if (grid.GetLength(0) != columns || grid.GetLength(1) != rows)
			return;

		Color[] colors = new Color[columns * rows * 4];
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

	int GetMax(int[,] grid) {
		if (grid.GetLength(0) != columns || grid.GetLength(1) != rows)
			return -1;
		
		int max = 0;
		for (int y = 0; y < rows; ++y) 
			for (int x = 0; x < columns; ++x) 
				max = Mathi.Max(grid[x,y], max);
			
		return max;
	}

	public void SetBrightnessFromGrid(int[,] grid, Color mask, int max = -1) {
		if (grid.GetLength(0) != columns || grid.GetLength(1) != rows)
			return;

		if (max <= 0) max = GetMax(grid);

		float fmax = (float)max;
		Color[] colors = new Color[columns * rows * 4];
		for (int y = 0; y < rows; ++y) {
			for (int x = 0; x < columns; ++x) {
				float fgrid = ((float)grid[x,y]) / fmax;
				Color color = new Color(mask.r * fgrid, mask.g * fgrid, mask.b * fgrid, 1f);
				for (int j = 0; j < 4; j++) {
					colors[(x+y*columns)*4 + j] = color;
				}
			}
		}
		mesh.colors = colors;
	}
}
