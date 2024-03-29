namespace Game.FSM
{
    /// <summary>
    /// Indicates that a state expects an inbound transition to assign input data of certain type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStateWithInput<in T> : IState
    {
        /// <summary>
        /// Applies transition data argument to a state.
        /// </summary>
        /// <param name="arg"></param>
        void SetTransitionData(T arg);
    }
}