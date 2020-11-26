using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Executors
{
    public class SingleThreadExecutor : IExecutor, IDisposable
    {
        public SingleThreadExecutor()
        {
            tasksQueue_ = new BlockingCollection<Action>();
            thread_ = new Thread(() =>
            {
                Console.WriteLine("spawn a new thread to take tasks from blockingCollection");
                foreach (var action in tasksQueue_.GetConsumingEnumerable())
                {
                    action();
                }
            });
            thread_.Start();
        }

        // execute the task
        public void Post(Action task)
        {
            tasksQueue_.Add(task);
        }

        void IDisposable.Dispose()
        {
            tasksQueue_.CompleteAdding();
        }


        private Thread thread_;
        private BlockingCollection<Action> tasksQueue_;
    }
}
