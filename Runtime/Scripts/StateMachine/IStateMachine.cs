namespace RuntimeUtilities.StateMachine {
    public interface IStateMachine {
        bool ShouldTriggerEvents { get; set; }
    }
}