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

    /// <summary>
    /// Public-facing interactions with an event whose invocation waits asynchronously upon the
    /// execution of its listeners.
    /// </summary>
    /// <typeparam name="T1">The type of object to send as the first parameter to the event</typeparam>
    /// <typeparam name="T2">The type of object to send as the second parameter to the event</typeparam>
    /// <remarks>
    /// The purpose of defining these operations in an interface is to allow restriction of the
    /// event's invocation to the object that contains it, like plain C# events, e.g.:
    /// <para/>
    /// <code>
    ///     private readonly <see cref="AwaitableEvent{T1, T2}"/> _thingChanged = new();
    ///     public <see cref="IAwaitableEvent{T1, T2}"/> ThingChanged => _thingChanged;
    /// </code>
    /// </remarks>
    public interface IAwaitableEvent<T1, T2>
    {
        /// <summary>
        /// Add a listener that will execute in order after any previously added sequential listeners.
        /// </summary>
        Task AddSequentialListener(Func<T1, T2, Task> listener);

        /// <summary>
        /// Add a listener that will execute in concert with any previously added non-sequential listeners.
        /// </summary>
        Task AddNonSequentialListener(Func<T1, T2, Task> listener);

        /// <summary>
        /// Unsubscribe the listener from the event invocation.
        /// </summary>
        Task RemoveListener(Func<T1, T2, Task> listener);
    }

    /// <summary>
    /// Public-facing interactions with an event whose invocation waits asynchronously upon the
    /// execution of its listeners.
    /// </summary>
    /// <typeparam name="T1">The type of object to send as the first parameter to the event</typeparam>
    /// <typeparam name="T2">The type of object to send as the second parameter to the event</typeparam>
    /// <typeparam name="T3">The type of object to send as the third parameter to the event</typeparam>
    /// <remarks>
    /// The purpose of defining these operations in an interface is to allow restriction of the
    /// event's invocation to the object that contains it, like plain C# events, e.g.:
    /// <para/>
    /// <code>
    ///     private readonly <see cref="AwaitableEvent{T1, T2, T3}"/> _thingChanged = new();
    ///     public <see cref="IAwaitableEvent{T1, T2, T3}"/> ThingChanged => _thingChanged;
    /// </code>
    /// </remarks>
    public interface IAwaitableEvent<T1, T2, T3>
    {
        /// <summary>
        /// Add a listener that will execute in order after any previously added sequential listeners.
        /// </summary>
        Task AddSequentialListener(Func<T1, T2, T3, Task> listener);

        /// <summary>
        /// Add a listener that will execute in concert with any previously added non-sequential listeners.
        /// </summary>
        Task AddNonSequentialListener(Func<T1, T2, T3, Task> listener);

        /// <summary>
        /// Unsubscribe the listener from the event invocation.
        /// </summary>
        Task RemoveListener(Func<T1, T2, T3, Task> listener);
    }

    /// <summary>
    /// Public-facing interactions with an event whose invocation waits asynchronously upon the
    /// execution of its listeners.
    /// </summary>
    /// <typeparam name="T1">The type of object to send as the first parameter to the event</typeparam>
    /// <typeparam name="T2">The type of object to send as the second parameter to the event</typeparam>
    /// <typeparam name="T3">The type of object to send as the third parameter to the event</typeparam>
    /// <typeparam name="T4">The type of object to send as the fourth parameter to the event</typeparam>
    /// <remarks>
    /// The purpose of defining these operations in an interface is to allow restriction of the
    /// event's invocation to the object that contains it, like plain C# events, e.g.:
    /// <para/>
    /// <code>
    ///     private readonly <see cref="AwaitableEvent{T1, T2, T3, T4}"/> _thingChanged = new();
    ///     public <see cref="IAwaitableEvent{T1, T2, T3, T4}"/> ThingChanged => _thingChanged;
    /// </code>
    /// </remarks>
    public interface IAwaitableEvent<T1, T2, T3, T4>
    {
        /// <summary>
        /// Add a listener that will execute in order after any previously added sequential listeners.
        /// </summary>
        Task AddSequentialListener(Func<T1, T2, T3, T4, Task> listener);

        /// <summary>
        /// Add a listener that will execute in concert with any previously added non-sequential listeners.
        /// </summary>
        Task AddNonSequentialListener(Func<T1, T2, T3, T4, Task> listener);

        /// <summary>
        /// Unsubscribe the listener from the event invocation.
        /// </summary>
        Task RemoveListener(Func<T1, T2, T3, T4, Task> listener);
    }
}
