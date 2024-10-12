using UnityEngine;

namespace RuntimeUtilities {
    /// <summary>
    /// This class is used to limit the frame rate of the game.
    /// Useful for testing on different devices and catching performance issues and frame rate dependent bugs.
    /// </summary>
    [DisallowMultipleComponent]
    public class FrameRateLimiter : MonoBehaviour {
        private void Update() {
            if (!Input.GetKey(KeyCode.LeftShift)) return;
            if (Input.GetKeyDown(KeyCode.Keypad1)) Application.targetFrameRate = 10;
            if (Input.GetKeyDown(KeyCode.Keypad2)) Application.targetFrameRate = 20;
            if (Input.GetKeyDown(KeyCode.Keypad3)) Application.targetFrameRate = 30;
            if (Input.GetKeyDown(KeyCode.Keypad4)) Application.targetFrameRate = 60;
            if (Input.GetKeyDown(KeyCode.Keypad5)) Application.targetFrameRate = 900;
        }
    }
}
