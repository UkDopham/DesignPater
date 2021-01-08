using System.Collections.Generic;
using System.Linq;

namespace ProjectDesignPatern.Exercice2
{
    public class DataSplitter<T, U>
    {
        public static IEnumerable<IEnumerable<KeyValuePair<T, U>>> Splitting(IEnumerable<KeyValuePair<T, U>> inputData, int n)
        {
            int num = inputData.Count() / n;
            if ((inputData.Count() % n != 0)) num += 1;

            return inputData
                     .Select((x, i) => new { Index = i, Value = x })
                     .GroupBy(x => x.Index / num)
                     .Select(x => x.Select(v => v.Value).ToList());
        }
        public static IEnumerable<IEnumerable<IGrouping<T, U>>> Splitting(IEnumerable<IGrouping<T, U>> inputData, int n)
        {
            int num = inputData.Count() / n;
            if ((inputData.Count() % n != 0)) num += 1;

            return inputData
                       .Select((x, i) => new { Index = i, Value = x })
                       .GroupBy(x => x.Index / num)
                       .Select(x => x.Select(v => v.Value).ToList());
        }


    }
}
