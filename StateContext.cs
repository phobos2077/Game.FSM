using System;

namespace Game.FSM
{
    /// <summary>
    /// State context. Acts as a parent node in a state hierarchy graph.
    /// </summary>
    public class StateContext : IStateContext
    {
        /// <inheritdoc />
        public event Action<IState> OnLeafStateChanged;

        /// <inheritdoc />
        public IState ActiveState { get; private set; }

        /// <inheritdoc />
        public virtual IState ActiveLeafState => ActiveState?.ActiveLeafState ?? ActiveState;

        public TransitionBase NextTransition { get; set; }

        /// <summary>
        /// Immediately switch to a given state.
        /// </summary>
        /// <param name="newState"></param>
        public void SwitchState(IState newState)
        {
            if (ActiveState != null)
            {
                ActiveState.SwitchState(null);
                ActiveState.Exit();
                ActiveState.OnLeafStateChanged -= InvokeLeafStateChange;
            }

            var oldState = ActiveState;
            ActiveState = newState;

            if (newState != null)
            {
                newState.Enter();
                newState.OnLeafStateChanged += InvokeLeafStateChange;
            }

            if (oldState != newState)
            {
                InvokeLeafStateChange(newState?.ActiveLeafState);
            }
        }

        private void InvokeLeafStateChange(IState state)
        {
            OnLeafStateChanged?.Invoke(state);
        }

        public bool ExecuteNextTransition()
        {
            if (NextTransition != null)
            {
                NextTransition.Execute();
                NextTransition = null;
                return true;
            }
            return ActiveState != null && ActiveState.ExecuteNextTransition();
        }

        /// <summary>
        /// Checks that target context belongs to an active branch of a given root context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private static bool IsActiveContext(IStateContext context, IStateContext root)
        {
            if (root == context)
            {
                return true;
            }
            var state = root.ActiveState;
            while (state != null)
            {
                if (state == context)
                {
                    return true;
                }

                state = state.ActiveState;
            }

            return false;
        }

        /// <summary>
        /// Traverse the active state tree branch and handle the event.
        /// </summary>
        /// <param name="stateEvent"></param>
        /// <param name="allowTransition"></param>
        /// <typeparam name="T">State event type.</typeparam>
        /// <returns>True if event was handled.</returns>
        public virtual bool TriggerEvent<T>(T stateEvent, bool allowTransition = true)
        {
            if (ActiveState != null)
            {
                var isHandled = ActiveState.TriggerEvent(stateEvent, false);
                if (allowTransition && ExecuteNextTransition())
                {
                    isHandled = true;
                }
                return isHandled;
            }

            return false;
        }
    }
}