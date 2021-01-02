using System.Threading;

/*
 Создать класс на языке C#, который: 
- называется Mutex и реализует двоичный семафор с помощью атомарной операции Interlocked.CompareExchange. 
- обеспечивает блокировку и разблокировку двоичного семафора с помощью public-методов Lock и Unlock.
*/
namespace ExamTasks
{
    public class Mutex
    {
        private Thread lockingThread;
        public void Lock()
        {
            while (Interlocked.CompareExchange(ref lockingThread, Thread.CurrentThread, null) != null)
            {
                Thread.Sleep(1);
            }
        }
        public void Unlock()
        {
            Interlocked.Exchange(ref lockingThread, null);
        }
    }
}
