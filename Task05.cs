using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
/*
Реализовать консольную программу на языке C#, которая: 
- принимает в параметре командной строки путь к сборке .NET (EXE- или DLL-файлу);
-загружает указанную сборку в память;
-выводит на экран полные имена всех public -типов данных этой сборки, упорядоченные по пространству имен (namespace) и по имени.
*/

namespace ExamTasks
{
    public static class AssemblyReader {
        public static Assembly GetAssembly(string Path) { 
            return Assembly.LoadFrom(Path);        
        }
        public static List<Type> GetTypes(Assembly assembly) { 
            List<Type> types = assembly.GetTypes().ToList();
            return types?.OrderBy(x => (x.Namespace)).ThenBy(x => (x.Name)).Where(x=>x.IsPublic)?.ToList();   
        }
    }
}
