using System;

namespace Futures
{
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
            sharedState_.SetResult(value);
        }

        private SharedState sharedState_;
    }
}
