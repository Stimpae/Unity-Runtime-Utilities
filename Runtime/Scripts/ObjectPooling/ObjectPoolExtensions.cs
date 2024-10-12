using UnityEngine;

namespace RuntimeUtilities.ObjectPooling {
    /// <summary>
    /// Provides extension methods for the ObjectPool class to manage pooling operations on GameObjects.
    /// </summary>
    public static class ObjectPoolExtensions {
        /// <summary>
        /// Populates the pool with a specified number of instances of the prefab.
        /// </summary>
        /// <param name="prefab">The prefab to populate the pool with.</param>
        /// <param name="count">The number of instances to create.</param>
        public static void Populate(this GameObject prefab, int count) {
            ObjectPool.GetPoolByPrefab(prefab)?.Populate(count);
        }

        /// <summary>
        /// Clears the pool associated with the prefab, optionally destroying active instances.
        /// </summary>
        /// <param name="prefab">The prefab whose pool is to be cleared.</param>
        /// <param name="destroyActive">Whether to destroy active instances.</param>
        public static void Clear(this GameObject prefab, bool destroyActive) {
            ObjectPool.GetPoolByPrefab(prefab, false)?.Clear(destroyActive);
        }

        /// <summary>
        /// Reuses an instance from the pool with an optional lifetime.
        /// </summary>
        /// <param name="prefab">The prefab to reuse an instance of.</param>
        /// <param name="lifeTime">The lifetime of the reused instance.</param>
        /// <returns>The reused GameObject instance.</returns>
        public static GameObject Reuse(this GameObject prefab, float lifeTime = 0) {
            return ObjectPool.GetPoolByPrefab(prefab)?.Reuse(lifeTime);
        }

        /// <summary>
        /// Reuses an instance from the pool, setting its parent and optional lifetime.
        /// </summary>
        /// <param name="prefab">The prefab to reuse an instance of.</param>
        /// <param name="parent">The parent transform to set.</param>
        /// <param name="lifeTime">The lifetime of the reused instance.</param>
        /// <returns>The reused GameObject instance.</returns>
        public static GameObject Reuse(this GameObject prefab, Transform parent, float lifeTime = 0) {
            return ObjectPool.GetPoolByPrefab(prefab)?.Reuse(parent, lifeTime);
        }

        /// <summary>
        /// Reuses an instance from the pool, setting its parent, world position stays flag, and optional lifetime.
        /// </summary>
        /// <param name="prefab">The prefab to reuse an instance of.</param>
        /// <param name="parent">The parent transform to set.</param>
        /// <param name="worldPositionStays">Whether to maintain the world position of the instance.</param>
        /// <param name="lifeTime">The lifetime of the reused instance.</param>
        /// <returns>The reused GameObject instance.</returns>
        public static GameObject Reuse(this GameObject prefab, Transform parent, bool worldPositionStays, float lifeTime = 0) {
            return ObjectPool.GetPoolByPrefab(prefab)?.Reuse(parent, worldPositionStays, lifeTime);
        }

        /// <summary>
        /// Reuses an instance from the pool, setting its position, rotation, and optional lifetime.
        /// </summary>
        /// <param name="prefab">The prefab to reuse an instance of.</param>
        /// <param name="position">The position to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="lifeTime">The lifetime of the reused instance.</param>
        /// <returns>The reused GameObject instance.</returns>
        public static GameObject Reuse(this GameObject prefab, Vector3 position, Quaternion rotation, float lifeTime = 0) {
            return ObjectPool.GetPoolByPrefab(prefab)?.Reuse(position, rotation, lifeTime);
        }

        /// <summary>
        /// Reuses an instance from the pool, setting its position, rotation, parent, and optional lifetime.
        /// </summary>
        /// <param name="prefab">The prefab to reuse an instance of.</param>
        /// <param name="position">The position to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="parent">The parent transform to set.</param>
        /// <param name="lifeTime">The lifetime of the reused instance.</param>
        /// <returns>The reused GameObject instance.</returns>
        public static GameObject Reuse(this GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, float lifeTime = 0) {
            return ObjectPool.GetPoolByPrefab(prefab)?.Reuse(position, rotation, parent, lifeTime);
        }

        /// <summary>
        /// Releases an instance back to the pool or destroys it if it is not pooled.
        /// </summary>
        /// <param name="instance">The instance to release.</param>
        public static void Release(this GameObject instance) {
            if (ObjectPool.GetPoolByInstance(instance, out var pool)) {
                pool.Release(instance);
            } else {
                Object.Destroy(instance);
            }
        }
    }
}