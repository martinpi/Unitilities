using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bresenham
{
    public delegate void Callback(int x, int y);

    public static void CallbackForLineFromTo(Callback cb, Vector2Int from, Vector2Int to) {
        Vector2Int s = new Vector2Int(from.x < to.x ? 1 : -1, from.y < to.y ? 1 : -1);
        Vector2Int d = new Vector2Int(Mathi.Abs(to.x-from.x), Mathi.Abs(to.y-from.y));

        // int dx = Mathi.Abs(to.x - from.x), s.x = from.x < to.x ? 1 : -1;
        // int dy = Mathi.Abs(to.y - from.y), s.y = from.y < to.y ? 1 : -1;
        int err = (d.x > d.y ? d.x : -d.y) / 2, e2;
        for(;;) {
            cb(from.x, from.y);
            if (from.x == to.x && from.y == to.y) break;
            e2 = err;
            if (e2 > -d.x) { err -= d.y; from.x += s.x; }
            if (e2 < d.y) { err += d.x; from.y += s.y; }
        }

    }

}
