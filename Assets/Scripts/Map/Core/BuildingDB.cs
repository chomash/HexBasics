using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using ProjectHex.Map.Tiles;
using ProjectHex.Map.Building;
using UnityEngine.Rendering;

namespace ProjectHex
{
    [CreateAssetMenu(menuName = "Data/DB/BuiltBuildings")]
    public class BuildingDB : SerializedScriptableObject
    {
        //[ReadOnly]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout), ReadOnly]
        public Dictionary<Vector3Int, FinalBuilding> buildingDataBase = new();


        //reword THIS
        public FinalBuilding GetBuildingData(Vector3Int position)
        {
            return buildingDataBase[position];
        }

        public void AddBuilding(Vector3Int buildingKey, string buildingName, TileBonusFinal tileBonus)
        {
            MapManager.Instance.buildingTemplateDB.buildingTemplate.TryGetValue(buildingName, out BuildingTemplate template);

            FinalBuilding newBuilding = new();

            newBuilding.buildingName = template.buildingName;
            newBuilding.tileBase = template.tileBase;
            Debug.Log(template.buildingType.GetType().GetEnumValues().ToString());
            //newBuilding.buildingType = template.buildingType.Get;
            //newBuilding.finalProduction = 
            //newBuilding.yieldFinalAmount = CalculateFinalYield(newBuilding.yieldBaseAmount, newBuilding.bonusEfficiency, tileBonus);

            //buildingDataBase.Add(buildingKey, newBuilding);
        }

        //private Dictionary<HexDB.ResourceType, int> CalculateFinalProduction(BuildingTemplate buildingTemplate)
        //{
        //    Dictionary<HexDB.ResourceType, int> temp = new();
        //    foreach (string resourceType in buildingTemplate.buildingType.GetType().GetEnumValues())

        //    return temp;
        //}
        //private BuildingYieldFinal CalculateFinalYield(BuildingYield baseYield, ElementEfficiency effi, TileBonusFinal tileBonus)
        //{
        //    BuildingYieldFinal newBuildingYield = new();
        //    newBuildingYield.rawFood = CalculateSingleYield(baseYield.rawFood, effi, tileBonus);
        //    newBuildingYield.food = CalculateSingleYield(baseYield.food, effi, tileBonus);
        //    newBuildingYield.junk = CalculateSingleYield(baseYield.junk, effi, tileBonus);
        //    newBuildingYield.energy = CalculateSingleYield(baseYield.energy, effi, tileBonus);
        //    newBuildingYield.tea = CalculateSingleYield(baseYield.tea, effi, tileBonus);
        //    return newBuildingYield;
        //}

       
        private float CalculateSingleYield(int y, ElementEfficiency eff, TileBonusFinal tileBonus) // move to hex/building, calculate self?
        {
            float maxTileBonusDivisiton = 1.0f / MapManager.Instance.tileBonusFinalMax;

            float natureBonus = eff.nature * tileBonus.nature * maxTileBonusDivisiton;
            float sunBonus = eff.sun * tileBonus.sun * maxTileBonusDivisiton;
            float waterBonus = eff.water * tileBonus.water * maxTileBonusDivisiton;
            float punkBonus = eff.punk * tileBonus.punk * maxTileBonusDivisiton;
            float corruptionBonus = -eff.corruption * tileBonus.corruption * maxTileBonusDivisiton;

            float bonusSum = natureBonus + sunBonus + waterBonus + punkBonus + corruptionBonus;

            float finalBonus = bonusSum * y * MapManager.Instance.buildingBonusCoefficient;



            return RoundToFourth(finalBonus + y);
        }

        private float RoundToFourth(float x)
        {
            return Mathf.Round(x * 4) / 4;
        }
    }

}