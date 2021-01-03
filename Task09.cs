using System;
using System.Collections;
using System.Linq;

/*
Создать на языке C# обобщенный (generic-) класс DynamicList<T>, который:
- реализует динамический массив с помощью обычного массива T[];
- имеет свойство Count, показывающее количество элементов; 
- имеет свойство Items для доступа к элементам по индексу; 
- имеет методы Add, Remove, RemoveAt, Clear для соответственно добавления, удаления, удаления по индексу и удаления всех элементов;
- реализует интерфейс IEnumerable<T>.
Реализовать простейший пример использования класса DynamicList<T> на языке C#.
*/
namespace ExamTasks
{
    public class DynamicList<T> : IEnumerable
    {
        private int capacity;
        private int count;
        private T[] array;
        public DynamicList(int capacity = 1)
        {
            this.capacity = capacity;
            array = new T[capacity];
            count = 0;
        }        
        public int Count
        {
            get { return count; }
        }
        public T this[int index]
        {
            get { return array[index]; }
            set { array[index] = value; }
        }
        private T[] ExtendAndCopy(T[] array)
        {
            capacity *= 2;
            T[] newArray = new T[capacity];
            for (int i = 0; i < array.Length; i++)
                newArray[i] = array[i];
            return newArray;
        }
        public void Add(T item)
        {
            array[count] = item;
            count++;
            if(count == capacity)
                array = ExtendAndCopy(array);
        }
        public void RemoveAt(int index)
        {
            if ((index > 0) &&(index < count)){
                Array.Copy(array, index + 1, array, index, count - index -1);
            }
            count--;
        }
        public void Remove(T item)
        {
            int numIndex = Array.IndexOf(array, item);
            RemoveAt(numIndex);
        }
        public void Clear()
        {
            array = new T[capacity];
            count = 0;
        }

        public IEnumerator GetEnumerator()
        {
            return new DynamicListEnum<T>(array, Count);
        }
        public IEnumerator GetEnumeratorEx()
        {
            for (int i = 0; i < count; i++) {
                yield return array[i];
            }
        }
    }
    public class DynamicListEnum<T> : IEnumerator
    {
        private T[] _array;
        private int _count;
        int position = -1;

        public DynamicListEnum(T[] array,int count)
        {
            _array = array;
            _count = count;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _count);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public T Current
        {
            get
            {
                return _array[position];
            }
        }
    }
}
