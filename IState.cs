using System;

namespace Game.FSM
{
    /// <summary>
    /// State that is also a StateContext for nesting support.
    /// </summary>
    public interface IState : IStateContext
    {
        /// <summary>
        /// Called when context is entering the state.
        /// </summary>
        void Enter();

        /// <summary>
        /// Called when context is exiting the state.
        /// </summary>
        void Exit();
    }
}