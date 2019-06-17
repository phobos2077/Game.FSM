namespace Game.FSM
{
    /// <summary>
    /// Attach this interface to a State class to handle events of a given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventHandler<in T>
    {
        bool HandleEvent(T stateEvent);
    }
}