using System;

namespace Game.FSM
{
    /// <summary>
    /// Base class for states.
    /// </summary>
    public abstract class StateBase : StateContext, IState
    {
        public abstract void Enter();
        public abstract void Exit();

        public override bool TriggerEvent<T>(T stateEvent, bool allowTransition = true)
        {
            bool isHandled = base.TriggerEvent(stateEvent, allowTransition);
            if (!isHandled && this is IEventHandler<T> eventHandler)
            {
                isHandled = eventHandler.HandleEvent(stateEvent);
                if (allowTransition && ExecuteNextTransition())
                {
                    isHandled = true;
                }
            }
            return isHandled;
        }
    }
}