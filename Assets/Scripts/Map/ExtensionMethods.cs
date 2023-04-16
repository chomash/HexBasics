using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectHex
{
    public static class ExtensionMethods
    {

        public static Vector3Int ToOffset(this QRS hex)
        {
            int x = hex.q + (hex.r - (hex.r & 1)) / 2; // &1 is BITWISE AND, used to detect evens and odds
            int y = hex.r;
            return new Vector3Int(x, y, 0);
        }

        public static QRS ToCube(this Vector3Int coords)
        {
            int q = coords.x - (coords.y - (coords.y & 1)) / 2; // &1 is BITWISE AND, used to detect evens and odds
            int r = coords.y;
            return new QRS(q, r, -q - r);
        }

        public static List<QRS> GetNeighbors(this QRS hex)
        {
            List<QRS> neighbour = new List<QRS>();
            QRS[] offset = new QRS[] {
                new QRS(1, -1, 0),
                new QRS(1, 0, -1),
                new QRS(0, 1, -1),
                new QRS(-1, 1, 0),
                new QRS(-1, 0, 1),
                new QRS(0, -1, 1)
            };

            foreach (var n in offset)
            {
                neighbour.Add(hex + n);
            }
            return neighbour;
        }
    }
}
