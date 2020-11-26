using System;
using System.Threading;

namespace Futures
{
    // Promise is used to store a value which can be retrived in the future.
    // One can get a Future from the Promise. Promise.SetValue will fulfill the
    // promise, thus the Future will be ready and the value can be retrived.
    // A typical use case is in the async programing:
    // Thread A create a Promise and return a Future to thread B
    // Thread A fulfil the Promise
    // Thread B get the value from the ready Future.
    public class Promise
    {
        public Promise()
        {
            sharedState_ = new SharedState();
        }

        public Future GetFuture()
        {
            return new Future(sharedState_);
        }

        public void SetValue(object value)
        {
            if (sharedState_.isReady)
            {
                throw new ArgumentException("Promise already fulfilled");
            }

            sharedState_.result = value;
            sharedState_.isReady = true;
            sharedState_.Execute();
            sharedState_.doneEvent.Set();
        }

        private SharedState sharedState_;
    }
}
