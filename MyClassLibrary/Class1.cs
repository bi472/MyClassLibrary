using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Class1
    {

        public Dictionary<string, int> ProcessTextParallel(string text)
        {
            // Разбиение текста на отдельные части
            string[] parts = text.Split(new char[] { ' ', ',', '.', ':', ';', '(', ')', '[', ']', '-', '!', '?', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); ;

            // Создание результирующего словаря
            Dictionary<string, int> result = new Dictionary<string, int>();

            // Запуск обработки каждой части текста в отдельном потоке
            Parallel.ForEach(parts, part =>
            {
                // Обработка текущей части текста
                Dictionary<string, int> partialResult = ProcessText(part);

                // Добавление результатов в результирующий словарь
                lock (result)
                {
                    foreach (var entry in partialResult)
                    {
                        if (result.ContainsKey(entry.Key))
                        {
                            result[entry.Key] += entry.Value;
                        }
                        else
                        {
                            result[entry.Key] = entry.Value;
                        }
                    }
                }
            });

            return result;
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

            return wordCounts;
        }
    }
}
