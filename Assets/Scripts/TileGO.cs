using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectHex
{
    public class TileGO : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject offsetGO;
        public SpriteRenderer baseRef, corruptionRef, overlayRef, buildingRef;
        public QRS coords = QRS.zero;
        private float offset;
        public PolygonCollider2D colli;

        

        public void Start()
        {
            corruptionRef.enabled = false;
            overlayRef.enabled = false;
            buildingRef.enabled = false;
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
            offset = _offset;
            offsetGO.transform.localPosition = new Vector3(0, offset, 0);
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
            Debug.Log("Click!");
            offsetGO.transform.localPosition = new Vector3(0, offset+0.3f, 0);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            overlayRef.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            overlayRef.enabled = false;
            offsetGO.transform.localPosition = new Vector3(0, offset, 0);
        }

    }
}
