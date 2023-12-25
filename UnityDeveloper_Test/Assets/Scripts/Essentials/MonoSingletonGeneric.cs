
using UnityEngine;

namespace GravityGuy.essential
{
    public class MonoSingletonGeneric<T> : MonoBehaviour where T : MonoSingletonGeneric<T>
    {
        private static T instance;
        public static T Instance { get { return instance; } set { } }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("Duplicate Character instance detected");
                Destroy(gameObject);
            }
        }
    }

}
