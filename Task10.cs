using System;
using System.IO;
using System.Threading;
/*
 Создать класс на языке C#, который:
- называется LogFile и обеспечивает добавление указанных строк в
текстовый журнал работы программы;
- открывает файл в конструкторе;
- реализует интерфейс IDisposable и позволяет принудительно
закрыть файл с помощью метода Dispose;
- реализует метод Write(string str), записывающий указанную строку
в формате <год-месяцдень><пробел><часы:минуты:секунды.миллисекунды>
<пробел><номер программного потока, вызвавшего метод><пробел><str>
*/
namespace ExamTasks
{
    public class LogFile : IDisposable
    {
        private bool disposed = false;
        private StreamWriter writer;
        private string path;
        public LogFile(string path)
        {
            this.path = path;
            writer = new StreamWriter(path,true);
        }
        public void Write(string str)
        {
            string time = DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss");
            writer.WriteLine(string.Format("{0} {1} {2}", time,
            Thread.CurrentThread.ManagedThreadId, str));
        }
        public void Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                disposed = true;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                writer.Close();
                writer.Dispose();
                disposed = true;
            }
        }
    }
}
