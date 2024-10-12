using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

namespace RuntimeUtilities {
    /// <summary>
    /// Utility class for managing Unity's PlayerLoop systems.
    /// </summary>
    public static class PlayerLoopUtils {
        /// <summary>
        /// Removes a specified system from the PlayerLoop.
        /// </summary>
        /// <typeparam name="T">The type of PlayerLoop system to remove.</typeparam>
        /// <param name="loop">The PlayerLoop system to modify.</param>
        /// <param name="systemToRemove">The system to remove from the PlayerLoop.</param>
        public static void RemoveSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToRemove) {
            if (loop.subSystemList == null) return;
            
            var playerLoopSystemList = new List<PlayerLoopSystem>(loop.subSystemList);
            for (int i = 0; i < playerLoopSystemList.Count; ++i) {
                if (playerLoopSystemList[i].type == systemToRemove.type && playerLoopSystemList[i].updateDelegate == systemToRemove.updateDelegate) {
                    playerLoopSystemList.RemoveAt(i);
                    loop.subSystemList = playerLoopSystemList.ToArray();
                }
            }
            
            HandleSubSystemLoopForRemoval<T>(ref loop, systemToRemove);
        }

        /// <summary>
        /// Handles the removal of a system from the sub-systems of a PlayerLoop.
        /// </summary>
        /// <typeparam name="T">The type of PlayerLoop system to remove.</typeparam>
        /// <param name="loop">The PlayerLoop system to modify.</param>
        /// <param name="systemToRemove">The system to remove from the PlayerLoop.</param>
        static void HandleSubSystemLoopForRemoval<T>(ref PlayerLoopSystem loop, PlayerLoopSystem systemToRemove) {
            if (loop.subSystemList == null) return;

            for (int i = 0; i < loop.subSystemList.Length; ++i) {
                RemoveSystem<T>(ref loop.subSystemList[i], systemToRemove);
            }
        }
        
        /// <summary>
        /// Inserts a system into the PlayerLoop at the specified index.
        /// </summary>
        /// <typeparam name="T">The type of PlayerLoop system to insert into.</typeparam>
        /// <param name="loop">The PlayerLoop system to modify.</param>
        /// <param name="systemToInsert">The system to insert into the PlayerLoop.</param>
        /// <param name="index">The index at which to insert the system.</param>
        /// <returns>True if the system was successfully inserted, false otherwise.</returns>
        public static bool InsertSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index) {
            if (loop.type != typeof(T)) return HandleSubSystemLoop<T>(ref loop, systemToInsert, index);
            
            var playerLoopSystemList = new List<PlayerLoopSystem>();
            if (loop.subSystemList != null) playerLoopSystemList.AddRange(loop.subSystemList);
            playerLoopSystemList.Insert(index, systemToInsert);
            loop.subSystemList = playerLoopSystemList.ToArray();
            return true;
        }

        /// <summary>
        /// Handles the insertion of a system into the sub-systems of a PlayerLoop.
        /// </summary>
        /// <typeparam name="T">The type of PlayerLoop system to insert into.</typeparam>
        /// <param name="loop">The PlayerLoop system to modify.</param>
        /// <param name="systemToInsert">The system to insert into the PlayerLoop.</param>
        /// <param name="index">The index at which to insert the system.</param>
        /// <returns>True if the system was successfully inserted, false otherwise.</returns>
        static bool HandleSubSystemLoop<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index) {
            if (loop.subSystemList == null) return false;

            for (int i = 0; i < loop.subSystemList.Length; ++i) {
                if (!InsertSystem<T>(ref loop.subSystemList[i], in systemToInsert, index)) continue;
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Prints the current PlayerLoop structure to the Unity console.
        /// </summary>
        /// <param name="loop">The PlayerLoop system to print.</param>
        public static void PrintPlayerLoop(PlayerLoopSystem loop) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Unity Player Loop");
            foreach (PlayerLoopSystem subSystem in loop.subSystemList) {
                PrintSubsystem(subSystem, sb, 0);
            }
            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// Recursively prints the structure of a PlayerLoop system and its sub-systems.
        /// </summary>
        /// <param name="system">The PlayerLoop system to print.</param>
        /// <param name="sb">The StringBuilder to append the output to.</param>
        /// <param name="level">The current level of indentation.</param>
        static void PrintSubsystem(PlayerLoopSystem system, StringBuilder sb, int level) {
            sb.Append(' ', level * 2).AppendLine(system.type.ToString());
            if (system.subSystemList == null || system.subSystemList.Length == 0) return;

            foreach (PlayerLoopSystem subSystem in system.subSystemList) {
                PrintSubsystem(subSystem, sb, level + 1);
            }
        }
    }
}