using System;
using System.Threading;
using System.Threading.Tasks;

namespace MainArtery.Utilities
{
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    /// <summary>
    /// Wrap an async process in event invocations to avoid propogating async method signatures.
    /// </summary>
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    public class AsyncWrappedByEvents
    {
        /// =======================================================================================
        /// Fields & Properties
        /// =======================================================================================

        /// <summary>
        /// Define how the wrapper expects to interface with an async process.
        /// </summary>
        /// <param name="sendProgress">Function to invoke when reporting progress</param>
        /// <param name="token">The cancellation token</param>
        public delegate Task WrappableProcessAsync(Action<float> sendProgress, CancellationToken token);

        /// <summary>
        /// The asynchronous process to wrap in event calls.
        /// </summary>
        private readonly WrappableProcessAsync _process;

        /// <summary>
        /// Wrap the executable process in a Task to provide metadata about its execution status.
        /// </summary>
        private Task _wrapperTask;

        /// <summary>
        /// Allow cancellation of the process if it handles it.
        /// </summary>
        private CancellationTokenSource _ctSource = new CancellationTokenSource();

        /// <summary>
        /// The time at which the process was last executed.
        /// </summary>
        public DateTime LastStartTime { get; private set; }

        /// <summary>
        /// The time at which the process last finished execution.
        /// </summary>
        public DateTime LastEndTime { get; private set; }

        /// <summary>
        /// Whether the last execution of the process successfully ran to completion.
        /// </summary>
        public bool LastRunSucceeded { get; private set; }

        /// <summary>
        /// Raised when the process begins execution.
        /// </summary>
        public event Action ProcessStarted;

        /// <summary>
        /// Raised when the process chooses to alert listeners of its progress.<br/>
        /// Sends the percentage of work that has been completed.
        /// </summary>
        public event Action<float> ProcessProgressed;

        /// <summary>
        /// Raised when the execution of the process has terminated.<br/>
        /// Sends whether the process completed successfully or was cancelled.
        /// </summary>
        public event Action<bool> ProcessEnded;

        /// <summary>
        /// Rasied when the <see cref="CanRun"/> property changes.
        /// </summary>
        public event Action<bool> CanRunChanged;

        /// <summary>
        /// Whether the process is able to be executed.
        /// </summary>
        public bool CanRun
        {
            get => _ableToRun;
            private set
            {
                if (_ableToRun != value)
                {
                    _ableToRun = value;
                    CanRunChanged?.Invoke(value);
                }
            }
        }
        private bool _ableToRun = true;

        /// <summary>
        /// Whether the process is currently being executed.
        /// </summary>
        public bool IsRunning => _wrapperTask != null
            && !(_wrapperTask.IsCompleted || _wrapperTask.IsFaulted || _wrapperTask.IsCanceled);

        /// =======================================================================================
        /// Constructor
        /// =======================================================================================

        /// <summary> Construct an instance of <see cref="AsyncWrappedByEvents"/>.</summary>
        /// <param name="processToRun">The async process to execute</param>
        public AsyncWrappedByEvents(WrappableProcessAsync processToRun)
        {
            _process = processToRun;

            ProcessStarted += DoProcess;
            ProcessStarted += () => { LastRunSucceeded = false; LastStartTime = DateTime.Now; };
            ProcessEnded += success => { LastRunSucceeded = success; LastEndTime = DateTime.Now; };
        }

        /// =======================================================================================
        /// Methods
        /// =======================================================================================

        /// <summary>
        /// Initiate execution of the process if able.
        /// </summary>
        /// <returns>Whether execution of the process was initiated</returns>
        public bool StartProcess()
        {
            if (IsRunning || !CanRun)
                return false;

            ProcessStarted.Invoke();
            return true;
        }

        /// <summary>
        /// Execute the process and wait for its completion.
        /// </summary>
        private async void DoProcess()
        {
            SynchronizationContext callingContext = SynchronizationContext.Current;
            try
            {
                _ctSource = new CancellationTokenSource();
                _ctSource.Token.ThrowIfCancellationRequested();

                Action<float> sendProgress;
                if (callingContext == null)
                    sendProgress = percentComplete => ProcessProgressed?.Invoke(percentComplete);
                else
                    sendProgress = percentComplete => callingContext.Post(delegate { ProcessProgressed?.Invoke(percentComplete); }, null);

                _wrapperTask = _process(sendProgress, _ctSource.Token);
                await _wrapperTask.ConfigureAwait(true);
            }
            catch (OperationCanceledException)
            {
                // Process was canceled
            }
            finally
            {
                _ctSource.Dispose();

                ProcessEnded.Invoke(_wrapperTask.Status == TaskStatus.RanToCompletion);

                _wrapperTask.Dispose();
                _wrapperTask = null;
            }
        }

        /// <summary>
        /// Halt execution of a currently-running process.
        /// </summary>
        public void CancelProcess()
        {
            if (IsRunning)
                _ctSource.Cancel();
        }
        /// =======================================================================================
    }
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
}