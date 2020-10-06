using System;

namespace Executors
{

    /**
     * Executor wraps the concept of the thread and has a post method that can
     * add the action to the executor. when a task was posted to the Executor: 
     * 1. try to enqueue it to a ConcurrnetQueue. 
     * 2. try to start a thread immediately to execute the task
     */
    public interface IExecutor
    {

        // execute the task
        public void Post(Action task);
    }
}
