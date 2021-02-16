using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace ThreadLocalVariables
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(Worker);
            ThreadPool.QueueUserWorkItem(Worker);

            Console.ReadLine();
        }

        [ThreadStatic]
        static int Id;

        static void Worker(object state)
        {
            Id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(Id);
            MyService.SecurityToken = Id.ToString();

            Console.WriteLine(MyService.SecurityToken);
            //IIdentity identity = Thread.CurrentPrincipal.Identity;
        }
    }

    class MyService
    {
        //public ThreadLocal<string> SecurityToken;

        private static LocalDataStoreSlot slot = Thread.AllocateDataSlot();

        public static string SecurityToken
        {
            get
            {
                return (string)Thread.GetData(slot);
            }
            set
            {
                Thread.SetData(slot, value);
            }
        }
    }
}
