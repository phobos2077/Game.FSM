using System;

namespace Game.FSM
{
    /// <summary>
    /// State that is also a StateContext for nesting support.
    /// </summary>
    public interface IState : IStateContext
    {
        void Enter();

        void Exit();
    }
}