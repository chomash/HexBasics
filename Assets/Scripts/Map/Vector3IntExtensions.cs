using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectHex
{
    public static class Vector3IntExtensions
    {
        public static QRS toQRS(this Vector3Int v)
        {
            int q = v.x;
            int r = v.y;
            int s = -q - r;
            return new QRS(q, r, s);
        }
    }
}
