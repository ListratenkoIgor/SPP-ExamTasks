using System;
using System.Timers;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
/*
 Создать класс на языке C#, который: 
Создать класс LogBuffer, который:
- представляет собой журнал строковых сообщений;
- предоставляет метод public void Add(string item);
- буферизирует добавляемые одиночные сообщения и записывает их пачками в конец текстового файла на диске;
- периодически выполняет запись накопленных сообщений, когда их количество достигает заданного предела;
- периодически выполняет запись накопленных сообщений по истечение заданного интервала времени (вне зависимости от наполнения буфера);
- выполняет запись накопленных сообщений асинхронно с добавлением сообщений в буфер;
*/

namespace ExamTasks
{
    public class LogBuffer {
        private string _filename;
        private int _stashSize = 0;
        private int _tempStashSize = 0;
        private int _maxStashSize;
        char[] _buffer = new char[0];
        char[] _tempbuffer = new char[0];
        bool isWriting = false;
        Timer timer;
        public LogBuffer(string filename, int maxStashSize=5,double interval = 3000) {
            _filename = filename;
            _maxStashSize = maxStashSize;
            timer = new Timer(interval);
            timer.Elapsed += async (sender, e) => await HandleTimer(this, e);
            timer.Start();
        }
        ~LogBuffer() {
            timer.Close();
        }
        public int Count {
            get { return _stashSize; }
        }
        public static void ConcatStringToArray(ref char[] arr, string str) {
            int index = arr.Length;
            char[] msg = str.ToCharArray();
            Array.Resize(ref arr, arr.Length+str.Length);
            Array.Copy(msg, 0, arr, index, msg.Length);
        }
        public void Add(string item) {
            Console.WriteLine("in stash {0}",_tempStashSize+_stashSize);
            if (!isWriting)
            {
                if (_tempbuffer.Length != 0) {
                    int index = _buffer.Length;
                    Array.Resize(ref _buffer, _buffer.Length+ _tempbuffer.Length);
                    Array.Copy(_tempbuffer, 0, _buffer, index, _tempbuffer.Length);
                    _tempbuffer = new char[0];
                    _stashSize += _tempStashSize;
                    _tempStashSize = 0;
                }
                ConcatStringToArray(ref _buffer, item);
                _stashSize++;
                if (_stashSize >= _maxStashSize)
                {
                    Console.WriteLine("The stash full event was raised at "+ DateTime.Now.ToString(" HH:mm:ss.fff")+"tid {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    Task task = WriteBufferAsync();
                    task.Wait();
                }
            }
            else {
                ConcatStringToArray(ref _tempbuffer, item);
                _tempStashSize++;
            }
        }
        private Task WriteBufferTask()
        {
            Task task = new Task(async() =>
            {
                if (!isWriting)
                {
                    isWriting = true;
                    using (FileStream SourceStream = File.Open(_filename, FileMode.Append))
                    {
                        using (StreamWriter Stream = new StreamWriter(SourceStream))
                        {
                            await Stream.WriteAsync(_buffer);
                            _buffer = new char[0];
                            _stashSize = 0;
                        }
                    }
                    isWriting = false;
                    
                }
                return ;
            });
            task.Start();
            return task;
        }
        private async Task WriteBufferAsync()
        {            
            await WriteBufferTask();
        }

        private static void OnTimerElapsed(Object source, ElapsedEventArgs e) {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",e.SignalTime);
        }
        private static Task HandleTimer(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff} tid {1}", e.SignalTime,System.Threading.Thread.CurrentThread.ManagedThreadId);
            Task task = ((LogBuffer)source).WriteBufferAsync();
            task.Wait();
            return task;            
        }
    }
    public class LogBuffer1
    {
        private string _filename;
        private int _stashSize = 0;
        private int _tempStashSize = 0;
        private int _maxStashSize;
        char[] _buffer = new char[0];
        char[] _tempbuffer = new char[0];
        bool isWriting = false;
        Timer timer;
        public LogBuffer1(string filename, int maxStashSize = 5, double interval = 3000)
        {
            _filename = filename;
            _maxStashSize = maxStashSize;
            timer = new Timer(interval);
            timer.Elapsed += (sender,e)=> { OnTimerElapsed(this, e); };
            timer.Start();
        }
        ~LogBuffer1()
        {
            timer.Close();
        }
        public int Count
        {
            get { return _stashSize; }
        }
        public static void ConcatStringToArray(ref char[] arr, string str)
        {
            int index = arr.Length;
            char[] msg = str.ToCharArray();
            Array.Resize(ref arr, arr.Length + str.Length);
            Array.Copy(msg, 0, arr, index, msg.Length);
        }
        public void Add(string item)
        {
            Console.WriteLine("in stash {0}", _tempStashSize + _stashSize);
            if (!isWriting)
            {
                if (_tempbuffer.Length != 0)
                {
                    int index = _buffer.Length;
                    Array.Resize(ref _buffer, _buffer.Length + _tempbuffer.Length);
                    Array.Copy(_tempbuffer, 0, _buffer, index, _tempbuffer.Length);
                    _tempbuffer = new char[0];
                    _stashSize += _tempStashSize;
                    _tempStashSize = 0;
                }
                ConcatStringToArray(ref _buffer, item);
                _stashSize++;
                if (_stashSize >= _maxStashSize)
                {
                    Console.WriteLine("The stash full event was raised at " + DateTime.Now.ToString(" HH:mm:ss.fff") + "tid {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    Task task = WriteBufferAsync();
                    task.GetAwaiter().GetResult();
                }
            }
            else
            {
                ConcatStringToArray(ref _tempbuffer, item);
                _tempStashSize++;
            }
        }
        private async Task WriteBufferTask()
        {
            Task task = Task.Run(() =>
            {
                if (!isWriting)
                {
                    isWriting = true;
                    using (FileStream SourceStream = File.Open(_filename, FileMode.Append))
                    {
                        using (StreamWriter Stream = new StreamWriter(SourceStream))
                        {
                            Stream.Write(_buffer);
                            _buffer = new char[0];
                            _stashSize = 0;
                        }
                    }
                    isWriting = false;

                }
                return;
            });
            await task;

        }
        private async Task WriteBufferAsync()
        {
            await WriteBufferTask();
        }

        private static void OnTimerElapsed(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff} tid {1}", e.SignalTime, System.Threading.Thread.CurrentThread.ManagedThreadId);
            Task task = ((LogBuffer1)source).WriteBufferAsync();
            task.GetAwaiter().GetResult();
        }
    }
}
