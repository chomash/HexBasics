using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectHex.Map;



namespace ProjectHex.UI
{
    public class UI_Manager : MonoBehaviour
    {
        public static UI_Manager instance { get; private set; }

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
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }
    }
}