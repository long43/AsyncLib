using System;

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
        static void Main(string[] args)
        {
            IExecutor executor = new SingleThreadExecutor();

            Future f = Future.MakeFuture(1).Then(result =>
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
               int id = (int)result;
               return new Person(id);
           });


            Person value = (Person)f.Get();

            //int state = 1;
            //ManualResetEvent _doneEvent = new ManualResetEvent(false);

            //ThreadPool.QueueUserWorkItem((input) =>
            //{
            //    Console.WriteLine("hello world");
            //    state = 2;
            //    _doneEvent.Set();
            //});

            //_doneEvent.WaitOne();

            Console.WriteLine("person name is " + value.name + " and id is " + value.id);
            Console.WriteLine();
        }
    }
}
