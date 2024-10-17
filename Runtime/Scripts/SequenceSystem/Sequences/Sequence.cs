
using System;
using System.Collections;
using System.Collections.Generic;
using EditorUtilities.Attributes;
using RuntimeUtilities.EventBus;
using UnityEngine;
using UnityEngine.Serialization;

namespace RuntimeUtilities.Sequences{
    /// <summary>
    /// Base class for all sequences
    /// </summary>
    public abstract class Sequence : ScriptableObject {
        [InfoBox("A sequence is a collection of actions that can be executed in a specific order. " +
                 "Each sequence can have links to other sequences that are executed when the current sequence is completed.")]
        [Space]
        public string sequenceName;
        [Title("Links", subtitle: "(Assign links to other sequences)",showLine: true)] [Holder] public DecoratorHolder holder;
        [ListView] public List<Link> links = new List<Link>();
        
        [Title("Preload Objects", subtitle: "(Load objects when this sequences is entered)", showLine: true)]
        public bool clearOnExit = true;
        [ListView] public List<GameObject> preloadObjects = new List<GameObject>();
        
        [Splitter(padding: 10)]
        [Holder] public DecoratorHolder splitHolder;
        
        [BoxGroup("Sequence Events", 0),SerializeReference, SubclassSelector]public IEvent onEnterEvent;
        [BoxGroup("Sequence Events", 0),SerializeReference, SubclassSelector]public IEvent onExitEvent;
        [BoxGroup("Sequence Events", 0),SerializeReference, SubclassSelector]public IEvent onExecuteEvent;
        
        readonly List<GameObject> m_spawnedObjects = new List<GameObject>();
        
        public virtual void OnEnter() {
            onEnterEvent?.Raise();
            
            if (preloadObjects.Count == 0) return;
            foreach (var sequenceObject in preloadObjects) {
                m_spawnedObjects.Add(Instantiate(sequenceObject));
            }
        }

        public abstract IEnumerator OnExecute();

        public virtual void OnExit() {
            onExitEvent?.Raise();
            if (!clearOnExit) return;
            if(m_spawnedObjects.Count == 0) return;
            
            foreach (var spawnedObject in m_spawnedObjects) {
                Destroy(spawnedObject);
            }
            m_spawnedObjects.Clear();
        }

        public virtual void AddLink(Link link) {
            links.Add(link);
        }

        public virtual void RemoveLink(Link link) {
            links.Remove(link);
        }

        public virtual void RemoveAllLinks() {
            links.Clear();
        }

        public bool ValidateLinks(out Sequence nextSequence) {
            if (links.Count == 0) {
                nextSequence = null;
                return false;
            }
            
            nextSequence = null;
            foreach (var link in links) {
                if (link.Validate(out nextSequence)) {
                    return true;
                }
            }
            return false;
        }

        public virtual void EnableLinks() {
            foreach (var link in links) {
                link.Enable();
            }
        }

        public virtual void DisableLinks() {
            foreach (var link in links) {
                link.Disable();
            }
        }
    }
}