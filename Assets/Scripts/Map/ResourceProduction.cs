using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectHex
{
    [System.Serializable, HideReferenceObjectPicker]
    public class ResourceProduction
    {
        [Range(0, 10)]
        public int baseAmount;

        [GUIColor(0.4f, 0.8f, 1)]
        [SuffixLabel("%")]
        public ElementEfficiency tileBonusEfficiency;

    }
}
