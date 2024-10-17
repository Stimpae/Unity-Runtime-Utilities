using System;
using EditorUtilities.Attributes;
using RuntimeUtilities.EventBus;
using UnityEngine;
using UnityEngine.Serialization;

namespace RuntimeUtilities.Sequences.Types {
    [CreateAssetMenu(menuName = "Data/Sequences/Links/Event Link", fileName = "EventLink_Data")]
    public class EventLink : Link {
        [Title("Listener Event", showLine: true),SerializeReference, SubclassSelector] public IEvent @event;

        private bool m_eventTriggered;

        public override bool Validate(out Sequence nextSequence) {
            nextSequence = m_eventTriggered ? base.sequence : null;
            return m_eventTriggered;
        }

        public override void Enable() {
            @event.Register(OnEventTriggered);
        }

        public override void Disable() {
            @event.DeRegister(OnEventTriggered);
        }

        private void OnEventTriggered(IEvent newEvent) => m_eventTriggered = true;
    }
}