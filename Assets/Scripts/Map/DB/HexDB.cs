using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.ShaderGraph.Internal;

/// <summary>
/// class to store all hex info
/// to be changd to some other save data, should be private but saveable!
/// </summary>
namespace ProjectHex
{
    [CreateAssetMenu(menuName = "Data/DB/Hex")]
    public class HexDB : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "Cube Coordinates", ValueLabel ="Hex Data")]
        [HideReferenceObjectPicker]
        [ReadOnly]
        public Dictionary<QRS, HexData> hexDataBase = new();



        #region
        public enum ResourceType { None, RawFood, Food, Junk, Energy, Tea }
        public enum TileType { Water1, Water2, Beach, Plain, Field, Forest, Mountain, Swamp }
        public enum BuildingType { Main, Addition, Special, Undefined }

        public Dictionary<TileBase, TileType> tileTypeDictionary = new();
        public GameObject tilePrefab;

        #endregion

        public void AddNewHex(QRS coords)
        {
            
            hexDataBase.Add(coords, SetHexData(coords));
            SpawnHex(coords);
        }


        //corrupted have to be changed, need to be on another layer and work independed; spread/disappear etc
        private HexData SetHexData(QRS coords)
        {
            HexData newHexData = new();
            newHexData.type = GetTileType(coords);
            newHexData.finalBonus = CalculateTileBonus(newHexData.type, coords);
            //newHexData.buildingReference = SetBuildingRef(coords);
            //newHexData.isBuilt = newHexData.buildingReference != new Vector3Int(666, 666, 666) ? true : false;
            //if (newHexData.isBuilt)
            //    TryCreateBuilding(newHexData.buildingReference);

            return newHexData;
        }

        private void TryCreateBuilding(Vector3Int buildingKey)
        {
            string templateKey = MapManager.Instance.buildingTM.GetTile(buildingKey).name;
            MapManager.Instance._buildingDB.AddBuilding(buildingKey, templateKey);
        }

        private Vector3Int SetBuildingRef(QRS coords)
        {
            if (MapManager.Instance.buildingTM.GetTile(coords.ToOffset()) != null)
            {
                return coords.ToOffset();
            }
            else
            {
                return new Vector3Int(666, 666, 666);
            }
        }

        private void SpawnHex(QRS coords)
        {

            GameObject hex = Instantiate(tilePrefab, QRSToWorld(coords), Quaternion.identity);
            hex.transform.SetParent(MapManager.Instance.hexContainer.transform);
            
            MyTileData temp = MapManager.Instance.tileDB.tileDataBase[hexDataBase[coords].type];
            TileGO hexRef = hex.GetComponent<TileGO>();
            
            hexRef.SetOffset(temp.offset*MapManager.Instance.offsetMulti);
            hexRef.SetBaseTile(temp.tileSprite);
            hexRef.SetCoords(coords);
            hexDataBase[coords].gameObjectReference = hex;

        }

        #region Tile functions
        private TileBonusFinal CalculateTileBonus(TileType type, QRS coords)
        {
            TileBonusFinal newFinalBonus;
            newFinalBonus = GetTileBonusSelf(GetTileData(type).self) + GetTileBonusOthers(ExtensionMethods.ToCube(coords.ToOffset()));


            return ClampTileBonus(newFinalBonus, MapManager.Instance.tileBonusFinalMin, MapManager.Instance.tileBonusFinalMax);
        }

        private TileBonusFinal GetTileBonusSelf(TileBonusSelf tileRef)
        {
            TileBonusFinal tempTileBonus = new();    //used in randomize SelfBonus

            tempTileBonus.nature = Random.Range(tileRef.nature.x, tileRef.nature.y + 1);
            tempTileBonus.sun = Random.Range(tileRef.sun.x, tileRef.sun.y + 1);
            tempTileBonus.water = Random.Range(tileRef.water.x, tileRef.water.y + 1);
            tempTileBonus.punk = Random.Range(tileRef.punk.x, tileRef.punk.y + 1);

            return tempTileBonus;
        }

        private TileBonusFinal GetTileBonusOthers(QRS hex)
        {
            TileBonusFinal otherBonus = new();

            foreach (QRS n in hex.GetNeighbors())
            {
                if (MapManager.Instance.tileTM.GetTile(n.ToOffset()) != null)
                {
                    
                    otherBonus += GetTileData(GetTileType(n)).other;
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

            return other;
        }

        private TileType GetTileType(QRS coords)
        {
            tileTypeDictionary.TryGetValue(MapManager.Instance.tileTM.GetTile(coords.ToOffset()), out TileType type);
            return type;
        }

        private MyTileData GetTileData(TileType type)
        {
            MapManager.Instance.tileDB.tileDataBase.TryGetValue(type, out MyTileData tileData);
            return tileData;
        }


        private Vector3 QRSToWorld(QRS coords)
        {
            Vector3 v3 = new Vector3();
            v3.x = coords.q + 0.5f * coords.r;
            v3.y = 0.75f * coords.r;
            v3.z = -100+coords.ToOffset().y * 0.1f; //raycast priority
            return v3;

        }
        #endregion

    }
}
