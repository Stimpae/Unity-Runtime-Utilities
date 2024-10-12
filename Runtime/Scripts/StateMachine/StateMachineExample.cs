using System;
using UnityEngine;

namespace RuntimeUtilities.StateMachine {
    public enum TestingEnum {
        A,
        B,
        C,
    }
    
    
    public class StateMachineExample : MonoBehaviour {
        private StateMachine<TestingEnum> m_stateMachine;
        private void Awake() {
            m_stateMachine = new StateMachine<TestingEnum>(this.gameObject)
                .WithTriggerEvents()
                .WithState(TestingEnum.A, new StateOne())
                .WithState(TestingEnum.B, new StateTwo())
                .WithState(TestingEnum.C, new StateThree());
        }
        
        [ContextMenu("Enter State One")]
        public void EnterStateOne() {
            m_stateMachine.ChangeState(TestingEnum.A);
        }
        
        [ContextMenu("Enter State Two")]
        public void EnterStateTwo() {
            m_stateMachine.ChangeState(TestingEnum.B);
        }
        
        [ContextMenu("Enter State Three")]
        public void EnterStateThree() {
            m_stateMachine.ChangeState(TestingEnum.C);
        }

        public class StateOne : State {
            public override void OnEnter() {
                Debug.Log("State One Entered");
            }

            public override void OnExit() {
                Debug.Log("State One Exited");
            }

            public override void OnUpdate() {
                Debug.Log("State One Updated");
            }
        }
        
        public class StateTwo : State {
            public override void OnEnter() {
                Debug.Log("State Two Entered");
            }

            public override void OnExit() {
                Debug.Log("State Two Exited");
            }

            public override void OnUpdate() {
                Debug.Log("State Two Updated");
            }
        }
        
        public class StateThree : State {
            public override void OnEnter() {
                Debug.Log("State Three Entered");
            }

            public override void OnExit() {
                Debug.Log("State Three Exited");
            }

            public override void OnUpdate() {
                Debug.Log("State Three Updated");
            }
        }
        
        
    }
}