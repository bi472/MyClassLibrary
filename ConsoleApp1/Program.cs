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

            Stopwatch stopwatch = Stopwatch.StartNew();

            Dictionary<string, int> result = (Dictionary<string, int>)method.Invoke(classInstance, new object[] { text }); ;

            stopwatch.Stop();

            Console.WriteLine("Время выполнения приватного метода: " + stopwatch.Elapsed);

            stopwatch.Restart();

            // Вызов публичного метода, реализующий обработку текста используя потоки
            Dictionary<string, int> parallelResult = classInstance.ProcessTextParallel(text);

            stopwatch.Stop();

            Console.WriteLine("Время выполнения публичного метода: " + stopwatch.Elapsed);


            // Запись результата в файл
            using (StreamWriter writer = new StreamWriter(resultFilePath))
            {
                foreach (var entry in parallelResult)
                {
                    writer.WriteLine($"{entry.Key}: {entry.Value}");
                }
            }

            Console.WriteLine("Обработка текста завершена. Результат записан в файл.");
            Console.ReadLine();
        }
    }
}
