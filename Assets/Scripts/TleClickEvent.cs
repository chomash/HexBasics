using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectHex
{
    public class TleClickEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject overlay;
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click!");
        }



        public void OnPointerEnter(PointerEventData eventData)
        {
            overlay.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            overlay.SetActive(false);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //empty
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            //empty
        }

    }
}
