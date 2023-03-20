using Microsoft.Win32.SafeHandles;
using ProjectHex.Map.Tiles;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace ProjectHex
{
    [System.Serializable, HideReferenceObjectPicker]
    public struct Hex

    {
        [HorizontalGroup("1", LabelWidth = 12)]
        public int q, r, s;

        public Hex(int q, int r, int s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public static Hex operator +(Hex a, Hex b)
        {
            return new Hex( a.q + b.q,
                            a.r + b.r,
                            a.s + b.s);
        }
    }
}
