using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ProjectHex.Map.Tiles;
using UnityEngine.Tilemaps;
using ProjectHex.Map.Building;
using Unity.Collections.LowLevel.Unsafe;

/// <summary>
/// class to store all hex info
/// to be changd to some other save data, should be private but saveable!
/// </summary>
namespace ProjectHex
{
    [CreateAssetMenu(menuName = "Data/DB/Hex")]
    public class HexDB : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout), HideReferenceObjectPicker]
        //[ReadOnly]
        public Dictionary<Vector3Int, HexData> hexDataBase = new();



        #region
        public enum ResourceType { None, RawFood, Food, Junk, Energy, Tea }
        public enum TileType { Water, Beach, Plains, Forest, Mountains, Corrupted }
        public enum BuildingType { Main, Addition, Special, Undefined }

        public Dictionary<TileBase, TileType> tileTypeDictionary = new();

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
            hexDataBase.Add(coords, SetHexData(coords));
        }


        //corrupted have to be changed, need to be on another layer and work independed; spread/disappear etc
        private HexData SetHexData(Vector3Int coords)
        {
            HexData newHexData = new();
            newHexData.type = GetTileType(coords);
            newHexData.finalBonus = CalculateTileBonus(newHexData.type, coords);
            newHexData.buildingReference = SetBuildingRef(coords);
            newHexData.isBuilt = newHexData.buildingReference != new Vector3Int(666, 666, 666) ? true : false;
            if(newHexData.isBuilt)
                TryCreateBuilding(newHexData.buildingReference);

            return newHexData;
        }

        private void TryCreateBuilding(Vector3Int buildingKey)
        {
            string templateKey = MapManager.Instance.buildingTM.GetTile(buildingKey).name;
            MapManager.Instance._buildingDB.AddBuilding(buildingKey, templateKey);
        }

        private Vector3Int SetBuildingRef(Vector3Int coords)
        {
            if (MapManager.Instance.buildingTM.GetTile(coords) != null)
            {
                return coords;
            }
            else
            {
                return new Vector3Int(666, 666, 666);
            }
        }



        #region Tile functions
        private TileBonusFinal CalculateTileBonus(TileType type, Vector3Int coords)
        {
            TileBonusFinal newFinalBonus;
            newFinalBonus = GetTileBonusSelf(GetTileData(type).self) + GetTileBonusOthers(coords);


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

            foreach (Vector3Int pos in neighborCoords)
            {
                if (MapManager.Instance.tileTM.GetTile(pos) != null)
                {
                    
                    otherBonus += GetTileData(GetTileType(pos)).other;
                }
            }

            return ClampTileBonus(otherBonus, MapManager.Instance.tileBonusOtherMin, MapManager.Instance.tileBonusOtherMax);
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

        private TileType GetTileType(Vector3Int coords)
        {
            tileTypeDictionary.TryGetValue(MapManager.Instance.tileTM.GetTile(coords), out TileType type);
            return type;
        }

        private MyTileData GetTileData(TileType type)
        {
            MapManager.Instance.tileDB.tileDataBase.TryGetValue(type, out MyTileData tileData);
            return tileData;
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
        #endregion

    }
}
