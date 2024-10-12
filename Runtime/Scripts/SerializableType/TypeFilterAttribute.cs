using System;
using RuntimeUtilities.Extensions;
using UnityEngine;

namespace RuntimeUtilities.SerializableType {
    public class TypeFilterAttribute : PropertyAttribute {
        public Func<Type, bool> Filter { get; }
        public TypeFilterAttribute(Type filterType) {
            Filter = type => !type.IsAbstract &&
                             !type.IsInterface &&
                             !type.IsGenericType &&
                             type.InheritsOrImplements(filterType);
        }
    }
}