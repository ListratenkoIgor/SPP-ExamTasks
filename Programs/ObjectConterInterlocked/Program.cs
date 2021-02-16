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
            //lock (sync)
            //    count += 1;
            // 0
            // MOV AX, count    0     MOV AX, count    0
            // INC AX           1     INC AX           1
            // MOV count, AX    1     MOV count, AX    1
            Interlocked.Increment(ref count);
        }

        ~ObjectCounter()
        {
            //count -= 1;
            Interlocked.Decrement(ref count);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
