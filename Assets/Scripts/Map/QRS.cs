using Microsoft.Win32.SafeHandles;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace ProjectHex
{
    [System.Serializable, HideReferenceObjectPicker]
    public struct QRS

    {
        [HorizontalGroup("1", LabelWidth = 12)]
        public int q, r, s;

        public static QRS zero { get { return new QRS(0, 0, 0); } }
        public static QRS impossible { get { return new QRS(1, 1, 1); } }

        public QRS(int q, int r, int s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public static QRS operator +(QRS a, QRS b)
        {
            return new QRS(a.q + b.q,
                            a.r + b.r,
                            a.s + b.s);
        }
    }
}
