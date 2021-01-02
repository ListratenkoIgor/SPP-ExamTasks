using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
/*
Создать на языке C# пользовательский атрибут с именем ExportClass, применимый только к классам, и реализовать консольную программу, которая: 
- принимает в параметре командной строки путь к сборке .NET (EXE- или DLL-файлу);
- загружает указанную сборку в память;
- выводит на экран полные имена всех public-типов данных этой сборки, помеченные атрибутом ExportClass.

*/
namespace ExamTasks
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NonExportClassAttribute : Attribute
    {
        public NonExportClassAttribute()
        {

        }
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportClassAttribute : Attribute
    {
        public ExportClassAttribute()
        {

        }
    }
    public class AttributeReader {
        public static Assembly GetAssembly(string Path)
        {
            return Assembly.LoadFrom(Path);
        }
        public static List<Type> GetTypes(Assembly assembly)
        {
            List<Type> types = assembly.GetTypes().ToList();
            return types?.Where(type=>type.IsPublic).Where(type => type.GetCustomAttributes().Any(attr => attr is ExportClassAttribute)).ToList();
        }
    }
}
