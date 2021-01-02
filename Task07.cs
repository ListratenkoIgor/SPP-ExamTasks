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
        public static int CalculateFib(int n)
        {
            return n <= 1 ? n : CalculateFib(n - 1) + CalculateFib(n - 2);
        }
        public static void FibNumberTask()
        {
            Random rnd = new Random();
            Console.WriteLine(CalculateFib(rnd.Next(20, 40)));            
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
