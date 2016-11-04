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

namespace Unitilities.Utils {

	public class Math {

		public static float Round(float number, float precision) {
			return Mathf.Round(number * 1f/precision) * precision;
		}

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

		/* Linear normalized ADSR: a,s,r are percent. a always goes from 0 to 1 and r down to 0 again.
		   Create an ASR envelope by setting df = 1f */
		public static float NormalizedLinearADSR(float a, float df, float s, float r, float factor) {
			float f = Mathf.Clamp01(factor);
			/* a: duration 0 -> 1 .. df: 1 -> df .. s: duration df -> df .. r: duration 0.5 -> 0 */
			float de = (1f-r-s);
			float d = (1f-r-s-a);
			float se = (1f-r);

			return f <  a ? f/a : 
				 ( f < de ? ( df + (d+a-f)/d * (1f-df) ) : 
				 ( f < se ? df : 
				 ( (1f-f)/r * df )));
		}
		public static float NormalizedLinearASR(float a, float s, float r, float factor) {
			return NormalizedLinearADSR(a,1f,s,r,factor);
		}

		public static Vector2i DirectionToVector(int i) {
			return new Vector2i( (i > 0 && i < 4) ? 1 : (i > 4) ? -1 : 0, 
								 (i > 2 && i < 6) ? 1 : (i == 2 || i == 6) ? 0 : -1 );
		}

		/* 
		static unsigned long x=123456789,y=362436069,z=521288629,w=88675123,v=886756453;
      	// replace defaults with five random seed values in calling program
		unsigned long XORShift() {
			unsigned long t;
 			t=(x^(x>>7)); x=y; y=z; z=w; w=v;
 			v=(v^(v<<6))^(t^(t<<13)); return (y+y+1)*v;}
		*/
	}
}

public static class Boundsxtension
{
	public static bool ContainBounds(this Bounds bounds, Bounds target)
	{
		return bounds.Contains(target.min) && bounds.Contains(target.max);
	}
	public static bool ContainBoundsXY(this Bounds bounds, Bounds target)
	{
		return target.min.x >= bounds.min.x && target.max.x < bounds.max.x
			&& target.min.y >= bounds.min.y && target.max.y < bounds.max.y;

	}
}
