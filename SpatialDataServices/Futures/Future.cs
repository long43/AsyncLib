using System;

using Executors;

namespace Futures
{
    public class Future
    {
        // private method to make sure Future can only create by MakeFuture and
        // Promise
        internal Future()
        {
            sharedState_ = new SharedState();
        }

        internal Future(SharedState state)
        {
            sharedState_ = state;
        }


        // create a fullfilled Future
        public static Future MakeFuture(object value)
        {
            Future future = new Future(new SharedState());
            future.sharedState_.SetResult(value);
            return future;
        }

        public bool IsReady()
        {
            return sharedState_.IsReady();
        }

        public object Get()
        {
            Promise promise = new Promise();

            Future ret = promise.GetFuture();

            Action<object> action = (result) =>
            {
                promise.SetValue(result);
            };

            SetCallback(action);

            // block to wait
            while (!IsReady()) { }

            return sharedState_.GetResult();
        }

        public Future Then(Action<object> func)
        {
            Promise p = new Promise();

            Future f = p.GetFuture();

            this.SetCallback((input) =>
            {
                func(input);
                p.SetValue(new Object());
            });


            return f;
        }

        public Future Then(Func<object, object> func)
        {
            Promise p = new Promise();

            Future f = p.GetFuture();

            this.SetCallback((input) =>
            {
                Object result = func(input);
                p.SetValue(result);
            });


            return f;
        }

        public Future Via(IExecutor executor)
        {
            sharedState_.SetExecutor(executor);
            return this;
        }

        private void SetCallback(Action<object> action)
        {
            sharedState_.SetCallback(action);
        }


        private SharedState sharedState_;
    }
}
