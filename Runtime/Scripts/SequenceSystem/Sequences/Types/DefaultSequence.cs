using System.Collections;
using RuntimeUtilities.EventBus;
using UnityEditor;
using UnityEngine;

namespace RuntimeUtilities.Sequences.Types {
    [CreateAssetMenu(menuName = "Data/Sequences/Sequence/Default", fileName = "DefaultSequence_Data")]
    public class DefaultSequence : Sequence{
        public override IEnumerator OnExecute() {
            yield return null;
            onExecuteEvent.Raise();
        }
    }
}