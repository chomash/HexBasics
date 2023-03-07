using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectHex.Map.Tiles
{

    [System.Serializable]
    public struct TileBonusFinal
    {
        /// <summary>
        /// used as sum of other tile bonuses, this struct dont have a min/max range
        /// </summary>

        public int nature, sun, water, punk, corruption;

        public static TileBonusFinal operator +(TileBonusFinal a, TileBonusFinal b)
        {
            TileBonusFinal tileBonus = new();
            tileBonus.nature = a.nature + b.nature;
            tileBonus.sun = a.sun + b.sun;
            tileBonus.water = a.water + b.water;
            tileBonus.punk = a.punk + b.punk;
            tileBonus.corruption = a.corruption + b.corruption;

            return tileBonus;
        }
        public static TileBonusFinal operator +(TileBonusOther a, TileBonusFinal b)
        {
            TileBonusFinal tileBonus = new();
            tileBonus.nature = a.nature + b.nature;
            tileBonus.sun = a.sun + b.sun;
            tileBonus.water = a.water + b.water;
            tileBonus.punk = a.punk + b.punk;
            tileBonus.corruption = a.corruption + b.corruption;

            return tileBonus;
        }
        public static TileBonusFinal operator +(TileBonusFinal a, TileBonusOther b)
        {
            TileBonusFinal tileBonus = new();
            tileBonus.nature = a.nature + b.nature;
            tileBonus.sun = a.sun + b.sun;
            tileBonus.water = a.water + b.water;
            tileBonus.punk = a.punk + b.punk;
            tileBonus.corruption = a.corruption + b.corruption;

            return tileBonus;
        }
        public void Clear()
        {
            nature = 0;
            sun = 0;
            water = 0;
            punk = 0;
            corruption = 0;
        }
    }

    [System.Serializable]
    public struct TileBonusOther
    {
        [Range(-2, 4)]
        public int nature, sun, water, punk, corruption;
    }

    [System.Serializable]
    public struct TileBonusSelf
    {
        [MinMaxSlider(0, 6, true)]
        public Vector2Int nature, sun, water, punk, corruption;
    }
}
