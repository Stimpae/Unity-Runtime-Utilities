using System;
using UnityEngine;

namespace RuntimeUtilities.Timer {
    /// <summary>
    /// Timer that ticks at a specific frequency. (N times per second)
    /// </summary>
    public class FrequencyTimer : Timer {
        public int TicksPerSecond { get; private set; }

        public event Action OnTick;
        private float m_timeThreshold;

        public FrequencyTimer(int ticksPerSecond, Action onTick = null) : base(0) {
            CalculateTimeThreshold(ticksPerSecond);
            if (onTick != null) OnTick = onTick;
        }

        public override void Tick() {
            if (IsRunning && CurrentTime >= m_timeThreshold) {
                CurrentTime -= m_timeThreshold;
                OnTick.Invoke();
            }

            if (IsRunning && CurrentTime < m_timeThreshold) {
                CurrentTime += Time.deltaTime;
            }
        }

        public override bool IsFinished => !IsRunning;

        public override void Reset() {
            CurrentTime = 0;
        }

        public void Reset(int newTicksPerSecond) {
            CalculateTimeThreshold(newTicksPerSecond);
            Reset();
        }

        void CalculateTimeThreshold(int ticksPerSecond) {
            TicksPerSecond = ticksPerSecond;
            m_timeThreshold = 1f / TicksPerSecond;
        }
    }
}