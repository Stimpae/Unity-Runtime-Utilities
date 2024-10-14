using System;
using System.Collections.Generic;
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

        private readonly Dictionary<T, IState> m_states = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner of the state machine.</param>
        public StateMachine(GameObject owner) {
            Owner = owner;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void UpdateState() {
            if (m_states.TryGetValue(CurrentState, out var state)) state.OnUpdate();
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
        /// Adds a state to the state machine.
        /// </summary>
        /// <param name="state">The state to add.</param>
        /// <param name="stateObject">The state object associated with the state.</param>
        public void AddState(T state, IState stateObject) {
            if (m_states.TryAdd(state, stateObject)) stateObject.Owner = Owner;
            else Debug.LogWarning($"State {state} already exists in the state machine.");
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
                if (m_states.TryGetValue(PreviousState, out var previousState)) previousState.OnExit();
                if (m_states.TryGetValue(CurrentState, out var currentState)) currentState.OnEnter();
            }

            if (ShouldTriggerEvents) EventBus.EventBus<StateChangedEvent<T>>.Raise(new StateChangedEvent<T>(this));
        }
    }
}