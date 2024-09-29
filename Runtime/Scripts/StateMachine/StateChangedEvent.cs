using System;
using UnityEngine;

namespace TG.Utilities {
    public struct StateChangedEvent<T> : IEvent where T : struct, IComparable, IConvertible, IFormattable{
        public GameObject owner;
        public StateMachine<T> targetStateMachine;
        public T newState;
        public T previousState;

        public StateChangedEvent(StateMachine<T> stateMachine)
        {
            owner = stateMachine.Owner;
            targetStateMachine = stateMachine;
            newState = stateMachine.CurrentState;
            previousState = stateMachine.PreviousState;
        }
    }
}