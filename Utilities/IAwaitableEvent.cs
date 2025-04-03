using System;
using System.Threading.Tasks;

namespace MainArtery.Utilities
{
    /// <summary>
    /// Public-facing interactions with an event whose invocation waits asynchronously upon the
    /// execution of its listeners.
    /// </summary>
    /// <remarks>
    /// The purpose of defining these operations in an interface is to allow restriction of the
    /// event's invocation to the object that contains it, like plain C# events, e.g.:
    /// <para/>
    /// <code>
    ///     private readonly <see cref="AwaitableEvent"/> _thingChanged = new();
    ///     public <see cref="IAwaitableEvent"/> ThingChanged => _thingChanged;
    /// </code>
    /// </remarks>
    public interface IAwaitableEvent
    {
        /// <summary>
        /// Add a listener that will execute in order after any previously added sequential listeners.
        /// </summary>
        Task AddSequentialListener(Func<Task> listener);

        /// <summary>
        /// Add a listener that will execute in concert with any previously added non-sequential listeners.
        /// </summary>
        Task AddNonSequentialListener(Func<Task> listener);

        /// <summary>
        /// Unsubscribe the listener from the event invocation.
        /// </summary>
        Task RemoveListener(Func<Task> listener);
    }

    /// <summary>
    /// Public-facing interactions with an event whose invocation waits asynchronously upon the
    /// execution of its listeners.
    /// </summary>
    /// <typeparam name="T">The type of object to send as a parameter to the event</typeparam>
    /// <remarks>
    /// The purpose of defining these operations in an interface is to allow restriction of the
    /// event's invocation to the object that contains it, like plain C# events, e.g.:
    /// <para/>
    /// <code>
    ///     private readonly <see cref="AwaitableEvent{T}"/> _thingChanged = new();
    ///     public <see cref="IAwaitableEvent{T}"/> ThingChanged => _thingChanged;
    /// </code>
    /// </remarks>
    public interface IAwaitableEvent<T>
    {
        /// <summary>
        /// Add a listener that will execute in order after any previously added sequential listeners.
        /// </summary>
        Task AddSequentialListener(Func<T, Task> listener);

        /// <summary>
        /// Add a listener that will execute in concert with any previously added non-sequential listeners.
        /// </summary>
        Task AddNonSequentialListener(Func<T, Task> listener);

        /// <summary>
        /// Unsubscribe the listener from the event invocation.
        /// </summary>
        Task RemoveListener(Func<T, Task> listener);
    }
}
