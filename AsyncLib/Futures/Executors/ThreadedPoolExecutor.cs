using System;
using System.Threading;

namespace Executors
{
    public class ThreadedPoolExecutor : IExecutor
    {
        // execute the task
        public void Post(Action task)
        {
            ThreadPool.QueueUserWorkItem(input =>
            {
                task();
            });
        }
    }
}
