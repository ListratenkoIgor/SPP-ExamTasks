using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace InterlockedSample
{
    class Mutex
    {
        private Thread currentThread;

        void Lock()
        {
            Thread thread = Thread.CurrentThread;

            while (Interlocked.CompareExchange(ref currentThread, thread, null) != null)
            {

            }
        }

        void Unlock()
        {
            Interlocked.Exchange(ref currentThread, null);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }

        public static int LockingTread = -1;

        public static void DoWork()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            while (Interlocked.CompareExchange(
                ref LockingTread, threadId, -1) != -1)
            {
            }
            // Действия
            Interlocked.Exchange(ref LockingTread, -1);
        }

        public static int CompareExchange(
            ref int location, int value, int comparand)
        {
            int result = location;
            if (location == comparand)
                location = value;
            return result;
        }
    }

    public class User
    {
        public string Name;
        public string Email;
        public bool IsAdministrator; 
    }

    public class UserHolder
    {
        public User Value;

        public void Update(string name, string email)
        {
            User v = Value;
            //User v = (User)Thread.VolatileRead(ref Value);
            // v = Value;
            // Thread.MemoryBarrier();

            var newUser = new User()
            {
                Name = name,
                Email = email,
                IsAdministrator = v.IsAdministrator
            };
            Interlocked.Exchange(ref Value, newUser);
            
            //Thread.VolatileWrite(ref Value, newUser);
            // Thread.MemoryBarrier();
            // Value = v;
        }

        public void Update(string name)
        {
            User v;
            User newUser;
            do
            {
                v = Value;
                newUser = new User()
                {
                    Name = name,
                    Email = v.Email,
                    IsAdministrator = v.IsAdministrator
                };
            }
            while (Interlocked.CompareExchange(ref Value, newUser, v) != v);
        }

        public bool Update(string name, int attempts)
        {
            User v;
            User newUser;
            while (attempts > 0)
            {
                v = Value;
                newUser = new User()
                {
                    Name = name,
                    Email = v.Email,
                    IsAdministrator = v.IsAdministrator
                };
                if (Interlocked.CompareExchange(ref Value, newUser, v) == v)
                    break;
                else
                    attempts -= 1;
            }
            return attempts != 0;
        }
    }
}
