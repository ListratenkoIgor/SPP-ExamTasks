using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaitMultipleAsyncDelegates
{
    class Program
    {
        static int ActionCount = 0;

        static void MyAction1(object state)
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(id);
            }

            Interlocked.Decrement(ref ActionCount);
        }

        static void Test1()
        {
            for (int i = 0; i < 30; i++)
            {
                Interlocked.Increment(ref ActionCount);
                ThreadPool.QueueUserWorkItem(MyAction1);
            }
            while (ActionCount > 0)
                Thread.Sleep(10);
        }

        static object sync = new object();

        static void MyAction2(object state)
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(id);
            }
            Interlocked.Decrement(ref ActionCount);
            if (ActionCount == 0)
            {
                lock (sync)
                {
                    if (ActionCount == 0)
                        Monitor.Pulse(sync);
                }
            }
        }

        static void Test2()
        {
            for (int i = 0; i < 30; i++)
            {
                Interlocked.Increment(ref ActionCount);
                ThreadPool.QueueUserWorkItem(MyAction2);
            }
            lock (sync)
            {
                while (ActionCount > 0)
                    Monitor.Wait(sync);
            }
        }

       static void Main(string[] args)
        {
            Test1();
            Test2();
            Console.ReadLine();
        }
    }
}
