using System;
using System.Threading;

using Executors;

namespace Futures
{

    internal class SharedState
    {
        internal ManualResetEvent doneEvent = new ManualResetEvent(false);

        internal bool isReady;

        internal object result;

        internal Action func;

        internal IExecutor executor = new ThreadedPoolExecutor();

        internal void Execute()
        {
            if (func != null)
            {
                executor.Post(func);
            }
        }
    }
}
