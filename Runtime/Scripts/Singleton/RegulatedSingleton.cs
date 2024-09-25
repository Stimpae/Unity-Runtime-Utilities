using UnityEngine;

namespace TG.Utilities {
    /// <summary>
    /// A regulated singleton class for Unity components.
    /// Ensures that only one instance of the component exists in the scene and regulates its initialization.
    /// </summary>
    /// <typeparam name="T">The type of the component.</typeparam>
    public class RegulatedSingleton<T> : MonoBehaviour where T : Component {
        protected static T instance;
        
        public static bool HasInstance => instance != null;
        public static T TryGetInstance() => instance;
        public float InitializationTime { get; private set; }
        
        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindAnyObjectByType<T>() ?? CreateNewInstance();
                }
                return instance;
            }
        }
        
        private static T CreateNewInstance() {
            var go = new GameObject(typeof(T).Name + " Auto-Generated") {
                hideFlags = HideFlags.HideAndDontSave
            };
            return go.AddComponent<T>();
        }
        
        protected virtual void Awake() {
            InitializeSingleton();
        }
        
        protected virtual void InitializeSingleton() {
            if (!Application.isPlaying) return;
            InitializationTime = Time.time;
            DontDestroyOnLoad(gameObject);

            foreach (T old in FindObjectsByType<T>(FindObjectsSortMode.None)) {
                if (old.GetComponent<RegulatedSingleton<T>>().InitializationTime < InitializationTime) {
                    Destroy(old.gameObject);
                }
            }

            if (instance == null) {
                instance = this as T;
            }
        }
    }
}