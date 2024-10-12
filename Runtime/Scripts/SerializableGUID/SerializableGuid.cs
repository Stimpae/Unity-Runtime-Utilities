using System;
using RuntimeUtilities.Extensions;
using UnityEngine;

namespace RuntimeUtilities.SerializableGUID {
    /// <summary>
    /// Represents a globally unique identifier (GUID) that is serializable with Unity and usable in game scripts.
    /// </summary>
    [Serializable]
    public struct SerializableGuid : IEquatable<SerializableGuid> {
        [SerializeField, HideInInspector] public uint part1;
        [SerializeField, HideInInspector] public uint part2;
        [SerializeField, HideInInspector] public uint part3;
        [SerializeField, HideInInspector] public uint part4;
        
        public static SerializableGuid Empty => new(0, 0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the SerializableGuid struct.
        /// </summary>
        public SerializableGuid(uint val1, uint val2, uint val3, uint val4) {
            part1 = val1;
            part2 = val2;
            part3 = val3;
            part4 = val4;
        }

        /// <summary>
        /// Initializes a new instance of the SerializableGuid struct from a Guid.
        /// </summary>
        public SerializableGuid(Guid guid) {
            byte[] bytes = guid.ToByteArray();
            part1 = BitConverter.ToUInt32(bytes, 0);
            part2 = BitConverter.ToUInt32(bytes, 4);
            part3 = BitConverter.ToUInt32(bytes, 8);
            part4 = BitConverter.ToUInt32(bytes, 12);
        }

        /// <summary>
        /// Creates a new SerializableGuid from a new Guid.
        /// </summary>
        public static SerializableGuid NewGuid() => Guid.NewGuid().ToSerializableGuid();

        /// <summary>
        /// Creates a new SerializableGuid from a hex string.
        /// </summary>
        public static SerializableGuid FromHexString(string hexString) {
            if (hexString.Length != 32) {
                return Empty;
            }

            return new SerializableGuid
            (
                Convert.ToUInt32(hexString.Substring(0, 8), 16),
                Convert.ToUInt32(hexString.Substring(8, 8), 16),
                Convert.ToUInt32(hexString.Substring(16, 8), 16),
                Convert.ToUInt32(hexString.Substring(24, 8), 16)
            );
        }

        /// <summary>
        /// Converts the SerializableGuid to a hex string.
        /// </summary>
        public string ToHexString() {
            return $"{part1:X8}{part2:X8}{part3:X8}{part4:X8}";
        }

        /// <summary>
        /// Converts the SerializableGuid to a Guid.
        /// </summary>
        public Guid ToGuid() {
            var bytes = new byte[16];
            BitConverter.GetBytes(part1).CopyTo(bytes, 0);
            BitConverter.GetBytes(part2).CopyTo(bytes, 4);
            BitConverter.GetBytes(part3).CopyTo(bytes, 8);
            BitConverter.GetBytes(part4).CopyTo(bytes, 12);
            return new Guid(bytes);
        }

        /// <summary>
        /// Implicit conversion from SerializableGuid to Guid.
        /// </summary>
        public static implicit operator Guid(SerializableGuid serializableGuid) => serializableGuid.ToGuid();

        /// <summary>
        /// Implicit conversion from Guid to SerializableGuid.
        /// </summary>
        public static implicit operator SerializableGuid(Guid guid) => new SerializableGuid(guid);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj) {
            return obj is SerializableGuid guid && this.Equals(guid);
        }

        /// <summary>
        /// Determines whether the specified SerializableGuid is equal to the current SerializableGuid.
        /// </summary>
        public bool Equals(SerializableGuid other) {
            return part1 == other.part1 && part2 == other.part2 && part3 == other.part3 && part4 == other.part4;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode() {
            return HashCode.Combine(part1, part2, part3, part4);
        }

        /// <summary>
        /// Determines whether two specified SerializableGuids have the same value.
        /// </summary>
        public static bool operator ==(SerializableGuid left, SerializableGuid right) => left.Equals(right);

        /// <summary>
        /// Determines whether two specified SerializableGuids have different values.
        /// </summary>
        public static bool operator !=(SerializableGuid left, SerializableGuid right) => !(left == right);
    }
}