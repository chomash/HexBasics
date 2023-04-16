using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Rendering;

namespace ProjectHex
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        #region references
        // these TM are odd-r, growing top-right
        [FoldoutGroup("Tile Maps", false)]
        [Required, SerializeField] private GameObject TMContainer;
        [FoldoutGroup("Tile Maps", false)]
        [Required] public Tilemap tileTM, buildingTM, corruptionTM;

        [FoldoutGroup("DataBase", false)]
        public TileDB tileDB;
        [FoldoutGroup("DataBase", false)]
        public HexDB _hexDB;
        [FoldoutGroup("DataBase", false)]
        public BuildingTemplateDB buildingTemplateDB;
        [FoldoutGroup("DataBase", false)]
        public BuildingDB _buildingDB;



        [FoldoutGroup("TileBonusClamp", false)]
        
        [HorizontalGroup("TileBonusClamp/1")]
        public  int tileBonusFinalMin = 0;
        [HorizontalGroup("TileBonusClamp/1")]
        public  int tileBonusFinalMax = 10;

        [HorizontalGroup("TileBonusClamp/2")]
        public int tileBonusOtherMin = -2;
        [HorizontalGroup("TileBonusClamp/2")]
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

        [InfoBox("Use with Caution! This will change how building bonuses are calculated. Default value is: 1")]
        public float maximumTileBonusCoefficient = 1f; // for simplicity sake, do not touch
        public float offsetMulti = 0.1f;
        public GameObject hexContainer;
        #endregion

        private void Awake()
        {
            Singleton();
        }

        public void Start()
        {
            GenerateWorld();
            cleanStart = false;
            TMContainer.SetActive(false);
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
                        _hexDB.AddNewHex(coords.ToCube());
                    }
                }
            }

            //UpdateProduction();
        }

        private void UpdateProduction()
        {
            foreach(var kvp in _buildingDB.buildingDataBase)
            {

                kvp.Value.UpdateProduction(_hexDB.hexDataBase[ExtensionMethods.ToCube(kvp.Key)].finalBonus);
            }
        }

        private void OnMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {

                //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                


                //UI_Manager.Instance.ShowHexInfo(value, gridPosition);

                //HighlightNeighbours(gridPosition);
                //Debug.Log(gridPosition);
            }
        }
        private void OnMouseOver()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tileTM.WorldToCell(mousePosition);
            TileBase overTile = tileTM.GetTile(gridPosition);

            if (overTile != null)
            {

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
            foreach (var n in coords.ToCube().GetNeighbors())
            {
                if (tileTM.GetTile(ExtensionMethods.ToOffset(n)) != null)
                {
                    
                }
            }
        }
        #endregion


    }
}
