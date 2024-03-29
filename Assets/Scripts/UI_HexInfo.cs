using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using System.Linq.Expressions;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

//using UnityEngine.UIElements;
namespace ProjectHex
{
    public class UI_HexInfo : MonoBehaviour
    {

        [SerializeField] private TMP_Text hexName;
        [FoldoutGroup("Tiles", false)]
        [SerializeField] Image baseTileSprite, corruptionTileSprite, buildingSprite;
        [FoldoutGroup("Tiles", false)]
        [SerializeField] private TMP_Text natureValue, sunValue, waterWalue, punkValue, corruptionValue;
        [FoldoutGroup("Buildings", false)]
        [SerializeField] GameObject buildingInfo;
        [FoldoutGroup("Buildings", false)]
        [SerializeField] private List<GameObject> resourcesProduction = new();

        public void UpdateHexInfo(HexData hexData, Vector3Int position)
        {
            TileGO hexRef = hexData.gameObjectReference.GetComponent<TileGO>();
            if (hexData.buildingReference != new Vector3Int(666, 666, 666))
            {
                DeactivateAllResources();
                buildingInfo.SetActive(true);
                //UpdateBuildingInfo(GetBuildingReference(position));
                //buildingImage.sprite = MapManager.Instance.buildingTM.GetSprite(position);
            }
            else
            {
                buildingInfo.SetActive(false);
                hexName.text = "Empty";

            }

            UpdateTileSprite(hexRef);
            UpdateTileText(hexData);

        }

        private void DeactivateAllResources()
        {
            foreach (GameObject obj in resourcesProduction)
            {
                obj.SetActive(false);
            }
        }

        private void UpdateTileSprite(TileGO hexRef)
        {
            baseTileSprite.sprite = hexRef.baseRef.sprite;
            corruptionTileSprite.sprite = hexRef.corruptionRef.sprite;
            //buildingSprite.sprite = hexRef.buildingRef.sprite;

            if(corruptionTileSprite.sprite == null)
            {
                corruptionTileSprite.enabled = false;
            }
            else
            {
                corruptionTileSprite.enabled = true;
            }

            if (buildingSprite.sprite == null)
            {
                buildingSprite.enabled = false;
            }
            else
            {
                buildingSprite.enabled = true;
            }
        }

        private void UpdateTileText(HexData hexData)
        {
            natureValue.text = hexData.finalBonus.nature.ToString();
            sunValue.text = hexData.finalBonus.sun.ToString();
            punkValue.text = hexData.finalBonus.punk.ToString();
            waterWalue.text = hexData.finalBonus.water.ToString();
            corruptionValue.text = hexData.finalBonus.corruption.ToString();
        }
        
        //private FinalBuilding GetBuildingReference(Vector3Int key)
        //{
        //    MapManager.Instance._buildingDB.buildingDataBase.TryGetValue(key, out var buildingInfo);
        //    return buildingInfo;
        //}

        //private void UpdateBuildingInfo(FinalBuilding building)
        //{
        //    hexName.text = building.buildingName;
        //    foreach(var prod in building.finalProduction)
        //    {
        //        for(int i = 0; i < resourcesProduction.Count; i++)
        //        {
        //            if (resourcesProduction[i].name == prod.Key.ToString())
        //            {
        //                resourcesProduction[i].SetActive(true);
        //                resourcesProduction[i].transform.GetChild(0).GetComponent<TMP_Text>().text = prod.Value.ToString();
        //            }
                    
        //        }


        //    }

        //}

    }
}

