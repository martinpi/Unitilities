using UnityEngine;

public struct Mathi {

	public static int Min(int x, int y) { return x < y ? x : y; }
	public static int Max(int x, int y) { return x > y ? x : y; }
	public static int Abs(int x) { return x >= 0 ? x : -x; }
	public static int Clamp(int value, int min, int max) { return Max(Min(value,max), min); }
	public static int Lerp(int from, int to, float t) { return Mathf.FloorToInt((float)(to-from)*Mathf.Clamp01(t)+(float)from); }
}