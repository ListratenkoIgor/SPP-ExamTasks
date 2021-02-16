using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace ConcurrentCollections
{
    class Program
    {
        static ConcurrentDictionary<int, string> CashedData =
            new ConcurrentDictionary<int, string>();

        static void Main(string[] args)
        {
        }

        static void ReadData(object state)
        {
            string str;
            if (CashedData.TryGetValue(5, out str))
                Console.WriteLine(str);
        }

        static void WriteData(object state)
        {
            string str = "абракадабра";
            CashedData.AddOrUpdate(5, str,
                (int key, string oldValue) => { return str; });
        }

        static void DoWriteOnceWork(object state)
        {
            string str = CashedData.GetOrAdd(5, "абракадабра");
            Console.WriteLine(str);
        }
    }
}
