using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectHex
{
    public static class ExtensionMethods
    {
       
        public static Vector3Int CubeToOffset(Hex hex)
        {
            int x = hex.q + (hex.r + (hex.r&1)) / 2; // &1 is BITWISE AND, used to detect evens and odds
            int y = hex.r;
            return new Vector3Int(x, y, 0);
        }

        public static Hex OffsetToCube(Vector3Int coords)
        {
            int q = coords.x - (coords.y + (coords.y&1)) / 2; // &1 is BITWISE AND, used to detect evens and odds
            int r = coords.y;
            return new Hex(q, r, -q-r);
        }

    }
}
