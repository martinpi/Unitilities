using UnityEngine;
using System.Collections;

namespace Unitilities.R {
	public class MarkovChain {

		public static float[,] GetChain(int dim) {
			float[,] chain = new float[dim,dim];
			for (int x=0; x<dim; ++x) {
				float row_sum = 0f;
				for (int y=0; y<dim; ++y) {
					float row_val = UnityEngine.Random.value;
					chain[x,y] = row_val;
					row_sum += row_val;
				}

				for (int y=0; y<dim; ++y)
					chain[x,y] /= row_sum;
			}
			return chain;
		}

		public static int Next(float[,] chain, int column) {
			float val = UnityEngine.Random.value;
			float running_sum = chain[column, 0];
			int i=0;

			while (val >= running_sum) 
				running_sum += chain[column, ++i];
			
			return i;
		}

	}

}

