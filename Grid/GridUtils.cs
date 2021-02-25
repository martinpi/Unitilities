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
using System.Collections;

namespace Unitilities.Grid {
	public static class GridUtils {
		public static float[,] average(float[,] grid, int radius, float strength) {
			float f = Mathf.Clamp01(strength);
			float p = 1f / (float)((2 * radius + 1) * (2 * radius + 1));
			float a = 0f;

			float[,] g = new float[grid.GetLength(0), grid.GetLength(1)];

			for (int x = 0; x < grid.GetLength(0); ++x) {
				for (int y = 0; y < grid.GetLength(1); ++y) {

					a = 0f;
					for (int xx = -radius; xx <= radius; ++xx) {
						for (int yy = -radius; yy <= radius; ++yy) {
							int px = Mathi.Clamp(xx + x, 0, grid.GetLength(0) - 1);
							int py = Mathi.Clamp(yy + y, 0, grid.GetLength(1) - 1);

							a += grid[px, py] * p;
						}
					}
					g[x, y] = (1.0f - f) * grid[x, y] + f * a;
				}
			}
			return g;
		}
	}

}

