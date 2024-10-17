using System;

namespace RuntimeUtilities.EventBus {
    public interface IEvent {
        
    }
    
    [Serializable]
    public struct ExampleEvent : IEvent {
        public int exampleValue;
    }
}