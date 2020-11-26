using System;
using System.Threading;

using Executors;

namespace Futures
{
    class Person
    {
        public Person(int id)
        {
            this.id = id;
            this.name = "Yi Zhang";
        }
        public int id;
        public string name;
    }


    class Program
    {
        static void TestFutures()
        {
            IExecutor executor = new SingleThreadExecutor();

            Promise p = new Promise();

            Future f = p.GetFuture().Then(result =>
            {
                int ret = (int)result;
                Console.WriteLine("result is " + ret);
                ret++;
                return ret;
            }).Then(result =>
            {
                int ret = (int)result;
                Console.WriteLine("result is " + ret);
                ret++;
                return ret;
            }).Via(executor).Then(result =>
            {
                Console.WriteLine("result is " + result);
                int id = (int)result;
                return new Person(id);
            }).Via(executor).Then(result =>
            {
                Person ps = (Person)result;
                Console.WriteLine("person id is " + ps.id + " name is " + ps.name);
                return ps;
            });


            p.SetValue(1);

            Person ps = (Person)f.Get();
            Console.WriteLine("person id is " + ps.id + " name is " + ps.name);
        }

        static void TestAsyncQueue()
        {
            AsyncQueue queue = new AsyncQueue();

            //use another thread to enqueue tasks to the queue
            Thread producer = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    int n = i;
                    AsyncTask task = new AsyncTask(() =>
                    {
                        Thread.Sleep(1000);
                    });

                    queue.Enqueue(task);
                }
            });

            producer.Start();

            producer.Join();

            Future f = queue.queueEnd;

            f.Get();

        }

        static void Main(string[] args)
        {
            TestAsyncQueue();
        }
    }
}
