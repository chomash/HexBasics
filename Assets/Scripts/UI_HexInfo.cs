using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using System.Linq.Expressions;
using ProjectHex.Map;
using ProjectHex.Map.Building;
using ProjectHex.Map.Tiles;

//using UnityEngine.UIElements;
namespace ProjectHex.UI
{
    public class UI_HexInfo : MonoBehaviour
    {

        [SerializeField] private TMP_Text hexName;
        [FoldoutGroup("Tiles", false)]
        [SerializeField] Image tileImage;
        [FoldoutGroup("Tiles", false)]
        [SerializeField] private TMP_Text natureValue, sunValue, waterWalue, punkValue, corruptionValue;
        [FoldoutGroup("Buildings", false)]
        [SerializeField] Image buildingImage;
        [FoldoutGroup("Buildings", false)]
        [SerializeField] GameObject buildingInfo;
        [FoldoutGroup("Buildings", false)]
        [SerializeField] private GameObject rawFood, food, junk, power, tea;

        public void UpdateHexInfo(HexData hexData, Vector3Int position)
        {
            if (hexData.buildingReference != new Vector3Int(2137, 2137, 2137))
            {
                //UpdateBuildingInfo(hexData.buildingKey);
            }
            else
            {
                buildingInfo.SetActive(false);
                hexName.text = "Empty";

            }

            tileImage.sprite = MapManager.Instance.tileTM.GetSprite(position);
            tileImage.color = MapManager.Instance.tileTM.GetColor(position);

            natureValue.text = hexData.finalBonus.nature.ToString();
            sunValue.text = hexData.finalBonus.sun.ToString();
            punkValue.text = hexData.finalBonus.punk.ToString();
            waterWalue.text = hexData.finalBonus.water.ToString();
            corruptionValue.text = hexData.finalBonus.corruption.ToString();
        }

        /* solid refactor needed
        private void UpdateBuildingInfo(Vector3Int coords)
        {
            buildingInfo.SetActive(true);
            FinalBuilding buildingRef = MapManager.Instance._buildingDB.GetBuildingData(coords);
            buildingImage.sprite = MapManager.Instance.buildingTM.GetSprite(coords);
            hexName.text = buildingRef.buildingName;

            if (buildingRef.yieldFinalAmount.rawFood == 0)
                rawFood.SetActive(false);
            else
            {
                rawFood.SetActive(true);
                rawFood.transform.GetChild(0).GetComponent<TMP_Text>().text = buildingRef.yieldFinalAmount.rawFood.ToString();
            }

            if (buildingRef.yieldFinalAmount.food == 0)
                food.SetActive(false);
            else
            {
                food.SetActive(true);
                food.transform.GetChild(0).GetComponent<TMP_Text>().text = buildingRef.yieldFinalAmount.food.ToString();
            }

            if (buildingRef.yieldFinalAmount.junk == 0)
                junk.SetActive(false);
            else
            {
                junk.SetActive(true);
                junk.transform.GetChild(0).GetComponent<TMP_Text>().text = buildingRef.yieldFinalAmount.junk.ToString();
            }

            if (buildingRef.yieldFinalAmount.energy == 0)
                power.SetActive(false);
            else
            {
                power.SetActive(true);
                power.transform.GetChild(0).GetComponent<TMP_Text>().text = buildingRef.yieldFinalAmount.energy.ToString();
            }

            if (buildingRef.yieldFinalAmount.tea == 0)
                tea.SetActive(false);
            else
            {
                tea.SetActive(true);
                tea.transform.GetChild(0).GetComponent<TMP_Text>().text = buildingRef.yieldFinalAmount.tea.ToString();
            }
        }
        */
    }
}

