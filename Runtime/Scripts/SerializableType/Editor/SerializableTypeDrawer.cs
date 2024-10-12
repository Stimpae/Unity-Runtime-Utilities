using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RuntimeUtilities.SerializableType.Editor {
    [CustomPropertyDrawer(typeof(SerializableType))]
    public class SerializableTypeDrawer : PropertyDrawer{
        private TypeFilterAttribute m_typeFilter;
        private string[] m_typeNames;
        private string[] m_typeFullNames;

        private void Initialize() {
            if (m_typeFullNames != null) return;
            
            m_typeFilter = (TypeFilterAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(TypeFilterAttribute));
            
            var filteredTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => m_typeFilter == null ? DefaultFilter(t) : m_typeFilter.Filter(t))
                .ToArray();
            
            m_typeNames = filteredTypes.Select(t => t.ReflectedType == null ? t.Name : $"t.ReflectedType.Name + t.Name").ToArray();
            m_typeFullNames = filteredTypes.Select(t => t.AssemblyQualifiedName).ToArray();
        }
        
        static bool DefaultFilter(Type type) {
            return !type.IsAbstract && !type.IsInterface && !type.IsGenericType;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Initialize();
            var typeIdProperty = property.FindPropertyRelative("assemblyQualifiedName");

            if (string.IsNullOrEmpty(typeIdProperty.stringValue)) {
                typeIdProperty.stringValue = m_typeFullNames.First();
                property.serializedObject.ApplyModifiedProperties();
            }

            var currentIndex = Array.IndexOf(m_typeFullNames, typeIdProperty.stringValue);
            var selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, m_typeNames);

            if (selectedIndex < 0 || selectedIndex == currentIndex) return;
            typeIdProperty.stringValue = m_typeFullNames[selectedIndex];
            property.serializedObject.ApplyModifiedProperties();
        }
        
    }
}