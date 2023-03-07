using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using ProjectHex.Map.Tiles;
using Sirenix.Utilities;
using Newtonsoft.Json;
using UnityEditor;
using Unity.Burst.Intrinsics;

/// <summary>
/// all info of a built building
/// </summary>
namespace ProjectHex.Map.Building
{
    [HideReferenceObjectPicker]
    public class BuildingTemplate
    {

        [HorizontalGroup("Split", 135), PropertyOrder(-1)]
        [PreviewField(130, Sirenix.OdinInspector.ObjectFieldAlignment.Left), HideLabel, JsonIgnore]
        public TileBase tileBase; //image
        [FoldoutGroup("Split/$Name", false), Required]
        public string buildingName;
        [FoldoutGroup("Split/$Name", false), Required]
        public BuildingType buildingType;

        [FoldoutGroup("Split/$Name", false), HideReferenceObjectPicker]
        public Dictionary<HexDB.ResourceType, ResourceProduction> resourceProduction = new();


        //public BuildingYield yieldBaseAmount;
        [FoldoutGroup("Split/$Name", false)]
        //public TileBonusEfficiency bonusEfficiency;

        [HideInInspector]
        public string tileBasePath;

        public enum BuildingType { Main, Addition, Special, Undefined }
        private string Name { get { return this.buildingName != null ? buildingName : "Null"; } }

        public void GetTileBasePath()
        {
            tileBasePath = AssetDatabase.GetAssetPath(this.tileBase);
        }

        public void LoadTileBase()
        {
            tileBase = AssetDatabase.LoadAssetAtPath<TileBase>(tileBasePath);
        }
    
    }
}
