using System;
using SpinWheel.Scripts.Utility;
using UnityEngine;

namespace SpinWheel.Scripts.Factory
{
    public static class SpriteFactory
    {
        private const string BombIconName = "ui_card_icon_death";
        public static Sprite GetSprite(string iconName)
        {
            try
            {
                string dir = "ItemSprites/" + iconName;
                Texture2D tex = Resources.Load<Texture2D>(dir);
                Sprite sprite = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),100);
                
                return sprite;
            }
            catch (Exception e)
            {
                Debug.LogError("sprite icon: "+ iconName + " " + e.Message);
                return null;
            }
        }

        public static Sprite GetBombSprite()
        {
            return GetSprite(BombIconName);
        }
    }
}