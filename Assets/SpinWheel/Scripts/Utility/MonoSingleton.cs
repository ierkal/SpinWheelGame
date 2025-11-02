using UnityEngine;

namespace SpinWheel.Scripts.Utility
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        protected void InitializeSingleton()
        {
            if(Instance == null)
                Instance = this as T;
            else
                Destroy(gameObject);
        }
    }
}