using JetBrains.Annotations;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

namespace ProjectHex
{
    [HideReferenceObjectPicker]
    public class MyTileData
    {
        [HorizontalGroup("Split", 135), PropertyOrder(-1)]
        [PreviewField(130, Sirenix.OdinInspector.ObjectFieldAlignment.Left), HideLabel, JsonIgnore]
        public Sprite tileSprite; //image

        [FoldoutGroup("Split/$Name", false), Required]
        [Range(-1, 5)]
        public int offset = 0;
        [FoldoutGroup("Split/$Name", false), Required]
        public TileBonusSelf self;
        [FoldoutGroup("Split/$Name", false), CanBeNull]
        public TileBonusOther other;


 
        [HideInInspector]
        public string tileSpritePath;
        private string Name { get { return tileSprite != null ? tileSprite.name : "Null"; } }



        public void GetTileSpritePath()
        {
            tileSpritePath = AssetDatabase.GetAssetPath(this.tileSprite);
        }

        public void LoadTileSprite()
        {
            tileSprite = AssetDatabase.LoadAssetAtPath<Sprite>(tileSpritePath);
        }
    }
}
