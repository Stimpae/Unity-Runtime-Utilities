using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace RuntimeUtilities.Bootstrapper.Editor {
    /// <summary>
    /// Auto-loads a bootstrap screen (first scene in Build Settings) while working in the Editor.
    /// Adds menu items to toggle behavior.
    /// </summary>
    [InitializeOnLoad]
    public class EditorBootstrapper {
        private const string K_PREVIOUS_SCENE = "PreviousScene";
        private const string K_SHOULD_LOAD_BOOTSTRAP = "LoadBootstrapScene";
        private const string K_LOAD_BOOTSTRAP_MENU = "Tools/Utilities/Load Bootstrap Scene On Play";
        private const string K_DONT_LOAD_BOOTSTRAP_MENU = "Tools/Utilities/Don't Load Bootstrap Scene On Play";

        private static string BootstrapScene => EditorBuildSettings.scenes[0].path;

        private static string PreviousScene {
            get => EditorPrefs.GetString(K_PREVIOUS_SCENE);
            set => EditorPrefs.SetString(K_PREVIOUS_SCENE, value);
        }

        private static bool ShouldLoadBootstrapScene {
            get => EditorPrefs.GetBool(K_SHOULD_LOAD_BOOTSTRAP, true);
            set => EditorPrefs.SetBool(K_SHOULD_LOAD_BOOTSTRAP, value);
        }

        static EditorBootstrapper() {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange) {
            if (!ShouldLoadBootstrapScene) return;

            switch (playModeStateChange) {
                case PlayModeStateChange.ExitingEditMode:
                    PreviousScene = SceneManager.GetActiveScene().path;
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() && IsSceneInBuildSettings(BootstrapScene)) {
                        EditorSceneManager.OpenScene(BootstrapScene);
                    }
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    if (!string.IsNullOrEmpty(PreviousScene)) {
                        EditorSceneManager.OpenScene(PreviousScene);
                    }
                    break;
            }
        }

        [MenuItem(K_LOAD_BOOTSTRAP_MENU)]
        private static void EnableBootstrapper() => ShouldLoadBootstrapScene = true;

        [MenuItem(K_LOAD_BOOTSTRAP_MENU, true)]
        private static bool ValidateEnableBootstrapper() => !ShouldLoadBootstrapScene;

        [MenuItem(K_DONT_LOAD_BOOTSTRAP_MENU)]
        private static void DisableBootstrapper() => ShouldLoadBootstrapScene = false;

        [MenuItem(K_DONT_LOAD_BOOTSTRAP_MENU, true)]
        private static bool ValidateDisableBootstrapper() => ShouldLoadBootstrapScene;

        private static bool IsSceneInBuildSettings(string scenePath) {
            return !string.IsNullOrEmpty(scenePath) && EditorBuildSettings.scenes.Any(scene => scene.path == scenePath);
        }
    }
}