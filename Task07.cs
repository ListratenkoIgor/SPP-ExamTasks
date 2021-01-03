using System;
using System.Threading;
/*
 Создать на языке C# статический метод класса Parallel.WaitAll, который: 
- принимает в параметрах массив делегатов;
- выполняет все указанные делегаты параллельно с помощью пула потоков;
- дожидается окончания выполнения всех делегатов.
Реализовать простейший пример использования метода Parallel.WaitAll.
*/
namespace ExamTasks
{
    public class Parallel
    {
        public delegate void Task();
        public static void WaitAll(Task[] tasks)
        {
            using (var countdownEvent = new CountdownEvent(tasks.Length))
            {
                foreach(var task in tasks)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                    {
                        Console.WriteLine("Thread {0} start", Thread.CurrentThread.ManagedThreadId);
                        task.Invoke();
                        Console.WriteLine("Thread {0} sleep", Thread.CurrentThread.ManagedThreadId);
                        countdownEvent.Signal();
                    }));
                }
                countdownEvent.Wait();
                Console.WriteLine("All tasks are complete.");
            }
        }
        public static void FibNumberTask()
        {
            Random rnd = new Random();
            Console.WriteLine(CalculateFib(rnd.Next(20, 40)));            
        }
        public static int CalculateFib(int n)
        {
            return n <= 1 ? n : CalculateFib(n - 1) + CalculateFib(n - 2);
        }
        public static void Start()
        {
            Task[] tasks = new Task[63];
            for (int i = 0; i < tasks.Length; i++)
                tasks[i] = FibNumberTask;
            WaitAll(tasks);
        }
    }
    public class Parallel2
    {
        public delegate void Task();
        static int actionCount = 0;
        public static void WaitAll(Task[] tasks)
        {

            foreach (var task in tasks)
            {
            Interlocked.Increment(ref actionCount);
                ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                {
                    Console.WriteLine("Thread {0} start", Thread.CurrentThread.ManagedThreadId);
                    task.Invoke();
                    Console.WriteLine("Thread {0} sleep", Thread.CurrentThread.ManagedThreadId);
                    Interlocked.Decrement(ref actionCount);
                }));
            }
            while (actionCount > 0)
                Thread.Sleep(10);
            Console.WriteLine("All tasks are complete.");

        }
        public static void FibNumberTask()
        {
            Random rnd = new Random();
            Console.WriteLine(CalculateFib(rnd.Next(20, 40)));
        }
        public static int CalculateFib(int n)
        {
            return n <= 1 ? n : CalculateFib(n - 1) + CalculateFib(n - 2);
        }
        public static void Start()
        {
            Task[] tasks = new Task[63];
            for (int i = 0; i < tasks.Length; i++)
                tasks[i] = FibNumberTask;
            WaitAll(tasks);
        }
    }
    public class Parallel3
    {
        public delegate void Task();
        static object sync = new object();
        static int actionCount = 0;
        public static void WaitAll(Task[] tasks)
        {

            foreach (var task in tasks)
            {
                Interlocked.Increment(ref actionCount);
                ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
                {
                    Console.WriteLine("Thread {0} start", Thread.CurrentThread.ManagedThreadId);
                    task.Invoke();
                    Console.WriteLine("Thread {0} sleep", Thread.CurrentThread.ManagedThreadId);
                    Interlocked.Decrement(ref actionCount);
                    if (actionCount == 0)
                    {
                        lock (sync)
                        {
                            if (actionCount == 0)
                                Monitor.Pulse(sync);
                        }
                    }
                }));
            }
            lock (sync) {
                while (actionCount > 0) {
                    Monitor.Wait(sync);
                }
            }
            Console.WriteLine("All tasks are complete.");

        }
        public static void FibNumberTask()
        {
            Random rnd = new Random();
            Console.WriteLine(CalculateFib(rnd.Next(20, 40)));
        }
        public static int CalculateFib(int n)
        {
            return n <= 1 ? n : CalculateFib(n - 1) + CalculateFib(n - 2);
        }
        public static void Start()
        {
            Task[] tasks = new Task[63];
            for (int i = 0; i < tasks.Length; i++)
                tasks[i] = FibNumberTask;
            WaitAll(tasks);
        }
    }
}
