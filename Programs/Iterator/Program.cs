using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace Iterator
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            foreach (int a in numbers)
            {
                Console.WriteLine(a);
            }

            IEnumerator it = numbers.GetEnumerator();
            while (it.MoveNext())
            {
                int a = (int)it.Current;
                Console.WriteLine(a);
            }

            List<int> numberList = new List<int>();
            foreach (int a in numberList)
            {
                Console.WriteLine(a);
            }

            IEnumerator<int> itl = numberList.GetEnumerator();
            while (itl.MoveNext())
            {
                int a = itl.Current;
                Console.WriteLine(a);
            }
        }

        public static void TestStringList()
        {
            var l = new StringList();

            foreach (string s in l.GetTop(10))
                Console.WriteLine(s);

            foreach (string s in l.GetTopEx(10))
                Console.WriteLine(s);

            List<string> top10 = l.GetTop(10).ToStringList();
            string[] top10Array = l.GetTop(10).ToArray();

            IEnumerable<string> enumerable = l.GetTopEx(10);
            IEnumerator<string> enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string s = enumerator.Current;
                Console.WriteLine(s);
            }
        }

        public static void TestStringListMore()
        {
            var l = new StringList();
            List<string> result = l
                .Where((string t) => !string.IsNullOrWhiteSpace(t))
                .OrderBy((string s) => s, StringComparer.CurrentCultureIgnoreCase)
                .ToList();
        }
    }

    public class StringList : List<string>
    {
        public IEnumerable<string> GetTop(int topCount)
        {
            int count = Math.Max(Count, topCount);
            for (int i = 0; i < count; i++)
            {
                yield return this[i];
            }
        }

        public IEnumerable<string> GetTopEx(int topCount)
        {
            return new StringListEnumerable(this, topCount);
        }
    }

    public class StringListEnumerable : IEnumerable<string>, IEnumerable
    {
        public StringListEnumerable(StringList stringList, int topCount)
        {
            this.stringList = stringList;
            this.topCount = topCount;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new StringListEnumerator(stringList, topCount);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private StringList stringList;
        private int topCount;
    }

    public class StringListEnumerator : IEnumerator<string>, IEnumerator
    {
        public StringListEnumerator(StringList stringList, int topCount)
        {
            this.stringList = stringList;
            this.topCount = topCount;
            currentIndex = -1;
        }

        public string Current { get { return stringList[currentIndex]; } }

        object IEnumerator.Current { get { return Current; } }

        public bool MoveNext()
        {
            currentIndex++;
            return currentIndex < stringList.Count &&
                currentIndex < topCount;
        }

        public void Reset()
        {
            currentIndex = -1;
        }

        public void Dispose()
        {
        }

        private StringList stringList;
        private int currentIndex;
        private int topCount;
    }

    public static class EnumerableExtensions
    {
        public static StringList ToStringList(
            this IEnumerable<string> enumerator)
        {
            var result = new StringList();
            foreach (string s in enumerator)
                result.Add(s);
            return result;
        }

        public static IEnumerable<T> Where2<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
