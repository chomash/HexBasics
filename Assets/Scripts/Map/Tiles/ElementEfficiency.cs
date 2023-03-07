using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace ProjectHex.Map.Tiles
{
    [System.Serializable]
    public struct ElementEfficiency
    {
        /// <summary>
        /// used in buildings template to set efficiency of tile bonus
        /// </summary>

        [Range(0f, 1.5f)]
        public float nature, sun, water, punk, corruption;



    }
}
