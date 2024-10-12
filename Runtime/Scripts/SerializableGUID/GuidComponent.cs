using UnityEngine;

namespace RuntimeUtilities.SerializableGUID {
    [DisallowMultipleComponent]
    public class GuidComponent : MonoBehaviour {
        [SerializeField] private SerializableGuid guid = SerializableGuid.NewGuid();
        
        public SerializableGuid Guid => guid;
        
        public virtual void Reset() {
            guid = SerializableGuid.NewGuid();
        }
        
        public virtual void GenerateNewGuid() {
            guid = SerializableGuid.NewGuid();
        }
        
        public virtual void GenerateNewGuidIfEmpty() {
            if (guid == SerializableGuid.Empty) {
                guid = SerializableGuid.NewGuid();
            }
        }
    }
}