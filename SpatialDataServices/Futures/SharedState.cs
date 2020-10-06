using System;
using System.Threading;

using Executors;

namespace Futures
{

    internal class SharedState
    {
        internal enum State
        {
            Start,
            OnlyResult,
            OnlyCallback,
            Done
        }


        internal SharedState()
        {
            state_ = State.Start;
            // default executor is ThreadedPoolExecutor
            executor_ = new ThreadedPoolExecutor();
            doneEvent = new ManualResetEvent(false);
        }

        internal void SetResult(object result)
        {
            if (state_ == State.Done)
            {
                return;
            }

            result_ = result;

            switch (state_)
            {
                case State.Start:
                    state_ = State.OnlyResult;
                    return;
                case State.OnlyCallback:
                    // immediately run callback
                    state_ = State.Done;
                    doCallback();
                    return;
                case State.OnlyResult:
                default:
                    throw new InvalidOperationException("can't setResult when state is OnlyResult");

            }
        }

        internal void SetCallback(Action<object> func)
        {
            if (state_ == State.Done)
            {
                return;
            }

            func_ = func;
            switch (state_)
            {
                case State.Start:
                    state_ = State.OnlyCallback;
                    return;
                case State.OnlyResult:
                    state_ = State.Done;
                    // immediately run the callback
                    doCallback();
                    return;
                case State.OnlyCallback:
                default:
                    throw new InvalidOperationException("can't setResult when state is OnlyResult");

            }

        }

        internal void SetExecutor(IExecutor executor)
        {
            executor_ = executor;
        }

        internal bool IsReady()
        {
            return state_ == State.Done || state_ == State.OnlyResult;
        }

        internal object GetResult()
        {
            return result_;
        }

        internal IExecutor GetExecutor()
        {
            return executor_;
        }

        private void doCallback()
        {
            // execute the callback
            executor_.Post(() =>
            {
                func_(result_);

                doneEvent.Set();
            });

            doneEvent.WaitOne();
        }

        private object result_;
        private Action<object> func_;
        private IExecutor executor_;
        private State state_;
        private ManualResetEvent doneEvent;
    }
}
