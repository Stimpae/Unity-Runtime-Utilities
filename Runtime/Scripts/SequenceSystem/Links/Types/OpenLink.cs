using UnityEngine;

namespace RuntimeUtilities.Sequences.Types {
    [CreateAssetMenu(menuName = "Data/Sequences/Links/Open Link", fileName = "OpenLink_Data")]
    public class OpenLink : Link {
        public override bool Validate(out Sequence nextSequence) {
            nextSequence = base.sequence;
            return true;
        }

        public override void Enable() {
        }

        public override void Disable() {
        }
    }
}