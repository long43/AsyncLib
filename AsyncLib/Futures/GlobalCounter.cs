using System;
using System.Threading;

namespace Futures
{
    public sealed class GlobalCounter
    {
        public Int64 Current
        {
            get
            {
                Interlocked.Increment(ref counter_);
                return counter_;
            }
        }

        /// Use a lambda that would be evaluated on the first call to the Instance getter 
        private static readonly Lazy<GlobalCounter> lazy = new Lazy<GlobalCounter>(() => new GlobalCounter());

        /// The lazily-loaded instance
        public static GlobalCounter Instance { get { return lazy.Value; } }

        /// Prevent instance creation outside of the class
        private GlobalCounter()
        {
            counter_ = 0;
        }

        private Int64 counter_;
    }
}
