using System;

namespace Futures
{
    // AsyncTask is an abstraction of delegated Action or Func, where the Action
    // or Func will be executed asynchronously.
    // One can get a Future from the task and when the task is executed, it will
    // fulfill the Future. 
    public class AsyncTask
    {
        public AsyncTask(Action func)
        {
            Promise promise = new Promise();
            future_ = promise.GetFuture();

            func_ = (input) =>
            {
                func();
                promise.SetValue(input);
            };
        }

        public AsyncTask(Action<object> func) : base()
        {
            Promise promise = new Promise();

            future_ = promise.GetFuture();

            func_ = (input) =>
            {
                func(input);
                promise.SetValue(input);
            };
        }

        public AsyncTask(Func<object, object> func) : base()
        {
            Promise promise = new Promise();

            future_ = promise.GetFuture();

            func_ = (input) =>
            {
                promise.SetValue(func(input));
            };
        }

        public Future GetFuture()
        {
            if (func_ == null)
            {
                throw new ArgumentNullException("no task is specified");
            }

            return future_;
        }

        public void Execute(object input)
        {
            func_(input);
        }

        public void Execute()
        {
            func_(new object());
        }

        private Future future_;
        private Action<object> func_;
    }
}
