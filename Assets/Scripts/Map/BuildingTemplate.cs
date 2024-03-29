using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.Utilities;
using Newtonsoft.Json;
using UnityEditor;
using Unity.Burst.Intrinsics;

/// <summary>
/// all info of a built building
/// </summary>
namespace ProjectHex
{
    [HideReferenceObjectPicker]
    public class BuildingTemplate
    {

        [HorizontalGroup("Split", 135), PropertyOrder(-1)]
        [PreviewField(130, Sirenix.OdinInspector.ObjectFieldAlignment.Left), HideLabel, JsonIgnore]
        public Sprite buildingImage; //image
        [FoldoutGroup("Split/$Name", false), Required]
        public string buildingName;
        [FoldoutGroup("Split/$Name", false), Required]
        public BuildingType buildingType;

        [FoldoutGroup("Split/$Name", false), HideReferenceObjectPicker]
        public Dictionary<HexDB.ResourceType, ResourceProduction> resourceProduction = new();

        [HideInInspector]
        public string tileBasePath;

        public enum BuildingType { Main, Addition, Special, Undefined }
        private string Name { get { return this.buildingName != null ? buildingName : "Null"; } }

        public void GetTileBasePath()
        {
            tileBasePath = AssetDatabase.GetAssetPath(this.buildingImage);
        }

        public void LoadTileBase()
        {
            buildingImage = AssetDatabase.LoadAssetAtPath<Sprite>(tileBasePath);
        }
    
    }
}
