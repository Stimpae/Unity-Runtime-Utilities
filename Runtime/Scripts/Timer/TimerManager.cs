using System.Collections.Generic;

namespace TG.Utilities {
    /// <summary>
    /// Manages the registration, deregistration, and updating of timers.
    /// </summary>
    public static class TimerManager {
        private static readonly List<Timer> Timers = new();
        private static readonly List<Timer> Sweep = new();
        
        /// <summary>
        /// Registers a timer to be managed.
        /// </summary>
        /// <param name="timer">The timer to register.</param>
        public static void RegisterTimer(Timer timer) => Timers.Add(timer);

        /// <summary>
        /// Deregisters a timer from being managed.
        /// </summary>
        /// <param name="timer">The timer to deregister.</param>
        public static void DeregisterTimer(Timer timer) => Timers.Remove(timer);

        /// <summary>
        /// Updates all registered timers by calling their Tick method.
        /// </summary>
        public static void UpdateTimers() {
            if (Timers.Count == 0) return;
            
            Sweep.RefreshWith(Timers);
            foreach (var timer in Sweep) {
                timer.Tick();
            }
        }
        
        /// <summary>
        /// Clears all registered timers and disposes of them.
        /// </summary>
        public static void Clear() {
            Sweep.RefreshWith(Timers);
            foreach (var timer in Sweep) {
                timer.Dispose();
            }
            
            Timers.Clear();
            Sweep.Clear();
        }
    }
}