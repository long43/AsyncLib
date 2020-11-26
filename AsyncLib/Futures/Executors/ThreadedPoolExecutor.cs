using System;
using System.Threading;

namespace Executors
{
    public class ThreadedPoolExecutor : IExecutor
    {
        // execute the task
        public void Post(Action action)
        {
            ThreadPool.QueueUserWorkItem(input =>
            {
                action();
            });
        }

    }
}
