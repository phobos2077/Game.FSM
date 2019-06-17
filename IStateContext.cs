using System;

namespace Game.FSM
{
    /// <summary>
    /// State context. Acts as a parent node in a state hierarchy graph.
    /// </summary>
    public interface IStateContext
    {
        /// <summary>
        /// Invoked when the innermost active child state is changed.
        /// </summary>
        event Action<IState> OnLeafStateChanged;

        /// <summary>
        /// Current innermost active child state.
        /// </summary>
        IState ActiveLeafState { get; }

        /// <summary>
        /// Current immediate child state in this context.
        /// </summary>
        IState ActiveState { get; }

        /// <summary>
        /// Next transition to execute.
        /// </summary>
        TransitionBase NextTransition { set; }

        /// <summary>
        /// Executes the next transition in this context or any of it's children in the active branch.
        /// </summary>
        /// <returns>True if transition was executed.</returns>
        bool ExecuteNextTransition();

        /// <summary>
        /// Immediately switch to a new state.
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(IState newState);

        /// <summary>
        /// Triggers the event for the active states to handle.
        /// </summary>
        /// <param name="stateEvent">Event data.</param>
        /// <param name="allowTransition">Allow transitions to be executed automatically after event is handled.</param>
        /// <typeparam name="T">Type of event.</typeparam>
        /// <returns>True if event was handled by any active state.</returns>
        bool TriggerEvent<T>(T stateEvent, bool allowTransition = true);
    }
}