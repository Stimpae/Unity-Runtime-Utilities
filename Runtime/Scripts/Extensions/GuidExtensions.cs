using System;
using RuntimeUtilities.SerializableGUID;

namespace RuntimeUtilities.Extensions {
    /// <summary>
    /// Provides extension methods for the Guid class.
    /// </summary>
    public static class GuidExtensions {
        
        /// <summary>
        /// Converts a System.Guid to a SerializableGuid.
        /// </summary>
        /// <param name="systemGuid">The System.Guid to convert.</param>
        /// <returns>A SerializableGuid that represents the same value as the input System.Guid.</returns>
        public static SerializableGuid ToSerializableGuid(this Guid systemGuid) {
            byte[] bytes = systemGuid.ToByteArray();
            return new SerializableGuid(
                BitConverter.ToUInt32(bytes, 0),
                BitConverter.ToUInt32(bytes, 4),
                BitConverter.ToUInt32(bytes, 8),
                BitConverter.ToUInt32(bytes, 12)
            );
        }

        /// <summary>
        /// Converts a SerializableGuid to a System.Guid.
        /// </summary>
        /// <param name="serializableGuid">The SerializableGuid to convert.</param>
        /// <returns>A System.Guid that represents the same value as the input SerializableGuid.</returns>
        public static Guid ToSystemGuid(this SerializableGuid serializableGuid) {
            byte[] bytes = new byte[16];
            Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.part1), 0, bytes, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.part2), 0, bytes, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.part3), 0, bytes, 8, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.part4), 0, bytes, 12, 4);
            return new Guid(bytes);
        }
    }
}