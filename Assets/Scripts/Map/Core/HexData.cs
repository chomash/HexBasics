using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectHex.Map.Tiles;

namespace ProjectHex
{
    [System.Serializable]
    public class HexData
    {
        public TileBonusFinal finalBonus;
        public HexDB.TileType type;
        public Vector3Int buildingKey;
    }
}

