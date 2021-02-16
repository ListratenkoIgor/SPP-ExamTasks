using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VolatileVariables
{
    public class CopyParams
    {
        public string[] Files;
        public string Destination;
    }

    class Program
    {
        static void Main(string[] args)
        {
            CopyParams copyArgs = new CopyParams();

            // args.Files = 
            // args.Destination = 

            ThreadPool.QueueUserWorkItem(Copy, copyArgs);
            //...
            Stopped = true;
        }

        public static volatile bool Stopped = false;

        public static void Copy(object state)
        {
            var args = (CopyParams)state;
            foreach (var f in args.Files)
            {
                if (!Stopped)
                {
                    Copy(f, args.Destination);
                }
                else
                    break;
            }
        }

        public static void Copy(string file, string destination)
        {
            // ...
        }
    }
}
