using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using ProjectHex.UI;
using ProjectHex.Map;
using ProjectHex.Map.Building;
using ProjectHex.Map.Tiles;

namespace ProjectHex
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        #region references
        // these TM are odd-r, growing top-right
        [FoldoutGroup("Tile Maps", false)]
        [Required] public Tilemap tileTM, buildingTM, overlayTM;

        [FoldoutGroup("DataBase", false)]
        public TileDB tileDB;
        [FoldoutGroup("DataBase", false)]
        public HexDB _hexDB;
        [FoldoutGroup("DataBase", false)]
        public BuildingTemplateDB buildingTemplateDB;
        [FoldoutGroup("DataBase", false)]
        public BuildingDB _buildingDB;

        [InfoBox("Use with Caution! This will change how building bonuses are calculated. Default value is: 1")]
        public float maximumTileBonusCoefficient = 1f; // for simplicity sake, do not touch
        public TileBase overlayTile; //how overlay should look

        [FoldoutGroup("TileBonusClamp", false)]
        
        [HorizontalGroup("TileBonusClamp/1", LabelWidth = 200)]
        public  int tileBonusFinalMin = 0;
        [HorizontalGroup("TileBonusClamp/1", LabelWidth = 200)]
        public  int tileBonusFinalMax = 10;

        [HorizontalGroup("TileBonusClamp/2", LabelWidth = 200)]
        public int tileBonusOtherMin = -2;
        [HorizontalGroup("TileBonusClamp/2", LabelWidth = 200)]
        public int tileBonusOtherMax = 4;

    
        [SerializeField, FoldoutGroup("GenerationSettings")] private bool cleanStart = true;
        [SerializeField, FoldoutGroup("GenerationSettings")] private bool randomizeSeed = true;
        [SerializeField, FoldoutGroup("GenerationSettings")] private int seed;
        [SerializeField, FoldoutGroup("GenerationSettings"), Button]
        private void RegenerateWorld()
        {
            cleanStart = true;
            GenerateWorld();
        }
        #endregion

        private void Awake()
        {
            Singleton();
        }

        public void Start()
        {
            GenerateWorld();
            cleanStart = false;

        }

        void Update()
        {
            OnMouseClick();
            //OnMouseOver();
        }

        public void GenerateWorld()
        {
            if (cleanStart)
            {
                if (randomizeSeed)
                {
                    seed = (int)System.DateTime.Now.Ticks;
                }
                Random.InitState(seed);

                _hexDB.hexDataBase.Clear();
                _buildingDB.buildingDataBase.Clear();
                Initialize();
            }
        }

        private void Initialize() //add tile info to hextDataBase
        {
            for (int x = tileTM.origin.x; x < tileTM.origin.x + tileTM.size.x; x++)
            {
                for (int y = tileTM.origin.y; y < tileTM.origin.y + tileTM.size.y; y++)
                {
                    Vector3Int coords = new Vector3Int(x, y, 0);
                    if (tileTM.GetTile(coords) != null)
                    {
                        _hexDB.AddNewHex(ExtensionMethods.OffsetToCube(coords));
                    }
                }
            }

            UpdateProduction();
        }

        private void UpdateProduction()
        {
            foreach(var kvp in _buildingDB.buildingDataBase)
            {

                kvp.Value.UpdateProduction(_hexDB.hexDataBase[ExtensionMethods.OffsetToCube(kvp.Key)].finalBonus);
            }
        }

        private void OnMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = tileTM.WorldToCell(mousePosition);

                _hexDB.hexDataBase.TryGetValue(ExtensionMethods.OffsetToCube(gridPosition), out HexData value);

                UI_Manager.Instance.ShowHexInfo(value, gridPosition);

                HighlightNeighbours(gridPosition);
                Debug.Log(gridPosition);
            }
        }
        private void OnMouseOver()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tileTM.WorldToCell(mousePosition);
            TileBase overTile = tileTM.GetTile(gridPosition);

            if (overTile != null)
            {
                overlayTM.ClearAllTiles();
                overlayTM.SetTile(gridPosition, overlayTile);
            }

        }

        private void Singleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        #region Debug Functions

        private void HighlightNeighbours(Vector3Int coords)
        {
            foreach (var n in ExtensionMethods.GetNeighbors(ExtensionMethods.OffsetToCube(coords)))
            {
                if (tileTM.GetTile(ExtensionMethods.CubeToOffset(n)) != null)
                {
                    overlayTM.SetTile(ExtensionMethods.CubeToOffset(n), overlayTile);
                }
            }
        }
        #endregion


    }
}
