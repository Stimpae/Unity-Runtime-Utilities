namespace TG.Utilities {
    public interface IState {
        public void OnEnter() {}
        public void OnExit() {}
        public void OnUpdate() {}
    }
}