
namespace System.Threading.Tasks
{
    using System.Linq;
    using System.Threading;

    /// <summary>Extensions methods for Task.</summary>
    public static class TaskExtensions
    {
        /// <summary>Suppresses default exception handling of a Task that would otherwise reraise the exception on the finalizer thread.</summary>
        /// <param name="task">The Task to be monitored.</param>
        /// <returns>The original Task.</returns>
        public static Task IgnoreExceptions(this Task task, Action<AggregateException> handler = null)
        {
            task.ContinueWith(t =>
                {
                    var ignored = t.Exception;
                    if (handler != null)
                    {
                        handler(t.Exception);
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.Default);
            return task;
        }

        /// <summary>Suppresses default exception handling of a Task that would otherwise reraise the exception on the finalizer thread.</summary>
        /// <param name="task">The Task to be monitored.</param>
        /// <returns>The original Task.</returns>
        public static Task<T> IgnoreExceptions<T>(this Task<T> task, Action<AggregateException> handler = null)
        {
            return (Task<T>)((Task)task).IgnoreExceptions(handler);
        }
    }
}