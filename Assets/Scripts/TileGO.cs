using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace ProjectHex
{
    public class TileGO : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject offsetGO;
        public SpriteRenderer baseRef, corruptionRef, overlayRef, buildingRef;
        public QRS coords = QRS.zero;
        [SerializeField] private float animationOffset = 0.1f;
        private float tileOffset;
        private bool isSelected = false;
        


        
        public void CheckForCorruption()
        {
            TileBase corr = MapManager.Instance.corruptionTM.GetTile(coords.ToOffset());
            if (corr != null)
            {
                SetCorruption(MapManager.Instance.tileDB.corruptionDictionary[corr], true);
            }
            else
            {
                SetCorruption(null, false);
            }
        }






        public void SetBaseTile(Sprite baseTileSprite)
        {
            baseRef.sprite = baseTileSprite;
        }
        public void SetCoords(QRS coords)
        {
            this.coords = coords;

        }
        public void SetOffset(float _offset)
        {
            tileOffset = _offset;
            offsetGO.transform.localPosition = new Vector3(0, tileOffset, 0);
        }
        public void SetBuilding(Sprite buildingSprite, bool isVisible)
        {
            buildingRef.sprite = buildingSprite;
            buildingRef.enabled = isVisible;
        }
        public void SetCorruption(Sprite corruptionSprite, bool isVisible)
        {
            corruptionRef.enabled = isVisible;
            corruptionRef.sprite = corruptionSprite;
        }
        
        
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (isSelected)
            {
                isSelected = false;
                offsetGO.transform.localPosition = new Vector3(0, tileOffset, 0);
            }
            else
            {
                isSelected= true;
                UI_Manager.Instance.ShowHexInfo(MapManager.Instance._hexDB.hexDataBase[coords], coords.ToOffset());
                offsetGO.transform.localPosition = new Vector3(0, tileOffset + animationOffset, 0);
            }

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            overlayRef.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            overlayRef.enabled = false;
            offsetGO.transform.localPosition = new Vector3(0, tileOffset, 0);
        }

    }
}
