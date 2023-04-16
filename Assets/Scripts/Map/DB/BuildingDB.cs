using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Net.NetworkInformation;

namespace ProjectHex
{
    [CreateAssetMenu(menuName = "Data/DB/BuiltBuildings")]
    public class BuildingDB : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "Coordinates", ValueLabel = "Building Data")]
        [ReadOnly]
        public Dictionary<Vector3Int, FinalBuilding> buildingDataBase = new();


        //reword THIS
        public FinalBuilding GetBuildingData(Vector3Int position)
        {
            return buildingDataBase[position];
        }

        public void AddBuilding(Vector3Int coords, string buildingName)
        {
            FinalBuilding newBuilding = new();
            BuildingTemplate template = GetBuildingTemplate(buildingName);
            if (template != null)
            {
                newBuilding.buildingName = template.buildingName;
                newBuilding.buildingImage = template.buildingImage;
                newBuilding.buildingTemplateReference = buildingName;
                buildingDataBase.Add(coords, newBuilding);
            }
            else
            {
                Debug.Log($"Building at: {coords.x}, {coords.y} do not have a valid template!");
            }
        }

        private BuildingTemplate GetBuildingTemplate(string buildingName) 
        {
            MapManager.Instance.buildingTemplateDB.buildingTemplate.TryGetValue(buildingName, out BuildingTemplate template);
            return template;
        }

    }

}