using System.Collections;
using EditorUtilities.Attributes;
using RuntimeUtilities.EventBus;
using UnityEngine;

namespace RuntimeUtilities.Sequences.Types {
    [CreateAssetMenu(menuName = "Data/Sequences/Sequence/Delay", fileName = "DelaySequence_Data")]
    public class DelaySequence : Sequence {
        [BoxGroup("Sequence Settings", 0)] public float delay = 1f;

        public override IEnumerator OnExecute() {
            yield return new WaitForSeconds(delay);
            onExecuteEvent.Raise();
        }
    }
}