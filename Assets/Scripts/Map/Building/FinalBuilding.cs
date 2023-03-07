using Newtonsoft.Json;
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
        public TileBase tileBase; //image
        [FoldoutGroup("Split/$Name", false), Required]
        public string buildingName;
        [FoldoutGroup("Split/$Name", false), Required]
        public HexDB.BuildingType buildingType;
        [FoldoutGroup("Split/$Name", false)]
        public Dictionary<HexDB.ResourceType, int> finalProduction = new(); //base prod and efficiency can be pulled from BuildingTemplateDB

        [HideInInspector]
        public string tileBasePath; //location of a tileBase


        private string Name { get { return this.buildingName != null ? buildingName : "Null"; } }

        public void UpdateYield(Vector3Int myCoords)
        {
            //recalculate based on building template and hexdata from coords
        }

    }
}
