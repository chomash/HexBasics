using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ProjectHex
{
    public class UI_Manager : MonoBehaviour
    {
        public static UI_Manager Instance { get; private set; }

        [Required]
        [SerializeField]
        private UI_HexInfo ui_hexInfo;

        private void Awake()
        {
            Singleton();
        }


        public void ShowHexInfo(HexData hD, Vector3Int position)
        {
            ui_hexInfo.UpdateHexInfo(hD, position);
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
    }
}