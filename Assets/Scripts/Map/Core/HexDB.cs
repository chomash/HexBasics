using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ProjectHex.Map.Tiles;

/// <summary>
/// class to store all hex info
/// to be changd to some other save data, should be private but saveable!
/// </summary>
namespace ProjectHex
{
    [CreateAssetMenu(menuName = "Data/DB/Hex")]
    public class HexDB : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        [ReadOnly]
        public Dictionary<Vector2Int, HexData> hexDataBase = new();



        #region
        public enum ResourceType { None, RawFood, Food, Junk, Energy, Tea }
        public enum TileType { Water, Beach, Plains, Forest, Mountains, Corrupted }
        public enum BuildingType { Main, Addition, Special, Undefined }

        public Vector2Int[] evenRowNeighborOffset { get; private set; } = new Vector2Int[] {
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),

        };
        public Vector2Int[] oddRowNeighborOffset { get; private set; } = new Vector2Int[]{
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
        };

        #endregion

        public void AddNewHex(Vector3Int coords)
        {
            hexDataBase.Add(new Vector2Int(coords.x, coords.y), SetHexData(coords));
        }

        private HexData SetHexData(Vector3Int coords)
        {
            HexData newHexData = new();

            newHexData.finalBonus = CalculateTileBonus(coords);
            newHexData.type = GetTileDataAt(coords).type;
            newHexData.buildingKey = SetBuildingRef(coords);


            //TryCreateBuilding(newHexData.buildingKey, newHexData.finalBonus);
            

            return newHexData;
        }

        private void TryCreateBuilding(Vector3Int buildingKey, TileBonusFinal tileBonus)
        {
            if(buildingKey != new Vector3Int(2137, 2137, 2137))
            {
                string buildingID = MapManager.Instance.buildingTM.GetTile(buildingKey).name;
                MapManager.Instance._buildingDB.AddBuilding(buildingKey, buildingID, tileBonus);
            }
            
        }

        private Vector3Int SetBuildingRef(Vector3Int coords)
        {
            if (MapManager.Instance.buildingTM.GetTile(coords) != null)
            {
                return coords;
            }
            else
            {
                return new Vector3Int(2137, 2137, 2137);
            }
        }


        //corrupted have to be changed, need to be on another layer and work independed; spread/disappear etc

        #region Tile functions
        private TileBonusFinal CalculateTileBonus(Vector3Int coords)
        {
            TileBonusFinal newFinalBonus;
            newFinalBonus = GetTileBonusSelf(GetTileDataAt(coords).self) + GetTileBonusOthers(coords);


            return ClampTileBonus(newFinalBonus, MapManager.Instance.tileBonusFinalMin, MapManager.Instance.tileBonusFinalMax);
        }

        private TileBonusFinal GetTileBonusSelf(TileBonusSelf tileRef)
        {
            TileBonusFinal tempTileBonus = new();    //used in randomize SelfBonus

            tempTileBonus.nature = Random.Range(tileRef.nature.x, tileRef.nature.y + 1);
            tempTileBonus.sun = Random.Range(tileRef.sun.x, tileRef.sun.y + 1);
            tempTileBonus.water = Random.Range(tileRef.water.x, tileRef.water.y + 1);
            tempTileBonus.punk = Random.Range(tileRef.punk.x, tileRef.punk.y + 1);
            tempTileBonus.corruption = Random.Range(tileRef.corruption.x, tileRef.corruption.y + 1);

            return tempTileBonus;
        }

        private TileBonusFinal GetTileBonusOthers(Vector3Int coords)
        {
            TileBonusFinal otherBonus = new();
            List<Vector3Int> neighborCoords = GetNeighborsCoords(coords);

            foreach (Vector3Int v3 in neighborCoords)
            {
                if (MapManager.Instance.tileTM.GetTile(v3) != null)
                {
                    otherBonus += GetTileDataAt(v3).other;
                }
            }

            return ClampTileBonus(otherBonus, MapManager.Instance.tileBonusOtherMin, MapManager.Instance.tileBonusOtherMax);
        }

        private List<Vector3Int> GetNeighborsCoords(Vector3Int coords)
        {
            List<Vector3Int> neighborCoords = new();
            if (coords.y % 2 == 0)
            {
                foreach (Vector2Int offset in evenRowNeighborOffset)
                {
                    neighborCoords.Add(new Vector3Int(coords.x + offset.x, coords.y + offset.y, 0));
                }
            }
            else
            {
                foreach (Vector2Int offset in oddRowNeighborOffset)
                {
                    neighborCoords.Add(new Vector3Int(coords.x + offset.x, coords.y + offset.y, 0));
                }
            }
            return neighborCoords;
        }

        public TileBonusFinal ClampTileBonus(TileBonusFinal t, int min, int max)
        {
            TileBonusFinal other = new();

            other.nature = Mathf.Clamp(t.nature, min, max);
            other.sun = Mathf.Clamp(t.sun, min, max);
            other.water = Mathf.Clamp(t.water, min, max);
            other.punk = Mathf.Clamp(t.punk, min, max);
            other.corruption = Mathf.Clamp(t.corruption, min, max);

            return other;
        }

        private TileData GetTileDataAt(Vector3Int coords)
        {
            MapManager.Instance.tileDB.tileDataBase.TryGetValue(MapManager.Instance.tileTM.GetTile(coords).name, out TileData tileData);
            return tileData;
        }
        #endregion

    }
}
