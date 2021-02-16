using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ObjectConter
{
    public class ObjectCounter
    {
        static int count = 0;
        static object sync = new object();

        public ObjectCounter()
        {
            //count = count + 1;
            //{
            //    mov AX, [count]              // 0      0
            //    inc AX                       // 1      1
            //    mov [count], AX              // 1      1
            //}

            //Monitor.Enter(sync);
            //try
            //{
            //    count -= 1;
            //}
            //finally
            //{
            //    Monitor.Exit(sync);
            //}
            lock (sync)
            {
                count += 1;
            }
        }

        ~ObjectCounter()
        {
            Monitor.Enter(sync);
            try
            {
                count -= 1;
            }
            finally
            {
                Monitor.Exit(sync);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
