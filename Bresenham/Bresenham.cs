using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unitilities {

    public class Gridtools {
        public delegate void CallbackXY(int x, int y);
        public delegate void CallbackXYValue(int x, int y, int value);
        public delegate bool CheckXY(int x, int y);

        // Bresenham
        public static void CallbackForLineFromTo(CallbackXY cb, Vector2Int from, Vector2Int to) {
            Vector2Int s = new Vector2Int(from.x < to.x ? 1 : -1, from.y < to.y ? 1 : -1);
            Vector2Int d = new Vector2Int(Mathi.Abs(to.x - from.x), Mathi.Abs(to.y - from.y));

            int err = (d.x > d.y ? d.x : -d.y) / 2, e2;
            for (; ; ) {
                cb(from.x, from.y);
                if (from.x == to.x && from.y == to.y) break;
                e2 = err;
                if (e2 > -d.x) { err -= d.y; from.x += s.x; }
                if (e2 < d.y) { err += d.x; from.y += s.y; }
            }
        }
		/** 
		Check if callback for all tiles between to and from is true.
		*/
		public static bool CheckForLineFromTo(CheckXY cb, Vector2Int from, Vector2Int to) {
			Vector2Int s = new Vector2Int(from.x < to.x ? 1 : -1, from.y < to.y ? 1 : -1);
			Vector2Int d = new Vector2Int(Mathi.Abs(to.x - from.x), Mathi.Abs(to.y - from.y));

			bool clear = true;
			int err = (d.x > d.y ? d.x : -d.y) / 2, e2;
			for (; ; ) {
				clear &= cb(from.x, from.y);
				if (from.x == to.x && from.y == to.y) break;
				e2 = err;
				if (e2 > -d.x) { err -= d.y; from.x += s.x; }
				if (e2 < d.y) { err += d.x; from.y += s.y; }
			}
			return clear;
		}

		// Dijkstra
		public static void CallbackForFillFrom(CallbackXYValue cb, Vector2Int from, CheckXY check, int distance) {
            if (!check(from.x, from.y)) return;
            if (distance < 0) return;
            cb(from.x, from.y, distance);
            CallbackForFillFrom(cb, from + Vector2Int.left, check, distance - 1);
            CallbackForFillFrom(cb, from + Vector2Int.right, check, distance - 1);
            CallbackForFillFrom(cb, from + Vector2Int.up, check, distance - 1);
            CallbackForFillFrom(cb, from + Vector2Int.down, check, distance - 1);
        }
    }
}

