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

        /// <inheritdoc />
        public TransitionBase NextTransition { get; set; }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        private void InvokeLeafStateChange(IState state)
        {
            OnLeafStateChanged?.Invoke(state);
        }

        /// <summary>
        /// Checks that target context belongs to an active branch of a given root context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public static bool IsActiveContext(IStateContext context, IStateContext root)
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
    }
}