using System.Collections.Generic;
using System.Linq;
using EditorUtilities.Attributes.Editor;
using UnityEditor;
using UnityEngine;

namespace RuntimeUtilities.Sequences.Editor {
    [CustomEditor(typeof(SequenceContainer))]
    public class SequenceContainerEditor : AttributeEditor {
        private SequenceContainer m_container;
        private Dictionary<Sequence, bool> foldoutStates = new Dictionary<Sequence, bool>();

        protected override void OnEnable() {
            base.OnEnable();
            m_container = target as SequenceContainer;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            // Header with debugging as the title
            EditorGUILayout.LabelField("Debugging", EditorStyles.boldLabel);
            AttributeEditorStyles.DrawSplitter();

            EditorGUILayout.Space();
            if (!Application.isPlaying) {
                EditorGUILayout.HelpBox("Debugging information will only show when the game is running",
                    MessageType.Warning);
                return;
            }
            
            var currentSequence = m_container.GetCurrentSequence();
            if (currentSequence != null) {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Current Sequence",
                    currentSequence.name + " -> " + currentSequence.sequenceName);
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Sequence Hierarchy", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginVertical("box");
            if (m_container.initialSequence != null) {
                DrawSequenceHierarchy(m_container.initialSequence, 0);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawSequenceHierarchy(Sequence sequence, int indentLevel) {
            if (sequence == null) return;
            EditorGUI.indentLevel = indentLevel;
            
            foldoutStates.TryAdd(sequence, true); // Default to open
            bool isCurrentSequence = m_container.GetCurrentSequence() == sequence;
            GUI.color = isCurrentSequence ? Color.green : Color.white;
            
            string sequenceLabel = $"{sequence.sequenceName} (Type: {sequence.GetType().Name})";
            foldoutStates[sequence] = EditorGUILayout.Foldout(foldoutStates[sequence], sequenceLabel);
            
            GUI.color = Color.white;

            if (!foldoutStates[sequence]) return;
            foreach (var link in sequence.links.Where(link => link != null && link.sequence != null)) {
                bool isCurrentLink = m_container.GetCurrentSequence() == link.sequence;
                GUI.color = isCurrentLink ? Color.green : Color.white;

                string linkLabel = $"Link -> {link.sequence.sequenceName} (Link Type: {link.GetType().Name})";
                EditorGUILayout.LabelField(linkLabel);

                DrawSequenceHierarchy(link.sequence, indentLevel + 1);
                GUI.color = Color.white;
            }
        }
    }
}