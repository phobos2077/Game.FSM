using System;

namespace Game.FSM
{
    /// <summary>
    /// Extension methods providing fluent interface for transition building.
    /// </summary>
    public static class TransitionExtensions
    {
        /// <summary>
        /// Adds a condition for a given transition.
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="condition">Condition predicate.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T WithCondition<T>(this T transition, Func<bool> condition) where T : TransitionBase
        {
            transition.Conditions.Add(condition);
            return transition;
        }

        /// <summary>
        /// Adds a target state for transition with no data.
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static T With<T>(this T transition, IState state) where T: TransitionBase
        {
            transition.AddTarget(state);
            return transition;
        }

        /// <summary>
        /// Adds target state with a transition data source function.
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="state">The state to transition.</param>
        /// <param name="dataSource">Where to get the output data for the target state.</param>
        /// <returns></returns>
        public static Transition With<TOut>(this Transition transition, IStateWithInput<TOut> state, Func<TOut> dataSource)
        {
            transition.AddTarget(state, dataSource);
            return transition;
        }

        /// <summary>
        /// Adds transition to a target state with transition data of the same type as the trigger data.
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="state"></param>
        public static Transition<TIn> With<TIn>(this Transition<TIn> transition, IStateWithInput<TIn> state)
        {
            transition.AddTarget(state);
            return transition;
        }

        /// <summary>
        /// Adds transition to a target state with transition data of type different from the trigger data.
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="state"></param>
        /// <param name="convert">Convertion between the trigger data and state transition data.</param>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        public static Transition<TIn> With<TIn, TOut>(this Transition<TIn> transition, IStateWithInput<TOut> state, Func<TIn, TOut> convert)
        {
            transition.AddTarget(state, convert);
            return transition;
        }

        /// <summary>
        /// Creates an empty transition.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Transition Transition(this IStateContext context)
        {
            return new Transition(context);
        }

        /// <summary>
        /// Creates an empty transition.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Transition<TIn> Transition<TIn>(this IStateContext context)
        {
            return new Transition<TIn>(context);
        }

        /// <summary>
        /// Creates transition into a state with no data.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="target">Target state.</param>
        /// <returns></returns>
        public static Transition Transition(this IStateContext context, IState target)
        {
            return new Transition(context).With(target);
        }

        /// <summary>
        /// Creates transition into a state with data source function.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="target">Target state.</param>
        /// <param name="dataSource">Where to get the data for the target state from.</param>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        public static Transition Transition<TOut>(this IStateContext context, IStateWithInput<TOut> target, Func<TOut> dataSource)
        {
            return new Transition(context).With(target, dataSource);
        }

        /// <summary>
        /// Creates transition into a state with no data.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="target">Target state.</param>
        /// <typeparam name="TIn"></typeparam>
        /// <returns></returns>
        public static Transition<TIn> Transition<TIn>(this IStateContext context, IState target)
        {
            return new Transition<TIn>(context).With(target);
        }

        /// <summary>
        /// Creates transition into a state of matching transition data type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="target">Target state.</param>
        /// <typeparam name="TIn"></typeparam>
        /// <returns></returns>
        public static Transition<TIn> Transition<TIn>(this IStateContext context, IStateWithInput<TIn> target)
        {
            return new Transition<TIn>(context).With(target);
        }

        /// <summary>
        /// Creates transition into a state with different transition data types.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="target">Target state.</param>
        /// <param name="convert">Transition data convertion function.</param>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        public static Transition<TIn> Transition<TIn, TOut>(this IStateContext context, IStateWithInput<TOut> target, Func<TIn, TOut> convert)
        {
            return new Transition<TIn>(context).With(target, convert);
        }
    }
}