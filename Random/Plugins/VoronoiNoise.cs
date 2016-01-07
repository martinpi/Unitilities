using UnityEngine;
using System.Collections;

public static class VoronoiNoise
{
	//Function delegates, makes using functions pointers easier
	private delegate float DISTANCE_FUNC2(Vector2 p1, Vector2 p2);
	private delegate float DISTANCE_FUNC3(Vector3 p1, Vector3 p2);
	private delegate float COMBINE_FUNC(float[] arr);
	//Function pointer to active distance function and combination function
	private static DISTANCE_FUNC2 DistanceFunc2 = EuclidianDistanceFunc2;
	private static DISTANCE_FUNC3 DistanceFunc3 = EuclidianDistanceFunc3;
	private static COMBINE_FUNC CombineFunc = CombineFunc_D0;
	//Set distance function
	public static void SetDistanceToEuclidian() { DistanceFunc2 = EuclidianDistanceFunc2; DistanceFunc3 = EuclidianDistanceFunc3; }
	public static void SetDistanceToManhattan() { DistanceFunc2 = ManhattanDistanceFunc2; DistanceFunc3 = ManhattanDistanceFunc3; }
	public static void SetDistanceToChebyshev() { DistanceFunc2 = ChebyshevDistanceFunc2; DistanceFunc3 = ChebyshevDistanceFunc3; }
	//Set combination function
	public static void SetCombinationTo_D0() { CombineFunc = CombineFunc_D0; }
	public static void SetCombinationTo_D1_D0() { CombineFunc = CombineFunc_D1_D0; }
	public static void SetCombinationTo_D2_D0() { CombineFunc = CombineFunc_D2_D0; }
	
	//Sample 2D fractal noise
	public static float FractalNoise2D(float x, float y, int octNum, float frq, float amp, int seed)
	{
		float gain = 1.0f;
		float sum = 0.0f;
	
		for(int i = 0; i < octNum; i++)
		{
			sum += Noise2D(new Vector2(x*gain/frq, y*gain/frq), seed) * amp/gain;
			gain *= 2.0f;
		}
		return sum;
	}
	
	//Sample 3D fractal noise
	public static float FractalNoise3D(float x, float y, float z, int octNum, float frq, float amp, int seed)
	{
		float gain = 1.0f;
		float sum = 0.0f;
	
		for(int i = 0; i < octNum; i++)
		{
			sum += Noise3D(new Vector3(x*gain/frq, y*gain/frq, z*gain/frq), seed) * amp/gain;
			gain *= 2.0f;
		}
		return sum;
	}
	
	//Sample single octave of 2D noise
	private static float Noise2D(Vector2 input, int seed)
	{
		//Declare some values for later use
		uint lastRandom, numberFeaturePoints;
		Vector2 randomDiff, featurePoint;
		int cubeX, cubeY;
		
		float[] distanceArray = new float[3];

		//Initialize values in distance array to large values
		for (int i = 0; i < distanceArray.Length; i++)
			distanceArray[i] = 6666;

		//1. Determine which cube the evaluation point is in
		int evalCubeX = (int)Mathf.Floor(input.x);
		int evalCubeY = (int)Mathf.Floor(input.y);

		for (int i = -1; i < 2; ++i)
		{
			for (int j = -1; j < 2; ++j)
			{
				cubeX = evalCubeX + i;
				cubeY = evalCubeY + j;

				//2. Generate a reproducible random number generator for the cube
				lastRandom = lcgRandom(hash((uint)(cubeX + seed), (uint)(cubeY)));
			
				//3. Determine how many feature points are in the cube
				numberFeaturePoints = probLookup(lastRandom);
				//4. Randomly place the feature points in the cube
				for (uint l = 0; l < numberFeaturePoints; ++l)
				{
					lastRandom = lcgRandom(lastRandom);
					randomDiff.x = (float)lastRandom / 0x100000000;

					lastRandom = lcgRandom(lastRandom);
					randomDiff.y = (float)lastRandom / 0x100000000;

					featurePoint = new Vector2(randomDiff.x + (float)cubeX, randomDiff.y + (float)cubeY);

					//5. Find the feature point closest to the evaluation point. 
					//This is done by inserting the distances to the feature points into a sorted list
					insert(distanceArray, DistanceFunc2(input, featurePoint));
				}
				//6. Check the neighboring cubes to ensure their are no closer evaluation points.
				// This is done by repeating steps 1 through 5 above for each neighboring cube
			}
		}

		return Mathf.Clamp01(CombineFunc(distanceArray));
	}
	
