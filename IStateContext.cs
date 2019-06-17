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

        TransitionBase NextTransition { set; }

        bool ExecuteNextTransition();

        /// <summary>
        /// Immediately switch to a new state.
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(IState newState);

        bool TriggerEvent<T>(T stateEvent, bool allowTransition = true);
    }
}