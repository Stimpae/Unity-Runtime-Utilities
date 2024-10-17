using System.Collections;
using UnityEngine;

namespace RuntimeUtilities {
    /// <summary>
    /// Encapsulates utility Coroutine methods.
    /// This class enables us to run coroutines from non-MonoBehaviour classes.
    /// It instantiates a hidden GameObject and adds an empty MonoBehaviour component
    /// to it that is used for starting/stopping coroutines.
    /// </summary>
    public static class CoroutineUtility {
        public class CoroutineHelper : MonoBehaviour {
        }

        private static MonoBehaviour _instance;

        private static MonoBehaviour Instance {
            get {
                if (_instance != null) return _instance;
                var instance = new GameObject(nameof(CoroutineUtility), typeof(CoroutineHelper));
                _instance = instance.GetComponent<CoroutineHelper>();
                instance.hideFlags = HideFlags.HideAndDontSave;
                Object.DontDestroyOnLoad(instance);

                return _instance;
            }
        }

        /// <summary>
        /// Starts a coroutine
        /// </summary>
        /// <param name="routine">The coroutine to start</param>
        /// <returns>The started coroutine</returns>
        public static Coroutine StartCoroutine(IEnumerator routine) => Instance.StartCoroutine(routine);

        /// <summary>
        /// Stops a coroutine
        /// </summary>
        /// <param name="coroutine">The coroutine to stop</param>
        public static void StopCoroutine(ref Coroutine coroutine) {
            if (coroutine != null) {
                Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}