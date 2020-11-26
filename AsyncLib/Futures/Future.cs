using System;
using System.Threading;

using Executors;

namespace Futures
{

    // Future is an abstraction of a way to fetch the result asynchronously
    // There are two types of the Futures:
    // 1. get from Promise
    // 2. created a fullfiled Future
    // One can attach continuation callbacks via Future.Then method. The callback
    // is essentially a Action<object> or Func<object, object>, where the former
    // doesn't return value and the latter will return a value.
    public class Future
    {
        internal Future(SharedState state)
        {
            sharedState_ = state;
        }

        // create a fullfilled Future
        public static Future MakeFuture(object value)
        {
            Promise p = new Promise();
            Future f = p.GetFuture();
            p.SetValue(value);
            return f;
        }

        public bool IsReady()
        {
            return sharedState_.isReady;
        }

        public object Get()
        {
            Wait();
            return sharedState_.result;
        }

        public void Wait()
        {
            while (!sharedState_.isReady)
            {
                sharedState_.doneEvent.WaitOne();
            }
        }

        public Future Then(Action<object> func)
        {
            AsyncTask task = new AsyncTask(func);
            Action action = () => { task.Execute(sharedState_.result); };


            if (sharedState_.isReady)
            {
                sharedState_.executor.Post(action);
            }
            else
            {
                sharedState_.func = action;
            }

            return task.GetFuture();
        }

        public Future Then(Func<object, object> func)
        {
            AsyncTask task = new AsyncTask(func);
            Action action = () => { task.Execute(sharedState_.result); };

            if (sharedState_.isReady)
            {
                sharedState_.executor.Post(action);
            }
            else
            {
                sharedState_.func = action;
            }

            return task.GetFuture();
        }

        public Future Via(IExecutor executor)
        {
            sharedState_.executor = executor;
            return this;
        }

        private SharedState sharedState_;
    }
}
