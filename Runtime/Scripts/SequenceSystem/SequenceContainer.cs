using System.Collections.Generic;
using EditorUtilities.Attributes;
using UnityEngine;

namespace RuntimeUtilities.Sequences {
    /// <summary>
    /// A sequence runner takes a collection of sequences and manages the state of the game based on observed game events
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Sequences/Sequence Container", fileName = "SequenceContainer_Data")]
    public class SequenceContainer : ScriptableObject {
        [InfoBox("The sequence container is used to manage the state of the game based on observed game events" +
                 "Make sure to initialize the sequence runner when the game starts")]
        [Space]
        [ScriptableObject] public Sequence initialSequence;
        private readonly SequenceRunner m_runner = new();
        
        public void Initialize() {
            if (initialSequence != null) m_runner.Run(initialSequence);
        }
        
        public Sequence GetCurrentSequence() {
            return m_runner.CurrentSequence;
        }
    }
}