namespace TG.Utilities {
    /// <summary>
    /// Interface for objects that want to receive callbacks from the pool.
    /// Use this to reset or clean up objects when they are reused or released.
    /// </summary>
    public interface IPoolCallbackReceiver {
        void OnReuse();
        void OnRelease();
    }
}