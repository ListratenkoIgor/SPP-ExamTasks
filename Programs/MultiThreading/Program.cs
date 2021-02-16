using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiThreading
{
    class Program
    {
        static object sync = new object();

        public static void DoWork(object obj)
        {
            //for (int i = 0; i < 1000; ++i)
            //{
            //    Console.Write(" {0}", obj);
            //}
            Thread.Sleep(2000);
        }

        static void DedicatedThreads()
        {
            Thread t1 = new Thread(DoWork, 16 * 1024);
            t1.Start("o");
            Thread t2 = new Thread(DoWork);
            t2.Start("X");
        }

        static void ThreadPoolThreads()
        {
            ThreadPool.QueueUserWorkItem(DoWork, "o");
            ThreadPool.QueueUserWorkItem(DoWork, "X");
        }

        static void ThousandThreads()
        {
            for (int i = 0; i < 40000; i++)
            {
                ThreadPool.QueueUserWorkItem(DoWork, i);
            }
        }

        static void Main(string[] args)
        {
            //DedicatedThreads();
            //ThreadPoolThreads();
            ThousandThreads();

            int workerThreads;
            int completionThreads;
            
            ThreadPool.GetMinThreads(out workerThreads, out completionThreads);
            Console.WriteLine("Min WorkerThreads = {0}, CompletionThreads = {1}",
                workerThreads, completionThreads);

            ThreadPool.SetMinThreads(100, 100);

            ThreadPool.GetMaxThreads(out workerThreads, out completionThreads);
            Console.WriteLine("Max WorkerThreads = {0}, CompletionThreads = {1}",
                workerThreads, completionThreads);

            for (int i = 0; i < 100; ++i)
            {
                int w;
                int c;
                ThreadPool.GetAvailableThreads(out w, out c);
                Console.WriteLine("Available WorkerThreads = {0}, CompletionThreads = {1}",
                    workerThreads - w, completionThreads - c);
                Thread.Sleep(1000);
            }
            Console.ReadLine();
        }


        public static List<int> List1 = new List<int>();
        public static List<int> List2 = new List<int>();

        public static void DoWork1()
        {
            int a = 10;
            lock (List1)
            {
                // переключение задач
                lock (List2) // ожидание... 
                {
                    List1.Add(a);
                    List2.Add(a);
                }
            }
        }

        public static void DoWork2()
        {
            int b = 5;
            lock (List2)
            {
                lock (List1) // ожидание... переключение задач
                {
                    List1.Add(b);
                    List2.Add(b);
                }
            }
        }

        public static void DoWork(int value, TimeSpan timeout)
        {
            //lock (List1)
            //    List1.Add(value);

            if (Monitor.TryEnter(List1, timeout))
            {
                try
                {
                    List1.Add(value);
                }
                finally
                {
                    Monitor.Exit(List1);
                }
            }
        }
    }
}
