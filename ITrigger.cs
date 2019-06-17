namespace Game.FSM
{
    /// <summary>
    /// Transition trigger w/o data.
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        /// Triggers the transition.
        /// </summary>
        /// <remarks>
        /// Transition will only initiate after the current State Event is handled for the active state sub-branch.
        /// </remarks>
        void Invoke();
    }

    /// <summary>
    /// Transition trigger with one data argument.
    /// </summary>
    /// <typeparam name="TArg">Type of the data argument.</typeparam>
    public interface ITrigger<in TArg>
    {
        /// <summary>
        /// Triggers the transition with a specified data argument.
        /// </summary>
        /// <remarks>
        /// Transition will only initiate after the current State Event is handled for the active state sub-branch.
        /// </remarks>
        /// <param name="arg"></param>
        void Invoke(TArg arg);
    }
}