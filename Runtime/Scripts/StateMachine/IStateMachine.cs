namespace TG.Utilities {
    public interface IStateMachine {
        bool ShouldTriggerEvents { get; set; }
        bool ShouldUpdate { get; set; }
    }
}