using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

#pragma warning disable 0414 //disable 'showSave not in use' warning
namespace ProjectHex
{
  
    
    [CreateAssetMenu(menuName = "Data/DB/Tiles")]
    public class TileDB : SerializedScriptableObject
    {
     
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout), PropertyOrder(10), HideReferenceObjectPicker]
        public Dictionary<HexDB.TileType, MyTileData> tileDataBase = new();

        #region Save & Load
        private static string directory = "/Resources/json/";
        private static string fileName = "TileDB.json";
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
                tileDataBase = JsonConvert.DeserializeObject<Dictionary<HexDB.TileType, MyTileData>>(json);
                LoadTileImage();
            }
            else
            {
                Debug.Log("TileDB not found");
            }
            showSave = false;
        }

        [VerticalGroup("Split/right", 0.5f), ShowIf("showSave")]
        [Button(ButtonSizes.Large), GUIColor(1f, 0.3f, 0.1f)]
        public void YesSave()
        {
            GetTileImagePath();
            string dir = Application.dataPath + directory;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string json = JsonConvert.SerializeObject(tileDataBase, Formatting.Indented);
            File.WriteAllText(dir + fileName, json);
            showSave = false;
        }

        public void GetTileImagePath()
        {
            foreach (var t in tileDataBase)
            {
                t.Value.GetTileSpritePath();
            }
        }

        public void LoadTileImage()
        {
            foreach (var t in tileDataBase)
            {
                t.Value.LoadTileSprite();
            }
        }
        #endregion

    }
}
