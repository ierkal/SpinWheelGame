using UnityEngine;

namespace SpinWheel.Scripts.UI.EndGame
{
    [CreateAssetMenu(fileName = "so_ReviveData", menuName = "Endgame/ReviveData", order = 0)]
    public class ReviveData : ScriptableObject
    {
        public int ReviveCost;
    }
}