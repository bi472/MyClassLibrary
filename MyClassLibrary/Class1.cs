using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class MyClass
    {

        public Dictionary<string, int> ProcessTextParallel(string text)
        {

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

            List<KeyValuePair<string, int>> sortedWordCounts = new Dictionary<string, int>(wordCount).ToList();
            sortedWordCounts.Sort((x, y) => y.Value.CompareTo(x.Value));

            Dictionary<string, int> sortedWordCountsDictionary = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> kvp in sortedWordCounts)
            {
                sortedWordCountsDictionary.Add(kvp.Key, kvp.Value);
            }

            return sortedWordCountsDictionary;
        }


        private Dictionary<string, int> ProcessText(string text)

        {
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

            List<KeyValuePair<string, int>> sortedWordCounts = new Dictionary<string, int>(wordCounts).ToList();
            sortedWordCounts.Sort((x, y) => y.Value.CompareTo(x.Value));

            Dictionary<string, int> sortedWordCountsDictionary = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> kvp in sortedWordCounts)
            {
                sortedWordCountsDictionary.Add(kvp.Key, kvp.Value);
            }

            return sortedWordCountsDictionary;
        }
    }
}
