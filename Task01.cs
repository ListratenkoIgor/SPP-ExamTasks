using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

/*
 Создать класс на языке C#, который: 
- называется TaskQueue и реализует логику пула потоков;
- создает указанное количество потоков пула в конструкторе;
- содержит очередь задач в виде делегатов без параметров:
delegate void TaskDelegate();
- обеспечивает постановку в очередь и последующее выполнение делегатов с помощью метода 
void EnqueueTask(TaskDelegate task);
 */

namespace ExamTasks
{
    public delegate void TaskDelegate();
    public class TaskQueue
    {
        private List<Thread> threads;
        private Queue<TaskDelegate> tasks;
        private int activeTaskCount;
        public TaskQueue(int threadCount)
        {
            tasks = new Queue<TaskDelegate>();
            threads = new List<Thread>();
            for (int i = 0; i < threadCount; i++)
            {
                Interlocked.Increment(ref activeTaskCount);
                var t = new Thread(DoThreadWork);
                threads.Add(t);
                t.IsBackground = true;
                t.Start();
            }
        }
        public int ThreadCount
        {
            get { return threads.Count; }
        }
        public void Close()
        {
            for (int i = 0; i < threads.Count; i++)
                DoEnqueueTask(null);
            lock (this)
            {
                while (activeTaskCount > 0)
                    Monitor.Wait(this);
            }
            foreach (Thread t in threads)
                t.Join();
        }
        public void EnqueueTask(TaskDelegate task)
        {
            if (task != null)
                DoEnqueueTask(task);
            else
                throw new ArgumentNullException("task");
        }
        private void DoEnqueueTask(TaskDelegate task)
        {
            lock (tasks)
            {
                tasks.Enqueue(task);
                Monitor.Pulse(tasks);
            }
        }
        private TaskDelegate DequeueTask()
        {
            lock (tasks)
            {
                while (tasks.Count == 0)
                    Monitor.Wait(tasks);
                TaskDelegate t = tasks.Dequeue();
                return t;
            }
        }
        private void DoThreadWork()
        {
            TaskDelegate task;
            do
            {
                task = DequeueTask();
                try
                {
                    task?.Invoke();
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            } while (task != null);
            lock (this)
            {
                activeTaskCount--;
                if (activeTaskCount == 0)
                    Monitor.Pulse(this);
            }
        }
    }
}
