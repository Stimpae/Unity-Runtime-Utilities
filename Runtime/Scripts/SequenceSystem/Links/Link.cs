using EditorUtilities.Attributes;
using UnityEngine;

namespace RuntimeUtilities.Sequences {
    public abstract class Link : ScriptableObject {
        [InfoBox("This is a link in a sequence. It is used to connect two sequences together.")]
        [Space]
        [ScriptableObject] public Sequence sequence;

        public abstract bool Validate(out Sequence nextSequence);
        public abstract void Enable();
        public abstract void Disable();
    }
}