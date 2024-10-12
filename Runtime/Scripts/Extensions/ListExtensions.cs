using System.Collections.Generic;
using UnityEngine;

namespace RuntimeUtilities.Extensions {
    /// <summary>
    /// Provides extension methods for List types
    /// </summary>
    public static class ListExtensions {
        /// <summary>
        /// Clears the list and adds the specified items to it.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to refresh.</param>
        /// <param name="items">The items to add to the list.</param>
        public static void RefreshWith<T>(this List<T> list, IEnumerable<T> items) {
            list.Clear();
            list.AddRange(items);
        }
        
        /// <summary>
        /// Clears the list and destroys each element if it is a Unity Object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to clear and destroy elements from.</param>
        /// <param name="immediate">If true, destroys objects immediately. Otherwise, destroys objects at the end of the frame.</param>
        public static void ClearAndDestroy<T>(this List<T> list, bool immediate = false) {
            if (list == null) return;
            foreach (var element in list) {
                if (element is not Object unityObject) continue;
                if (immediate) {
                    Object.DestroyImmediate(unityObject);
                } else {
                    Object.Destroy(unityObject);
                }
            }
            list.Clear();
        }

        /// <summary>
        /// Prints the names of GameObjects in the list to the Unity console.
        /// </summary>
        /// <param name="list">The list of GameObjects to print.</param>
        public static void PrintGameObjectList(this List<GameObject> list) {
            if (list == null) {
                Debug.Log("List is null");
                return;
            }

            if (list.Count == 0) {
                Debug.Log("List is empty");
                return;
            }

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Printing list of GameObjects with {list.Count} elements:");

            for (var i = 0; i < list.Count; i++) {
                sb.AppendLine($"Element {i}: {list[i].name}");
            }

            Debug.Log(sb.ToString());
        }
    }
}