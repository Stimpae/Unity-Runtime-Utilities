using System;using System.Collections;
using UnityEngine;
using RuntimeUtilities.StateMachine;

namespace RuntimeUtilities.Sequences {
    /// <summary>
    /// 
    /// </summary>
    public class SequenceRunner {
        public Sequence CurrentSequence { get; private set; }
        
        private Coroutine m_currentPlayCoroutine;
        private bool m_playLock;
        private Coroutine m_loopCoroutine;

        protected virtual void SetCurrentState(Sequence sequence) {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (CurrentSequence != null && m_currentPlayCoroutine != null) Skip();
            CurrentSequence = sequence;
            CoroutineUtility.StartCoroutine(Play());
        }

        private IEnumerator Play() {
            if (m_playLock) yield break;
            m_playLock = true;
            
            CurrentSequence.OnEnter();
            m_currentPlayCoroutine = CoroutineUtility.StartCoroutine(CurrentSequence.OnExecute());
            yield return m_currentPlayCoroutine;
            m_currentPlayCoroutine = null;
        }

        private void Skip() {
            if (CurrentSequence == null) throw new Exception($"{nameof(CurrentSequence)} is null!");
            if (m_currentPlayCoroutine == null) return;
            
            CoroutineUtility.StopCoroutine(ref m_currentPlayCoroutine);
            CurrentSequence.OnExit();
            m_currentPlayCoroutine = null;
            m_playLock = false;
        }

        public virtual void Run(Sequence sequence) {
            SetCurrentState(sequence);
            Run();
        }

        protected virtual void Run() {
            if (m_loopCoroutine != null) return;
            m_loopCoroutine = CoroutineUtility.StartCoroutine(Loop());
        }

        public void Stop() {
            if (m_loopCoroutine == null) return;
            if (CurrentSequence != null && m_currentPlayCoroutine != null) Skip();
            CoroutineUtility.StopCoroutine(ref m_loopCoroutine);
            CurrentSequence = null;
        }

        protected virtual IEnumerator Loop() {
            while (true) {
                if (CurrentSequence != null && m_currentPlayCoroutine == null) {
                    if (CurrentSequence.ValidateLinks(out var nextState)) {
                        if (m_playLock) {
                            CurrentSequence.OnExit();
                            m_playLock = false;
                        }
                        CurrentSequence.DisableLinks();
                        SetCurrentState(nextState);
                        CurrentSequence.EnableLinks();
                    }
                }
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public bool IsRunning => m_loopCoroutine != null;
    }
}