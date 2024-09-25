using System;
using System.Collections;
using UnityEngine;

namespace TG.Utilities {
    /// <summary>
    /// This class is used to provide a callback to objects that are pooled.
    /// It is added automatically to objects that are pooled
    /// </summary>
    [DisallowMultipleComponent]
    public class PoolableObject : MonoBehaviour {
        private IPoolCallbackReceiver[] m_poolCallbackReceivers = Array.Empty<IPoolCallbackReceiver>();
        
        private void Awake() {
            m_poolCallbackReceivers = GetComponents<IPoolCallbackReceiver>();
        }
        
        public void OnReuse(float lifeTime) {
            if (lifeTime > 0) {
                StartCoroutine(LifeTimeCoroutine(lifeTime));
            }
            
            if (m_poolCallbackReceivers.Length == 0) return;
            foreach (var receiver in m_poolCallbackReceivers) {
                receiver.OnReuse();
            }
        }

        private IEnumerator LifeTimeCoroutine(float lifeTime) {
            yield return new WaitForSeconds(lifeTime);
            gameObject.Release();
        }

        public void OnRelease() {
            if (m_poolCallbackReceivers.Length == 0) return;
            foreach (var receiver in m_poolCallbackReceivers) {
                receiver.OnRelease();
            }
        }
    }
}