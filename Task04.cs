using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
/*
 Создать класс на языке C#, который: 
- называется OSHandle и обеспечивает автоматическое или принудительное освобождение заданного дескриптора операционной системы;
- содержит свойство Handle, позволяющее установить и получить дескриптор операционной системы; 
- реализует метод Finalize для автоматического освобождения дескриптора;
- реализует интерфейс IDisposable для принудительного освобождения дескриптора; 
*/
namespace ExamTasks
{

    public abstract class OSHandle : CriticalFinalizerObject, IDisposable
    {        
        private IntPtr handle;
        [DllImport("kernel32")]
        private static extern bool CloseHandle(IntPtr handle); private bool disposed = false;
        public OSHandle(IntPtr handle)
        {
            this.handle = handle;
        }
        ~OSHandle()
        {
            Dispose(false);
        }
        public IntPtr Handle
        {
            get
            {
                if (!disposed)
                    return handle;
                else
                    throw new ObjectDisposedException(ToString());
            }
            set
            {
                if (!disposed)
                    handle = value;
                else
                    throw new ObjectDisposedException(ToString());
            }
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
            if (handle != IntPtr.Zero)
                CloseHandle(handle);
            if (disposing)
            {
                //Dispose
            }
            else
            {
                //Finalize
            }
        }

    }
    //интерфейса IDisposable сигнализирует пользователям
    // этого класса о том, что он поддерживает эталон освобождения ресурсов
    public abstract class SafeHandle : CriticalFinalizerObject, IDisposable
    {
        // Этот открытый метод можно вызвать, чтобы наверняка
        // уничтожить ресурс. Он реализует метод Dispose интерфейса IDisposable
        public void Dispose()
        {
            //Вызов метода, реально выполняющего очистку
            Dispose(true);
        }
        // Этот открытый метод можно вызвать в место Dispose
        public void Close()
        {
            Dispose(true);
        }
        // При сборке мусора этот метод финализации вызывается, чтобы закрыть ресурс
        ~SafeHandle()
        {
            //Вызов метода, реально выполняющего очистку
            Dispose(false);
        }
        //Этот общий метод реально выполняет очистку. Его вызывают метод финализации, 
        //а так же методы Dispose и Close. Поскольку этот класс неизолированный, метод 
        //является защищенным и виртуальным. Если бы класс был изолирован, метод был 
        //бы закрытым
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                //Объект явно уничтожается или закрывается, но не финализируется
                //Поэтому в этой условной инструкции обращение к полям,
                //ссылающимся на другие объекты, безопасно для кода, тк
                //метод финализации этих объектов еще не вызывался
                //Классу SafeHandle здесь делать ничего не нужно
            }
            //Выполняется уничтожение\закрытие или финализация объекта.
            //А происходит вот что: Если ресурс уже освобожден, просто воозвращается 
            //управление.
            //Если значение owsHandle равно false, возвращается управление.
            //Устанавливается флаг, указывающий, что данный ресурс был освобожден.
            //Вызов виртуального метода ReleaseHandle.
            //Вызов GC.SuppressFinalize(this), запрещающий вызов метода финализации.
        }
    }
}