	//Sample single octave of 3D noise
	private static float Noise3D(Vector3 input, int seed)
	{
		//Declare some values for later use
		uint lastRandom, numberFeaturePoints;
		Vector3 randomDiff, featurePoint;
		int cubeX, cubeY, cubeZ;
		
		float[] distanceArray = new float[3];

		//Initialize values in distance array to large values
		for (int i = 0; i < distanceArray.Length; i++)
			distanceArray[i] = 6666;

		//1. Determine which cube the evaluation point is in
		int evalCubeX = (int)Mathf.Floor(input.x);
		int evalCubeY = (int)Mathf.Floor(input.y);
		int evalCubeZ = (int)Mathf.Floor(input.z);

		for (int i = -1; i < 2; ++i)
		{
			for (int j = -1; j < 2; ++j)
			{
				for (int k = -1; k < 2; ++k)
				{
					cubeX = evalCubeX + i;
					cubeY = evalCubeY + j;
					cubeZ = evalCubeZ + k;

					//2. Generate a reproducible random number generator for the cube
					lastRandom = lcgRandom(hash((uint)(cubeX + seed), (uint)(cubeY), (uint)(cubeZ)));
				
					//3. Determine how many feature points are in the cube
					numberFeaturePoints = probLookup(lastRandom);
					//4. Randomly place the feature points in the cube
					for (uint l = 0; l < numberFeaturePoints; ++l)
					{
						lastRandom = lcgRandom(lastRandom);
						randomDiff.x = (float)lastRandom / 0x100000000;

						lastRandom = lcgRandom(lastRandom);
						randomDiff.y = (float)lastRandom / 0x100000000;

						lastRandom = lcgRandom(lastRandom);
						randomDiff.z = (float)lastRandom / 0x100000000;

						featurePoint = new Vector3(randomDiff.x + (float)cubeX, randomDiff.y + (float)cubeY, randomDiff.z + (float)cubeZ);

						//5. Find the feature point closest to the evaluation point. 
						//This is done by inserting the distances to the feature points into a sorted list
						insert(distanceArray, DistanceFunc3(input, featurePoint));
					}
					//6. Check the neighboring cubes to ensure their are no closer evaluation points.
					// This is done by repeating steps 1 through 5 above for each neighboring cube
				}
			}
		}

		return Mathf.Clamp01(CombineFunc(distanceArray));
	}
	
	//2D distance functions
	private static float EuclidianDistanceFunc2(Vector2 p1, Vector2 p2)
	{
		return (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
	}
	
	private static float ManhattanDistanceFunc2(Vector2 p1, Vector2 p2)
	{
		return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y);
	}

	private static float ChebyshevDistanceFunc2(Vector2 p1, Vector2 p2)
	{
		Vector2 diff = p1 - p2;
		return Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
	}
	//3D distance functions
	private static float EuclidianDistanceFunc3(Vector3 p1, Vector3 p2)
	{
		return (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y) + (p1.z - p2.z) * (p1.z - p2.z);
	}
	
	private static float ManhattanDistanceFunc3(Vector3 p1, Vector3 p2)
	{
		return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y) + Mathf.Abs(p1.z - p2.z);
	}

	private static float ChebyshevDistanceFunc3(Vector3 p1, Vector3 p2)
	{
		Vector3 diff = p1 - p2;
		return Mathf.Max(Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y)), Mathf.Abs(diff.z));
	}
	
	//Combination functions
	private static float CombineFunc_D0(float[] arr) { return arr[0]; }
	private static float CombineFunc_D1_D0(float[] arr) { return arr[1]-arr[0]; }
	private static float CombineFunc_D2_D0(float[] arr) { return arr[2]-arr[0]; }
	
	/// <summary>
	/// Given a uniformly distributed random number this function returns the number of feature points in a given cube.
	/// </summary>
	/// <param name="value">a uniformly distributed random number</param>
	/// <returns>The number of feature points in a cube.</returns>
	// Generated using mathmatica with "AccountingForm[N[Table[CDF[PoissonDistribution[4], i], {i, 1, 9}], 20]*2^32]"
	private static uint probLookup(uint value)
	{
		if (value < 393325350) return 1;
		if (value < 1022645910) return 2;
		if (value < 1861739990) return 3;
		if (value < 2700834071) return 4;
		if (value < 3372109335) return 5;
		if (value < 3819626178) return 6;
		if (value < 4075350088) return 7;
		if (value < 4203212043) return 8; 
		return 9;
	}
	/// <summary>
	/// Inserts value into array using insertion sort. If the value is greater than the largest value in the array
	/// it will not be added to the array.
	/// </summary>
	/// <param name="arr">The array to insert the value into.</param>
	/// <param name="value">The value to insert into the array.</param>
	private static void insert(float[] arr, float value)
	{
		float temp;
		for (int i = arr.Length - 1; i >= 0; i--)
		{
			if (value > arr[i]) break;
			temp = arr[i];
			arr[i] = value;
			if (i + 1 < arr.Length) arr[i + 1] = temp;
		}
	}
	/// <summary>
	/// LCG Random Number Generator.
	/// LCG: http://en.wikipedia.org/wiki/Linear_congruential_generator
	/// </summary>
	/// <param name="lastValue">The last value calculated by the lcg or a seed</param>
	/// <returns>A new random number</returns>
	private static uint lcgRandom(uint lastValue)
	{
		return (uint)((1103515245u * lastValue + 12345u) % 0x100000000u);
	}
	/// <summary>
	/// Constant used in FNV hash function.
	/// FNV hash: http://isthe.com/chongo/tech/comp/fnv/#FNV-source
	/// </summary>
	private const uint OFFSET_BASIS = 2166136261;
	/// <summary>
	/// Constant used in FNV hash function
	/// FNV hash: http://isthe.com/chongo/tech/comp/fnv/#FNV-source
	/// </summary>
	private const uint FNV_PRIME = 16777619;
	/// <summary>
	/// Hashes three integers into a single integer using FNV hash.
	/// FNV hash: http://isthe.com/chongo/tech/comp/fnv/#FNV-source
	/// </summary>
	/// <returns>hash value</returns>
	private static uint hash(uint i, uint j, uint k)
	{
		return (uint)((((((OFFSET_BASIS ^ (uint)i) * FNV_PRIME) ^ (uint)j) * FNV_PRIME) ^ (uint)k) * FNV_PRIME);
	}
	
	private static uint hash(uint i, uint j)
	{
		return (uint)((((OFFSET_BASIS ^ (uint)i) * FNV_PRIME) ^ (uint)j) * FNV_PRIME);
	}


}
