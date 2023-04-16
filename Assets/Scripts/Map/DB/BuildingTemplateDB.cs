using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


#pragma warning disable 0414 //disable 'showSave not in use' warning
namespace ProjectHex
{
    [CreateAssetMenu(menuName = "Data/DB/AvaliableBuildings")]

    public class BuildingTemplateDB : SerializedScriptableObject //Database of avaliable buildings
    {
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout), PropertyOrder(10), HideReferenceObjectPicker]
        public Dictionary<string, BuildingTemplate> buildingTemplate = new();






        #region Save & Load BUTTONS - USED IN EDITOR ONLY
        private static string directory = "/Resources/json/";
        private static string fileName = "BuildingDB.json";
        private bool showSave = false;


        [HorizontalGroup("Split", 0.5f)]
        [VerticalGroup("Split/left"), HideIf("showSave")]
        [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            showSave = true;
        }
        
        [VerticalGroup("Split/left", 0.5f), ShowIf("showSave")]
        [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        public void GoBack()
        {
            showSave = false;
        }
        
        [VerticalGroup("Split/right", 0.5f), HideIf("showSave")]
        [Button(ButtonSizes.Large), GUIColor(0.2f, 1f, 0.4f)]
        public void Load()
        {
            string fullPath = Application.dataPath + directory + fileName;
            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                buildingTemplate = JsonConvert.DeserializeObject<Dictionary<string, BuildingTemplate>>(json);
                LoadTileBase();
            }
            else
            {
                Debug.Log("TileDB not found");
            }
            showSave = false;
        }
        
        [VerticalGroup("Split/right", 0.5f), ShowIf("showSave")]
        [Button(ButtonSizes.Large), GUIColor(1f, 0.3f, 0.1f)]
        public void ImSure()
        {
            GetTileBasePath();
            string dir = Application.dataPath + directory;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string json = JsonConvert.SerializeObject(buildingTemplate, Formatting.Indented);
            File.WriteAllText(dir + fileName, json);
            showSave = false;
        }



        public void GetTileBasePath()
        {
            foreach(var t in buildingTemplate)
            {
                t.Value.GetTileBasePath();
            }
        }

        public void LoadTileBase()
        {
            foreach(var t in buildingTemplate)
            {
                t.Value.LoadTileBase();
            }
        }
        #endregion












    }
}
