using UnityEngine;

namespace RuntimeUtilities.Singleton {
    public abstract class AbstractSingleton<T> : MonoBehaviour where T : Component {
        private static T _instance;
        public static bool HasInstance => _instance != null;
        public static T TryGetInstance() => _instance;
        
        public static T Instance {
            get {
                if (_instance == null) {
                    _instance = FindAnyObjectByType<T>() ?? new GameObject(typeof(T).Name + " Auto-Generated").AddComponent<T>();
                }
                return _instance;
            }
        }
        
        protected virtual void Awake() {
            if (Application.isPlaying) _instance = this as T;
        }
    }
}