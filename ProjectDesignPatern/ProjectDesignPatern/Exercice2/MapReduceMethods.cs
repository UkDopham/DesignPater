using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProjectDesignPatern.Exercice2
{
    class MapReduceMethods
    {
        public static IList<KeyValuePair<string, int>> Map_Words(string key, string value)
        {
            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();

            value = Regex.Replace(value, @"[^a-zA-Z0-9\-]", " ");
            foreach (var word in value.Split(' '))
            {
                if (word.Length != 0)
                    result.Add(new KeyValuePair<string, int>(word, 1));
            }
            return result;
        }

        public static IEnumerable<int> Reduce(string key, IEnumerable<int> values)
        {
            int sum = 0;
            foreach (int value in values)
            {
                sum += value;
            }
            return new int[1] { sum };
        }



        public static IList<KeyValuePair<string, int>> Map_Chars(string key, string value)
        {
            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();
            foreach (var word in value.Split(' '))
            {
                foreach (char letter in word)
                {
                    result.Add(new KeyValuePair<string, int>($"{letter}", 1));
                }
            }
            return result;
        }
    }
}
