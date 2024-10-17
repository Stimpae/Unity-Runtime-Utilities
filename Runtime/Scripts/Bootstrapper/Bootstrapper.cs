using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeUtilities.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace RuntimeUtilities.Bootstrapper {
    /// <summary>
    /// Handles the initialization of the game and the persistent objects.
    /// There should be only one instance of this class in the game, it should be present in the BOOT scene.
    /// </summary>
    public class Bootstrapper : Singleton<Bootstrapper> {
        // List of persistent objects that will be instantiated at the start of the game and available from the start to the end of the game
        [SerializeField] private List<GameObject> persistentObjects = new List<GameObject>();
        
        public UnityEvent onBootstrapperInitializedEvent;
        public event UnityAction OnBootstrapperInitialized;
        
        protected override void Awake() {
            base.Awake();
        }
        
        /// <summary>
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        protected static async void InitBeforeSceneLoad() {
            // Load first scene?
        }


        /// <summary>
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static async void InitAfterSceneLoad() {
            await LoadPersistentObjects();
            Instance.OnBootstrapperInitialized?.Invoke();
            Instance.onBootstrapperInitializedEvent?.Invoke();
        }
        
        /// <summary>
        /// Loads the persistent objects.
        /// </summary>
        /// <returns> The completed task </returns>
        private static Task LoadPersistentObjects() {
            if (Instance.persistentObjects.Count == 0) {
                return Task.CompletedTask;
            }

            // Instantiate each persistent object in the list
            foreach (var persistentObject in Instance.persistentObjects) {
                Instantiate(persistentObject);
            }

            return Task.CompletedTask;
        }
    }
}