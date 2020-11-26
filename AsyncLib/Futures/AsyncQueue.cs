using System;
using Executors;

namespace Futures
{
    // AsyncQueue is used to schedule non-blocking calls in sequential and executed
    // asynchronously in the queue's own executor, i.e.
    // functor A is enqueued
    // functor B is enqueued
    // functor B will only be executed upon the finish of functor A
    public class AsyncQueue
    {
        public Future queueEnd { private set; get; }

        public AsyncQueue()
        {
            queueEnd = Future.MakeFuture(new object());
            executor_ = new ThreadedPoolExecutor();
        }

        // Enqueue a task. Return a future where the readiness of the future indicates
        // the execution of the task is finished.
        public Future Enqueue(AsyncTask task)
        {
            var id = GlobalCounter.Instance.Current;
            Console.WriteLine("enqueue the task: " + id);
            Future result = queueEnd;
            queueEnd = task.GetFuture();
            return result.Via(executor_).Then((input) =>
            {
                task.Execute();
                Console.WriteLine("finished executing task " + id);
            });
        }

        // the queueEnd_ is a future where the readiness of it indicates we reaches
        // the end of the queue, i.e. the last task is executed.
        private IExecutor executor_;
    }
}
