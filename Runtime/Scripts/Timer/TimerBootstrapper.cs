using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace TG.Utilities {
    /// <summary>
    /// Handles the initialization and cleanup of the TimerManager in the Unity PlayerLoop.
    /// </summary>
    public static class TimerBootstrapper {
        private static PlayerLoopSystem _timerSystem;

        /// <summary>
        /// Initializes the TimerManager and inserts it into the PlayerLoop before the scene loads.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init() {
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            if (!InsertTimerManager<Update>(ref currentPlayerLoop, 0)) {
                Debug.LogWarning("Timers not initialized, unable to register TimerManager into the Update loop.");
                return;
            }
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Clears editor garbage and sets up play mode state change handling.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ClearEditorGarbage() {
            EditorApplication.playModeStateChanged -= OnPlayModeState;
            EditorApplication.playModeStateChanged += OnPlayModeState;
        }

        /// <summary>
        /// Handles the play mode state change to clean up the TimerManager when exiting play mode.
        /// </summary>
        /// <param name="state">The current play mode state.</param>
        static void OnPlayModeState(PlayModeStateChange state) {
            if (state != PlayModeStateChange.ExitingPlayMode) return;
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            RemoveTimerManager<Update>(ref currentPlayerLoop);
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
            TimerManager.Clear();
        }
#endif

        /// <summary>
        /// Removes the TimerManager from the specified PlayerLoop system.
        /// </summary>
        /// <typeparam name="T">The type of PlayerLoop system to remove the TimerManager from.</typeparam>
        /// <param name="loop">The PlayerLoop system to modify.</param>
        private static void RemoveTimerManager<T>(ref PlayerLoopSystem loop) {
            PlayerLoopUtils.RemoveSystem<T>(ref loop, in _timerSystem);
        }

        /// <summary>
        /// Inserts the TimerManager into the specified PlayerLoop system at the given index.
        /// </summary>
        /// <typeparam name="T">The type of PlayerLoop system to insert the TimerManager into.</typeparam>
        /// <param name="loop">The PlayerLoop system to modify.</param>
        /// <param name="index">The index at which to insert the TimerManager.</param>
        /// <returns>True if the TimerManager was successfully inserted, false otherwise.</returns>
        private static bool InsertTimerManager<T>(ref PlayerLoopSystem loop, int index) {
            _timerSystem = new PlayerLoopSystem {
                type = typeof(TimerManager),
                updateDelegate = TimerManager.UpdateTimers
            };
            return PlayerLoopUtils.InsertSystem<T>(ref loop, in _timerSystem, index);
        }
    }
}