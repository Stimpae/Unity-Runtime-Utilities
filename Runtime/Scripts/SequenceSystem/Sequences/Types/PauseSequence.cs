using System.Collections;
using RuntimeUtilities.EventBus;
using UnityEngine;

namespace RuntimeUtilities.Sequences.Types {
    [CreateAssetMenu(menuName = "Data/Sequences/Sequence/Pause", fileName = "PauseSequence_Data")]
    public class PauseSequence : Sequence {
        public override void OnEnter() {
            base.OnEnter();
            Time.timeScale = 0;
        }

        public override IEnumerator OnExecute() {
            yield return null;
            onExecuteEvent.Raise();
        }

        public override void OnExit() {
            base.OnExit();
            Time.timeScale = 1;
        }
    }
}