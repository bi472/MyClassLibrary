using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MyClassLibrary;

namespace MyApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "input.txt";
            string resultFilePath = "output.txt";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Input file '{filePath}' not found.");
                return;
            }

            // Чтение текста из файла
            string text = File.ReadAllText(filePath);

            // Создание экземпляра класса из DLL
            var classInstance = new MyClass();
            var type = typeof(MyClass);
            var method = type.GetMethod("ProcessText", BindingFlags.NonPublic | BindingFlags.Instance);

            Dictionary<string, int> result = (Dictionary<string, int>)method.Invoke(classInstance, new object[] { text }); ;

            // Вызов публичного метода, реализующий обработку текста используя потоки
            Dictionary<string, int> parallelResult = classInstance.ProcessTextParallel(text);

            List<KeyValuePair<string, int>> sortedWordCounts = parallelResult.ToList();
            sortedWordCounts.Sort((x, y) => y.Value.CompareTo(x.Value));

            // Запись результата в файл
            using (StreamWriter writer = new StreamWriter(resultFilePath))
            {
                foreach (var entry in sortedWordCounts)
                {
                    writer.WriteLine($"{entry.Key}: {entry.Value}");
                }
            }

            Console.WriteLine("Обработка текста завершена. Результат записан в файл.");
            Console.ReadLine();
        }
    }
}
