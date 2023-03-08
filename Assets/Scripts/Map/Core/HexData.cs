using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectHex.Map.Tiles;
using Sirenix.OdinInspector;

namespace ProjectHex
{
    [System.Serializable, HideReferenceObjectPicker]
    public class HexData
    {
        public HexDB.TileType type;

        [HideInInspector] public bool isBuilt = false;
        [ShowIf("isBuilt")] public Vector3Int buildingReference;
        
        public TileBonusFinal finalBonus;



    }
}

