using UnityEngine;

namespace RuntimeUtilities.StateMachine {
    public interface IState {
        public GameObject Owner { get; set; }
        public void OnEnter() {}
        public void OnExit() {}
        public void OnUpdate() {}
    }
}