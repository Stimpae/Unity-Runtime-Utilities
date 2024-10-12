using System;
using System.Collections.Generic;
using System.Reflection;

namespace RuntimeUtilities.EventBus {
    /// <summary>
    /// Provides methods to interact with predefined assemblies.
    /// </summary>
    public class PredefinedAssemblyUtilities {
        /// <summary>
        /// Enum representing different types of assemblies.
        /// </summary>
        enum AssemblyType {
            ASSEMBLY_C_SHARP,
            ASSEMBLY_C_SHARP_EDITOR,
            ASSEMBLY_C_SHARP_EDITOR_FIRST_PASS,
            ASSEMBLY_C_SHARP_FIRST_PASS
        }

        /// <summary>
        /// Gets the assembly type based on the assembly name.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <returns>The corresponding <see cref="AssemblyType"/> or null if not found.</returns>
        static AssemblyType? GetAssemblyType(string assemblyName) {
            return assemblyName switch {
                "Assembly-CSharp" => AssemblyType.ASSEMBLY_C_SHARP,
                "Assembly-CSharp-Editor" => AssemblyType.ASSEMBLY_C_SHARP_EDITOR,
                "Assembly-CSharp-Editor-firstpass" => AssemblyType.ASSEMBLY_C_SHARP_EDITOR_FIRST_PASS,
                "Assembly-CSharp-firstpass" => AssemblyType.ASSEMBLY_C_SHARP_FIRST_PASS,
                _ => null
            };
        }

        /// <summary>
        /// Adds types from the specified assembly that implement the given interface to the results collection.
        /// </summary>
        /// <param name="assemblyTypes">Array of types from the assembly.</param>
        /// <param name="interfaceType">The interface type to check against.</param>
        /// <param name="results">The collection to add the matching types to.</param>
        static void AddTypesFromAssembly(Type[] assemblyTypes, Type interfaceType, ICollection<Type> results) {
            if (assemblyTypes == null) return;
            foreach (var type in assemblyTypes) {
                if (type != interfaceType && interfaceType.IsAssignableFrom(type)) {
                    results.Add(type);
                }
            }
        }

        /// <summary>
        /// Gets a list of types from predefined assemblies that implement the specified interface.
        /// </summary>
        /// <param name="interfaceType">The interface type to check against.</param>
        /// <returns>A list of types that implement the specified interface.</returns>
        public static List<Type> GetTypes(Type interfaceType) {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblyTypes = new Dictionary<AssemblyType, Type[]>();
            var types = new List<Type>();

            foreach (var assembly in assemblies) {
                var assemblyType = GetAssemblyType(assembly.GetName().Name);
                if (assemblyType != null) {
                    assemblyTypes[(AssemblyType)assemblyType] = assembly.GetTypes();
                }
            }

            if (assemblyTypes.TryGetValue(AssemblyType.ASSEMBLY_C_SHARP, out var assemblyCSharpTypes)) {
                AddTypesFromAssembly(assemblyCSharpTypes, interfaceType, types);
            }

            if (assemblyTypes.TryGetValue(AssemblyType.ASSEMBLY_C_SHARP_FIRST_PASS, out var assemblyCSharpFirstPassTypes)) {
                AddTypesFromAssembly(assemblyCSharpFirstPassTypes, interfaceType, types);
            }

            return types;
        }
    }
}