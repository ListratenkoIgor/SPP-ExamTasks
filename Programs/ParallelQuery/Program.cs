using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            List<int> result = numbers
                .AsParallel()
                .WithDegreeOfParallelism(4)
                .Where(n => n > 100)
                .Select(n => n * n).ToList();
        }
    }
}
