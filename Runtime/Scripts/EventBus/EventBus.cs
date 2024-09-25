using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace TG.Utilities {
    public static class EventBus<T> where T : IEvent {
        //in this case we specifically _want_ a static member in a generic type. We want a different bindings-hashSet in each instance of different close constructed type (https://www.jetbrains.com/help/resharper/StaticMemberInGenericType.html)
        // ReSharper disable once StaticMemberInGenericType
        private static readonly HashSet<Action> BindingsWithoutArguments = new();
        private static readonly HashSet<Action<T>> BindingsWithArguments = new();
    
        public static void Register(Action<T> binding) => BindingsWithArguments.Add(binding);
        public static void Register(Action binding) => BindingsWithoutArguments.Add(binding);

        public static void DeRegister(Action<T> binding) => BindingsWithArguments.Remove(binding);
        public static void DeRegister(Action binding) => BindingsWithoutArguments.Remove(binding);

        public static void Raise(T @event) {
            foreach (var binding in BindingsWithArguments) {
                binding?.Invoke(@event);
            }

            foreach (var binding in BindingsWithoutArguments) {
                binding?.Invoke();
            }
        }

        /// <summary>Remove all listeners.</summary>
        [UsedImplicitly] //used via reflection in EventBusUtilities.ClearAllBuses()
        public static void Clear() {
            Debug.Log($"Clearing {typeof(T).Name} bindings (removing all listeners)");
            BindingsWithArguments.Clear();
            BindingsWithoutArguments.Clear();
        }
    }
}