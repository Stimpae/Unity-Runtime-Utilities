using System;
using UnityEngine;

namespace TG.Utilities {
    /// <summary>
    /// Abstract base class for a timer that can be started, stopped, and reset.
    /// </summary>
    public abstract class Timer : IDisposable {
        /// <summary>
        /// Gets the current time of the timer.
        /// </summary>
        public float CurrentTime { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the timer is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// The initial time set for the timer.
        /// </summary>
        protected float initialTime;

        /// <summary>
        /// Gets the progress of the timer as a value between 0 and 1.
        /// </summary>
        public float Progress => Mathf.Clamp(CurrentTime / initialTime, 0, 1);

        /// <summary>
        /// Event triggered when the timer starts.
        /// </summary>
        public Action OnTimerStart = delegate { };

        /// <summary>
        /// Event triggered when the timer stops.
        /// </summary>
        public Action OnTimerStop = delegate { };

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class with the specified initial time.
        /// </summary>
        /// <param name="value">The initial time for the timer.</param>
        protected Timer(float value) {
            initialTime = value;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start() {
            CurrentTime = initialTime;
            if (IsRunning) return;
            IsRunning = true;
            TimerManager.RegisterTimer(this);
            OnTimerStart.Invoke();
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop() {
            if (!IsRunning) return;
            IsRunning = false;
            TimerManager.DeregisterTimer(this);
            OnTimerStop.Invoke();
        }

        /// <summary>
        /// Updates the timer. Must be implemented by derived classes.
        /// </summary>
        public abstract void Tick();

        /// <summary>
        /// Gets a value indicating whether the timer has finished. Must be implemented by derived classes.
        /// </summary>
        public abstract bool IsFinished { get; }

        /// <summary>
        /// Resumes the timer.
        /// </summary>
        public void Resume() => IsRunning = true;

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        public void Pause() => IsRunning = false;

        /// <summary>
        /// Resets the timer to the initial time.
        /// </summary>
        public virtual void Reset() => CurrentTime = initialTime;

        /// <summary>
        /// Resets the timer to a new specified time.
        /// </summary>
        /// <param name="newTime">The new time to set for the timer.</param>
        public virtual void Reset(float newTime) {
            initialTime = newTime;
            Reset();
        }

        #region Disposal
        private bool m_disposed;
        /// <summary>
        /// Finalizes an instance of the <see cref="Timer"/> class.
        /// </summary>
        ~Timer() {
            Dispose(false);
        }

        /// <summary>
        /// Disposes the timer and releases resources.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the timer and releases resources.
        /// </summary>
        /// <param name="disposing">Indicates whether the method is called from Dispose or the finalizer.</param>
        protected virtual void Dispose(bool disposing) {
            if (m_disposed) return;
            if (disposing) {
                TimerManager.DeregisterTimer(this);
            }
            m_disposed = true;
        }
        #endregion
    }
}