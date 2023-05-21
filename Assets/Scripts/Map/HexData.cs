using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ProjectHex
{
    [System.Serializable, HideReferenceObjectPicker]
    public class HexData
    {
        //tile data
        public HexDB.TileType type;
        public TileBonusFinal finalBonus;
        //building data
        [HideInInspector] public bool isBuilt = false;
        [ShowIf("isBuilt")] public Vector3Int buildingReference;
        public GameObject gameObjectReference;
        
        [Range(-2,6)]
        public int corruption;










    }
}

