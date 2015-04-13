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

namespace Utils {

	public class Math {

		public static double Clamp(double x, double bottom, double top) {
			return x < bottom ? bottom : ( x > top ? top : x );
		}
		public static double Max(double x, double y) { return (x > y) ? x : y; }
		public static double Min(double x, double y) { return (x < y) ? x : y; }

		public static Vector3 TrajectoryAtTime (Vector3 start, Vector3 startVelocity, float time) {
		    return start + startVelocity*time + Physics.gravity*time*time*0.5f;
		}

		public static float AngleBetween(Vector2 fromVector2, Vector2 toVector2) {
			float ang0 = Mathf.Atan2(fromVector2.y, fromVector2.x);
			float ang1 = Mathf.Atan2(toVector2.y, toVector2.x);
			return Mathf.Atan2(Mathf.Sin(ang0-ang1), Mathf.Cos(ang0-ang1));
		}
		
		public static float AngleBetween(float fromAngle, float toAngle) {
			return UnwrapRadian(toAngle - fromAngle);
		}
		
		public static float UnwrapRadian(float r) {
			r = r % Mathf.PI*2.0f;
			if (r > Mathf.PI)
				r -= Mathf.PI*2.0f;
			else if (r < -Mathf.PI)
				r += Mathf.PI*2.0f;
			return r;
		}
		
		public static float DegToRad(float degrees) {
			return degrees * (Mathf.PI / 180.0f);
		}

		public static float RadToDeg(float rad) {
			return rad / (Mathf.PI / 180.0f);
		}

		public static float IncrementToward(float n, float target, float a) {
			if (Mathf.Abs(n-target) < Mathf.Epsilon)
				return n;
			else {
				float dir = Mathf.Sign (target - n);
				n += a * dir;
				return (dir == Mathf.Sign(target-n)) ? n : target;
			}
		}

		public static Vector2 IncrementToward(Vector2 n, Vector2 target, float a) {
			float dist = (target-n).magnitude;
			if (dist < Mathf.Epsilon) return target;
			return n + Vector2.ClampMagnitude(target-n, Mathf.Min(dist, a));
		}

		public static Vector3 IncrementToward(Vector3 n, Vector3 target, float a) {
			float dist = (target-n).magnitude;
			if (dist < Mathf.Epsilon) return target;
			return n + Vector3.ClampMagnitude(target-n, Mathf.Min(dist, a));
		}

		public static float Mix(float x, float y, float a)
		{
			return (x + a * (y-x));
		}

		public static Vector3 BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float u = 1.0f - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * t;
			Vector3 p = uuu * p0; //first term
			p += 3 * uu * t * p1; //second term
			p += 3 * u * tt * p2; //third term
			p += ttt * p3; //fourth term
			return p;
		}
		public static Vector2 BezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float u = 1.0f - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * t;
			Vector2 p = uuu * p0; //first term
			p += 3 * uu * t * p1; //second term
			p += 3 * u * tt * p2; //third term
			p += ttt * p3; //fourth term
			return p;
		}

	}
}
