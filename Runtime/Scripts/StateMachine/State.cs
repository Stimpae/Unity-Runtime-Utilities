namespace TG.Utilities {
    /// <summary>
    /// Empty state class to be used as a base for all states
    /// </summary>
    public abstract class State : IState {
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnUpdate();
    }
}