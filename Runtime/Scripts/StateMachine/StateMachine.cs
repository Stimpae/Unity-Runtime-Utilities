using System;
using System.Collections.Generic;
using RuntimeUtilities.Timer;
using UnityEngine;

namespace RuntimeUtilities.StateMachine {
    /// <summary>
    /// A generic state machine class that manages state transitions and events.
    /// </summary>
    /// <typeparam name="T">The type of the state, which must be a struct, IComparable, IConvertible, and IFormattable.</typeparam>
    public class StateMachine<T> : IStateMachine where T : struct, IComparable, IConvertible, IFormattable {
        /// <summary>
        /// Gets or sets a value indicating whether events should be triggered on state changes.
        /// </summary>
        public bool ShouldTriggerEvents { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the state machine should update.
        /// </summary>
        public bool ShouldUpdate { get; set; }

        /// <summary>
        /// Gets or sets the owner of the state machine.
        /// </summary>
        public GameObject Owner { get; set; }

        /// <summary>
        /// Gets the current state of the state machine.
        /// </summary>
        public T CurrentState { get; private set; }

        /// <summary>
        /// Gets the previous state of the state machine.
        /// </summary>
        public T PreviousState { get; private set; }

        /// <summary>
        /// Occurs when the state changes.
        /// </summary>
        public event Action<T> OnStateChange;

        private FrequencyTimer m_updateTimer;
        private readonly Dictionary<T, State> m_states = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner of the state machine.</param>
        public StateMachine(GameObject owner) {
            Owner = owner;
        }

        /// <summary>
        /// Enables event triggering for the state machine.
        /// </summary>
        /// <returns>The current instance of the state machine.</returns>
        public StateMachine<T> WithTriggerEvents() {
            ShouldTriggerEvents = true;
            return this;
        }

        /// <summary>
        /// Enables updates for the state machine and sets the initial state.
        /// </summary>
        /// <param name="state">The initial state.</param>
        /// <param name="stateObject">The state object associated with the initial state.</param>
        /// <returns>The current instance of the state machine.</returns>
        public StateMachine<T> WithUpdates(T state, State stateObject) {
            ShouldUpdate = true;
            CurrentState = state;
            AddState(CurrentState, stateObject);
            m_states[CurrentState].OnEnter();
            m_updateTimer ??= new FrequencyTimer(30);
            m_updateTimer.OnTick += () => m_states[CurrentState].OnUpdate();
            m_updateTimer.Start();
            return this;
        }

        /// <summary>
        /// Adds a state to the state machine.
        /// </summary>
        /// <param name="state">The state to add.</param>
        /// <param name="stateObject">The state object associated with the state.</param>
        /// <returns>The current instance of the state machine.</returns>
        public StateMachine<T> WithState(T state, State stateObject) {
            AddState(state, stateObject);
            if (EqualityComparer<T>.Default.Equals(state, CurrentState)) m_states[state].OnEnter();
            return this;
        }

        /// <summary>
        /// Adds a state to the state machine.
        /// </summary>
        /// <param name="state">The state to add.</param>
        /// <param name="stateObject">The state object associated with the state.</param>
        public void AddState(T state, State stateObject) {
            if (!m_states.TryAdd(state, stateObject)) {
                Debug.LogWarning($"State {state} already exists in the state machine.");
            }
        }

        /// <summary>
        /// Changes the current state of the state machine.
        /// </summary>
        /// <param name="newState">The new state to transition to.</param>
        public virtual void ChangeState(T newState) {
            if (EqualityComparer<T>.Default.Equals(newState, CurrentState)) return;

            PreviousState = CurrentState;
            CurrentState = newState;
            OnStateChange?.Invoke(newState);

            if (m_states.Count != 0) {
                if (m_states.TryGetValue(PreviousState, out var previousState)) {
                    previousState.OnExit();
                    if (ShouldUpdate) m_updateTimer.OnTick -= previousState.OnUpdate;
                }

                if (m_states.TryGetValue(CurrentState, out var currentState)) {
                    currentState.OnEnter();
                }
            }

            if (ShouldTriggerEvents) {
                EventBus.EventBus<StateChangedEvent<T>>.Raise(new StateChangedEvent<T>(this));
            }
        }
    }
}