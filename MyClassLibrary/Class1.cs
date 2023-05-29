using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class MyClass
    {

        public Dictionary<string, int> ProcessTextParallel(string text)
        {
        
            Stopwatch stopwatch = Stopwatch.StartNew();

            ConcurrentDictionary<string, int> wordCount = new ConcurrentDictionary<string, int>();

            char[] separators = { ' ', ',', '.', ':', ';', '(', ')', '[', ']', '-', '!', '?', '\t', '\n', '\r' };

            string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var tasks = new Task[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                // Разбиваем строку на слова
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                tasks[i] = Task.Run(() =>
                {
                    // Считаем количество каждого слова в строке
                    foreach (string word in words)
                    {
                        wordCount.AddOrUpdate(word, 1, (_, count) => count + 1);
                    }
                });
            }

            // Ожидаем завершения всех задач
            Task.WaitAll(tasks);

            stopwatch.Stop();
            Console.WriteLine("Время выполнения публичного метода с многопоточной обработкой: " + stopwatch.Elapsed);

            return new Dictionary<string, int>(wordCount);
        }


        private Dictionary<string, int> ProcessText(string text)

        {

            // Измерение времени выполнения публичного метода с помощью объекта Stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            Dictionary<string, int> wordCounts = new Dictionary<string, int>();

            string[] words = text.Split(new char[] { ' ', ',', '.', ':', ';', '(', ')', '[', ']', '-', '!', '?', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                string cleanWord = word.ToLowerInvariant();
                if (!wordCounts.ContainsKey(cleanWord))
                {
                    wordCounts[cleanWord] = 0;
                }
                wordCounts[cleanWord]++;
            }

            stopwatch.Stop();
            Console.WriteLine("Время выполнения приватного метода: " + stopwatch.Elapsed);

            return wordCounts;
        }
    }
}
