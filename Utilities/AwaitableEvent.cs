﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainArtery.Utilities
{
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    /// <summary>
    /// Implementation of an event whose invocation waits upon the execution of its listeners.
    /// </summary>
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    public sealed class AwaitableEvent : IAwaitableEvent, IDisposable, IAsyncDisposable
    {
        private readonly List<Func<Task>> _sequential = new List<Func<Task>>();
        private readonly List<Func<Task>> _unordered = new List<Func<Task>>();

        private readonly AsyncLock _lock = new AsyncLock();

        /// <summary>
        /// Execute all listening functions.<br/>
        /// Non-sequential listeners are all immediately executed, then sequential listeners are
        /// executed, in order, after each successive one completes.
        /// </summary>
        /// <returns>The awaitable task</returns>
        public async Task Invoke()
        {
            using (await _lock.LockAsync())
            {
                var unordered = Task.WhenAll(_unordered.Select(listener => listener()));
                foreach (var listener in _sequential)
                    await listener();
                await unordered;
            }
        }

        /// <inheritdoc/>
        public async Task AddSequentialListener(Func<Task> listener)
        {
            using (await _lock.LockAsync())
                if (!_sequential.Contains(listener))
                    _sequential.Add(listener);
        }

        /// <inheritdoc/>
        public async Task AddNonSequentialListener(Func<Task> listener)
        {
            using (await _lock.LockAsync())
                if (!_unordered.Contains(listener))
                    _unordered.Add(listener);
        }

        /// <inheritdoc/>
        public async Task RemoveListener(Func<Task> listener)
        {
            using (await _lock.LockAsync())
            {
                _sequential.Remove(listener);
                _unordered.Remove(listener);
            }
        }

        /// <summary>
        /// Unsubscribe sequential and nonsequential listeners
        /// </summary>
        public async Task RemoveAllListeners()
        {
            using (await _lock.LockAsync())
            {
                _sequential.Clear();
                _unordered.Clear();
            }
        }

        /// <summary>
        /// Call <see cref="RemoveAllListeners"/>.
        /// </summary>
        public void Dispose() => _ = RemoveAllListeners();

        /// <summary>
        /// Wait for <see cref="RemoveAllListeners"/>.
        /// </summary>
        public async ValueTask DisposeAsync() => await RemoveAllListeners();
    }
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    /// <summary>
    /// Generic implementation of an event whose invocation waits upon the execution of its
    /// listeners, which receive one argument from the event.
    /// </summary>
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    public sealed class AwaitableEvent<T> : IAwaitableEvent<T>, IDisposable, IAsyncDisposable
    {
        private readonly List<Func<T, Task>> _sequential = new List<Func<T, Task>>();
        private readonly List<Func<T, Task>> _unordered = new List<Func<T, Task>>();

        private readonly AsyncLock _lock = new AsyncLock();

        /// <summary>
        /// Execute all listening functions.<br/>
        /// Non-sequential listeners are all immediately executed, then sequential listeners are
        /// executed, in order, after each successive one completes.
        /// </summary>
        /// <param name="args">The event argument</param>
        /// <returns>The awaitable task</returns>
        public async Task Invoke(T args)
        {
            using (await _lock.LockAsync())
            {
                var unordered = Task.WhenAll(_unordered.Select(listener => listener(args)));
                foreach (var listener in _sequential)
                    await listener(args);
                await unordered;
            }
        }

        /// <inheritdoc/>
        public async Task AddSequentialListener(Func<T, Task> listener)
        {
            using (await _lock.LockAsync())
                if (!_sequential.Contains(listener))
                    _sequential.Add(listener);
        }

        /// <inheritdoc/>
        public async Task AddNonSequentialListener(Func<T, Task> listener)
        {
            using (await _lock.LockAsync())
                if (!_unordered.Contains(listener))
                    _unordered.Add(listener);
        }

        /// <inheritdoc/>
        public async Task RemoveListener(Func<T, Task> listener)
        {
            using (await _lock.LockAsync())
            {
                _sequential.Remove(listener);
                _unordered.Remove(listener);
            }
        }

        /// <summary>
        /// Unsubscribe sequential and nonsequential listeners
        /// </summary>
        public async Task RemoveAllListeners()
        {
            using (await _lock.LockAsync())
            {
                _sequential.Clear();
                _unordered.Clear();
            }
        }

        /// <summary>
        /// Call <see cref="RemoveAllListeners"/>.
        /// </summary>
        public void Dispose() => _ = RemoveAllListeners();

        /// <summary>
        /// Wait for <see cref="RemoveAllListeners"/>.
        /// </summary>
        public async ValueTask DisposeAsync() => await RemoveAllListeners();
    }
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
}
