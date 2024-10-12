using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RuntimeUtilities.ObjectPooling {
    /// <summary>
    /// Manages a pool of reusable GameObjects to optimize performance by reducing the need for frequent instantiation and destruction.
    /// </summary>
    public class ObjectPool {
        private GameObject m_original;
        private PoolableObject m_originalPoolableObject;
        
        private Stack<PoolableObject> m_instances = new Stack<PoolableObject>(INITIAL_SIZE);
        private List<GameObject> m_allInstances = new List<GameObject>(INITIAL_SIZE);
        private bool m_prototypeIsNotSource;

        private static readonly Dictionary<GameObject, ObjectPool> PrefabLookup = new Dictionary<GameObject, ObjectPool>(64);
        private static readonly Dictionary<GameObject, ObjectPool> InstanceLookup = new Dictionary<GameObject, ObjectPool>(512);
        private static readonly Dictionary<GameObject, GameObject> ParentLookup = new Dictionary<GameObject, GameObject>(512);
        
        private const int INITIAL_SIZE = 128;

        /// <summary>
        /// Initializes a new instance of the ObjectPool class.
        /// </summary>
        /// <param name="prefab">The prefab to be pooled.</param>
        private ObjectPool(GameObject prefab) {
            m_original = prefab;
            m_originalPoolableObject = prefab.GetComponent<PoolableObject>() ?? CreatePoolableObject(prefab);
            PrefabLookup[m_original] = this;
        }

        /// <summary>
        /// Creates a poolable object instance of the prefab.
        /// </summary>
        /// <param name="prefab">The prefab to create a poolable object from.</param>
        /// <returns>The created PoolableObject.</returns>
        private PoolableObject CreatePoolableObject(GameObject prefab) {
            var prototype = Object.Instantiate(prefab).AddComponent<PoolableObject>();
            Object.DontDestroyOnLoad(prototype);
            prototype.gameObject.SetActive(false);
            m_prototypeIsNotSource = true;
            return prototype;
        }

        /// <summary>
        /// Gets the ObjectPool associated with the specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab to get the pool for.</param>
        /// <param name="create">Whether to create a new pool if one does not exist.</param>
        /// <returns>The ObjectPool associated with the prefab.</returns>
        public static ObjectPool GetPoolByPrefab(GameObject prefab, bool create = true) {
            return PrefabLookup.TryGetValue(prefab, out var pool) ? pool : create ? new ObjectPool(prefab) : null;
        }

        /// <summary>
        /// Gets the ObjectPool associated with the specified instance.
        /// </summary>
        /// <param name="instance">The instance to get the pool for.</param>
        /// <param name="pool">The ObjectPool associated with the instance.</param>
        /// <returns>True if the pool was found; otherwise, false.</returns>
        public static bool GetPoolByInstance(GameObject instance, out ObjectPool pool) {
            return InstanceLookup.TryGetValue(instance, out pool);
        }

        /// <summary>
        /// Removes the specified instance from the pool.
        /// </summary>
        /// <param name="instance">The instance to remove.</param>
        public static void Remove(GameObject instance) {
            InstanceLookup.Remove(instance);
        }

        /// <summary>
        /// Populates the pool with a specified number of instances.
        /// </summary>
        /// <param name="count">The number of instances to populate the pool with.</param>
        public void Populate(int count) {
            var parent = new GameObject("[Object Pool] " + m_original.name);
            ParentLookup[m_original] = parent;
            
            for (var i = 0; i < count; i++) {
                var instance = CreateInstance();
                instance.gameObject.SetActive(false);
                instance.transform.SetParent(parent.transform);
                m_instances.Push(instance);
            }
        }

        /// <summary>
        /// Clears the pool, optionally destroying active instances.
        /// </summary>
        /// <param name="destroyActive">Whether to destroy active instances.</param>
        public void Clear(bool destroyActive) {
            PrefabLookup.Remove(m_original);
            ParentLookup.Remove(m_original);
            foreach (var instance in m_allInstances) {
                if (instance == null) continue;
                InstanceLookup.Remove(instance);
                if (!destroyActive && instance.activeInHierarchy) continue;
                Object.Destroy(instance);
            }
            if (m_prototypeIsNotSource) Object.Destroy(m_originalPoolableObject.gameObject);
            m_original = null;
            m_originalPoolableObject = null;
            m_instances = null;
            m_allInstances = null;
        }

        /// <summary>
        /// Reuses an instance from the pool with a specified lifetime.
        /// </summary>
        /// <param name="lifeTime">The lifetime of the reused instance.</param>
        /// <returns>The reused GameObject.</returns>
        public GameObject Reuse(float lifeTime) => GetInstance(lifeTime).gameObject;

        /// <summary>
        /// Reuses an instance from the pool, setting its parent and lifetime.
        /// </summary>
        /// <param name="parent">The parent transform to set.</param>
        /// <param name="lifeTime">The lifetime of the reused instance.</param>
        /// <returns>The reused GameObject.</returns>
        public GameObject Reuse(Transform parent, float lifeTime) {
            var instance = GetInstance(lifeTime);
            instance.transform.SetParent(parent);
            return instance.gameObject;
        }
        
        /// <summary>
        /// Reuses an instance from the pool, setting its parent, world position stays flag, and lifetime.
        /// </summary>
        /// <param name="parent">The parent transform to set.</param>
        /// <param name="worldPositionStays">Whether to maintain the world position of the instance.</param>
        /// <param name="lifeTime">The lifetime of the reused instance.</param>
        /// <returns>The reused GameObject.</returns>
        public GameObject Reuse(Transform parent, bool worldPositionStays, float lifeTime) {
            var instance = GetInstance(lifeTime);
            instance.transform.SetParent(parent, worldPositionStays);
            return instance.gameObject;
        }

        /// <summary>
        /// Reuses an instance from the pool, setting its position, rotation, and lifetime.
        /// </summary>
        /// <param name="position">The position to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="lifeTime">The lifetime of the reused instance.</param>
        /// <returns>The reused GameObject.</returns>
        public GameObject Reuse(Vector3 position, Quaternion rotation, float lifeTime) {
            var instance = GetInstance(lifeTime);
            instance.transform.SetPositionAndRotation(position, rotation);
            return instance.gameObject;
        }

        /// <summary>
        /// Reuses an instance from the pool, setting its position, rotation, parent, and lifetime.
        /// </summary>
        /// <param name="position">The position to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="parent">The parent transform to set.</param>
        /// <param name="lifeTime">The lifetime of the reused instance.</param>
        /// <returns>The reused GameObject.</returns>
        public GameObject Reuse(Vector3 position, Quaternion rotation, Transform parent, float lifeTime) {
            var instance = GetInstance(lifeTime);
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.transform.SetParent(parent);
            return instance.gameObject;
        }
        
        /// <summary>
        /// Releases an instance back to the pool.
        /// </summary>
        /// <param name="instance">The instance to release.</param>
        public void Release(GameObject instance) {
            var poolable = instance.GetComponent<PoolableObject>();
            instance.SetActive(false);
            poolable.OnRelease();
            ResetTransform(instance);
            m_instances.Push(poolable);
        }

        /// <summary>
        /// Resets the transform of the specified instance.
        /// </summary>
        /// <param name="instance">The instance to reset.</param>
        private void ResetTransform(GameObject instance) {
            if (ParentLookup.TryGetValue(instance, out var parent)) {
                instance.transform.SetParent(parent.transform);
            }
        }

        /// <summary>
        /// Gets an instance from the pool, setting its lifetime.
        /// </summary>
        /// <param name="lifeTime">The lifetime of the instance.</param>
        /// <returns>The PoolableObject instance.</returns>
        private PoolableObject GetInstance(float lifeTime) {
            while (m_instances.Count > 0) {
                var instance = m_instances.Pop();
                if (instance != null) {
                    instance.gameObject.SetActive(true);
                    instance.OnReuse(lifeTime);
                    return instance;
                }
            }
            return CreateInstance();
        }

        /// <summary>
        /// Creates a new instance of the pooled object.
        /// </summary>
        /// <returns>The created PoolableObject instance.</returns>
        private PoolableObject CreateInstance() {
            var instance = Object.Instantiate(m_originalPoolableObject);
            InstanceLookup[instance.gameObject] = this;
            m_allInstances.Add(instance.gameObject);
            return instance;
        }
    }
}