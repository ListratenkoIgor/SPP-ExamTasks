using System;
using System.IO;
using System.Threading;
/*
 Реализовать консольную программу на языке C#, которая: 
- принимает в параметре командной строки путь к исходному и целевому каталогам на диске;
- выполняет параллельное копирование всех файлов из исходного  каталога в целевой каталог;
- выполняет операции копирования параллельно с помощью пула потоков;
- дожидается окончания всех операций копирования и выводит в консоль информацию о количестве скопированных файлов.
*/

namespace ExamTasks
{
    public class Copywriter
    {
        public int fileCounter = 0;
        private string _destPath;
        private string _soursePath;
        public Copywriter(string destPath, string soursePath)
        {
            _destPath = destPath;
            _soursePath = soursePath;
        }
        public static void WriteFile(string destDir, string sourseFilename, ref int counter)
        {
            string fileName = Path.GetFileName(sourseFilename);
            var destFile = Path.Combine(destDir, fileName);
            File.Copy(sourseFilename, destFile, true);
            counter++;
            Console.WriteLine("Thread {1} copy {0} file", fileName, Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Thread {0} end copy. Start wait", Thread.CurrentThread.ManagedThreadId);
        }
        public void Write()
        {
            Directory.CreateDirectory(_destPath);
            Directory.CreateDirectory(_soursePath);

            if (Directory.Exists(_soursePath))
            {
                string[] files = System.IO.Directory.GetFiles(_soursePath);
                using (var countdownEvent = new CountdownEvent(files.Length))
                {
                    foreach (string sourseFile in files)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(x => { WriteFile(_destPath, sourseFile, ref this.fileCounter); countdownEvent.Signal(); }));
                    }
                    countdownEvent.Wait();
                    Console.WriteLine("Thread {0} end wait", Thread.CurrentThread.ManagedThreadId);
                }
                Console.WriteLine("All threads end working. Copied {0} files", fileCounter);
            }
        }

    }
}

