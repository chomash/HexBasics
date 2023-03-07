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

        public float buildingBonusCoefficient = 0.5f;

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


        [SerializeField] private bool cleanStart = true;
        #endregion

        private void Awake()
        {
            Singleton();
        }

        public void Start()
        {
            if (cleanStart)
            {
                _hexDB.hexDataBase.Clear();
                _buildingDB.buildingDataBase.Clear();
                Initialize();
                cleanStart = false;
            }

        }

        void Update()
        {
            OnMouseClick();
            //OnMouseOver();
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
                        _hexDB.AddNewHex(coords);
                    }
                }
            }
        }




        private void OnMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = tileTM.WorldToCell(mousePosition);

                _hexDB.hexDataBase.TryGetValue(new Vector2Int(gridPosition.x, gridPosition.y), out HexData value);

                UI_Manager.instance.ShowHexInfo(value, gridPosition);

                HighlightNeighbours(gridPosition.x, gridPosition.y);
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

        private void HighlightNeighbours(int x, int y)
        {
            List<Vector3Int> neighborCoords = new();
            overlayTM.ClearAllTiles();
            if (y % 2 == 0)
            {
                foreach (Vector2Int offset in _hexDB.evenRowNeighborOffset)
                {
                    neighborCoords.Add(new Vector3Int(x + offset.x, y + offset.y, 0));
                }
            }
            else
            {
                foreach (Vector2Int offset in _hexDB.oddRowNeighborOffset)
                {
                    neighborCoords.Add(new Vector3Int(x + offset.x, y + offset.y, 0));
                }
            }


            foreach (Vector3Int coords in neighborCoords)
            {
                if (MapManager.Instance.tileTM.GetTile(coords) != null)
                {
                    overlayTM.SetTile(coords, overlayTile);
                }
            }
        }
        #endregion
    }
}
