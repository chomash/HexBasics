using Newtonsoft.Json;
using ProjectHex.Map.Tiles;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectHex.Map.Building
{
    public class FinalBuilding
    {
        [HorizontalGroup("Split", 135), PropertyOrder(-1)]
        [PreviewField(130, Sirenix.OdinInspector.ObjectFieldAlignment.Left), HideLabel, JsonIgnore]
        public TileBase buildingImage; //image
        [FoldoutGroup("Split/$Name", false), Required]
        public string buildingName;
        [FoldoutGroup("Split/$Name", false), Required]
        public HexDB.BuildingType buildingType;
        [FoldoutGroup("Split/$Name", false)]
        public Dictionary<HexDB.ResourceType, float> finalProduction = new(); //base prod and efficiency can be pulled from BuildingTemplateDB

        [HideInInspector] public string buildingTemplateReference;
        [HideInInspector]
        public string tileBasePath; //location of a tileBase


        private string Name { get { return this.buildingName != null ? buildingName : "Null"; } }

        public void UpdateProduction(TileBonusFinal tileBonus)
        {
            
            //k - co produkuje, v - ile i z jakim effi
            foreach(var kvp in GetBuildingTemplate(buildingTemplateReference))
            {
                if (finalProduction.ContainsKey(kvp.Key))
                {
                    finalProduction[kvp.Key] = CalculateSingleProduction(tileBonus, kvp.Value);
                }
                else
                {
                    finalProduction.Add(kvp.Key, CalculateSingleProduction(tileBonus, kvp.Value));
                }
            }

        }
        private float CalculateSingleProduction(TileBonusFinal tileBonus, ResourceProduction resourceProduction)
        {
            float bonusCoefficient = CalculateTileCoefficient(tileBonus, resourceProduction.tileBonusEfficiency);

            float finalBonusProduction = bonusCoefficient * resourceProduction.baseAmount * MapManager.Instance.maximumTileBonusCoefficient;

            float finalProduction = roundtofourth(finalBonusProduction + resourceProduction.baseAmount);

            return finalProduction;
        }


        private float CalculateTileCoefficient(TileBonusFinal tileBonus, ElementEfficiency efficiency)
        {
            float maxTileBonusDivisiton = 1.0f / MapManager.Instance.tileBonusFinalMax;

            float natureBonus = tileBonus.nature * efficiency.nature * maxTileBonusDivisiton;
            float sunBonus = tileBonus.sun * efficiency.sun * maxTileBonusDivisiton;
            float waterBonus = tileBonus.water * efficiency.water * maxTileBonusDivisiton;
            float punkBonus = tileBonus.punk * efficiency.punk * maxTileBonusDivisiton;
            float corrBonus = tileBonus.corruption * efficiency.corruption * maxTileBonusDivisiton;

            return natureBonus + sunBonus + waterBonus + punkBonus + corrBonus;

        }




        private float roundtofourth(float x)
        {
            return Mathf.Round(x * 4) / 4;
        }

        private Dictionary<HexDB.ResourceType, ResourceProduction> GetBuildingTemplate(string buildingTemplateReference)
        {
            MapManager.Instance.buildingTemplateDB.buildingTemplate.TryGetValue(buildingTemplateReference, out BuildingTemplate buildingTemplate);
            return buildingTemplate.resourceProduction;
        }
    }
}
