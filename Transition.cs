using System;
using System.Collections.Generic;

namespace Game.FSM
{
    /// <summary>
    /// Represents transition between the active state branches.
    /// </summary>
    public abstract class TransitionBase
    {
        public List<Func<bool>> Conditions { get; } = new List<Func<bool>>();

        public IStateContext Context { get; }

        private readonly List<IState> targetStates = new List<IState>();

        protected TransitionBase(IStateContext ctx)
        {
            this.Context = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        /// <summary>
        /// Adds a target state for transition with no data.
        /// </summary>
        /// <param name="target"></param>
        public void AddTarget(IState target)
        {
            targetStates.Add(target);
        }

        /// <summary>
        /// Immediately executes transition by switching context subtree to a list of target states.
        /// </summary>
        public void Execute()
        {
            if (targetStates.Count == 0)
            {
                throw new InvalidOperationException("Attempt to execute an empty transition.");
            }
            var stateContext = Context;
            foreach (var state in targetStates)
            {
                if (stateContext == null) break;

                BeforeStateSwitch(state);
                stateContext.SwitchState(state);
                stateContext = state;
            }

        }

        protected abstract void BeforeStateSwitch(IState state);

        protected bool TryInvoke()
        {
            if (CanInvoke())
            {
                Context.NextTransition = this;
                return true;
            }

            return false;
        }

        private bool CanInvoke()
        {
            return Conditions.TrueForAll(ConditionPredicate);
        }

        private bool ConditionPredicate(Func<bool> c)
        {
            return c == null || c.Invoke();
        }
    }

    public abstract class TransitionBase<TStateAction> : TransitionBase
    {
        private readonly Dictionary<IState, TStateAction> executeActions = new Dictionary<IState, TStateAction>();

        protected TransitionBase(IStateContext ctx) : base(ctx)
        {
        }

        protected void AddTarget(IState state, TStateAction action)
        {
            base.AddTarget(state);
            executeActions.Add(state, action);
        }

        protected override void BeforeStateSwitch(IState state)
        {
            if (state != null && executeActions.TryGetValue(state, out var action))
            {
                ApplyStateAction(state, action);
            }
        }

        protected abstract void ApplyStateAction(IState state, TStateAction action);
    }

    /// <summary>
    /// Transition without input data.
    /// </summary>
    public class Transition : TransitionBase<Action>, ITrigger
    {
        public Transition(IStateContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Tries to invoke the transition if all conditions are met.
        /// Transition will not be executed immediately, Execute has to be called separetely.
        /// </summary>
        public void Invoke()
        {
            TryInvoke();
        }

        /// <summary>
        /// Adds target state with a transition data source function.
        /// </summary>
        /// <param name="state">The state to transition.</param>
        /// <param name="dataSource">Where to get the output data for the target state.</param>
        /// <returns></returns>
        public void AddTarget<TOut>(IStateWithInput<TOut> state, Func<TOut> dataSource)
        {
            AddTarget(state, () =>
            {
                state.SetTransitionData(dataSource());
            });
        }

        protected override void ApplyStateAction(IState state, Action action)
        {
            action();
        }
    }

    /// <summary>
    /// Transition with one input trigger parameter.
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    public class Transition<TIn> : TransitionBase<Action<TIn>>, ITrigger<TIn>
    {
        /// <summary>
        /// Data for the next execution of the transition.
        /// </summary>
        private TIn inputData;

        public Transition(IStateContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Tries to invoke the transition with given argument, if all conditions are met.
        /// Transition will not be executed immediately, Execute has to be called separetely.
        /// </summary>
        public void Invoke(TIn arg)
        {
            if (TryInvoke())
            {
                inputData = arg;
            }
        }

        /// <summary>
        /// Immediately executes the transition with given input data.
        /// </summary>
        /// <param name="arg"></param>
        public void Execute(TIn arg)
        {
            inputData = arg;
            Execute();
        }

        /// <summary>
        /// Adds a target state with transition data of the same type as the trigger data.
        /// </summary>
        /// <param name="state"></param>
        public void AddTarget(IStateWithInput<TIn> state)
        {
            AddTarget(state, state.SetTransitionData);
        }

        /// <summary>
        /// Adds a target state with transition data of type different from the trigger data.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="convert">Convertion between the trigger data and state transition data.</param>
        /// <typeparam name="TOut"></typeparam>
        public void AddTarget<TOut>(IStateWithInput<TOut> state, Func<TIn, TOut> convert)
        {
            AddTarget(state, arg =>
            {
                state.SetTransitionData(convert(arg));
            });
        }

        protected override void ApplyStateAction(IState state, Action<TIn> action)
        {
            action(inputData);
        }
    }
}