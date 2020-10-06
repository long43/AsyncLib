using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Executors
{
    public class SingleThreadExecutor : IExecutor
    {
        public SingleThreadExecutor()
        {
            tasksQueue_ = new BlockingCollection<Action>();
            thread_ = new Thread(() =>
            {
                Console.WriteLine("spawn a new thread to take tasks from blockingCollection");
                foreach (var task in tasksQueue_.GetConsumingEnumerable())
                {
                    task();
                }
            });
            thread_.Start();
        }

        // execute the task
        public void Post(Action task)
        {
            tasksQueue_.Add(task);
            tasksQueue_.CompleteAdding();
        }

        private Thread thread_;
        private BlockingCollection<Action> tasksQueue_;
    }
}
