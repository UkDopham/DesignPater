using System.Collections.Generic;

namespace ProjectDesignPatern.Exercice2
{
    class MapReduceMethods
    {
        public static IList<KeyValuePair<string, int>> MapFromMem(string key, string value)
        {
            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();
            foreach (var word in value.Split(' '))
            {
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
    }
}
