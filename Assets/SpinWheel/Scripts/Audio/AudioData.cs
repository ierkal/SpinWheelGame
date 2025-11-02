using System;
using UnityEngine;

namespace SpinWheel.Scripts.Audio
{
    [Serializable]
    public enum AudioKey
    {
        WheelSpin,
        ZoneReach,
        PlayerDie,
        Card
    }

    [Serializable]
    public struct AudioEntry
    {
        public AudioKey Key;
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume;
        public bool FadeOut;
    }
}