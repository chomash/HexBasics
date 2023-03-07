using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace ProjectHex.Map.Tiles
{
    [System.Serializable]
    public struct TileData
    {
        [Required]
        public HexDB.TileType type;
        [Required]
        public TileBonusSelf self;
        [CanBeNull]
        public TileBonusOther other;
    }
}
