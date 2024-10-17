namespace RuntimeUtilities.EventBus {
    /// <summary>
    /// 
    /// </summary>
    public static class EventExtensions {
        public static void Register<T>(this T @event, System.Action<T> binding) where T : IEvent {
            EventBus<T>.Register(binding);
        }
        
        public static void Raise<T>(this T @event) where T : IEvent {
            EventBus<T>.Raise(@event);
        }
        
        public static void DeRegister<T>(this T @event, System.Action<T> binding) where T : IEvent {
            EventBus<T>.DeRegister(binding);
        }
        
        public static void Clear<T>(this T @event) where T : IEvent {
            EventBus<T>.Clear();
        }
    }
}