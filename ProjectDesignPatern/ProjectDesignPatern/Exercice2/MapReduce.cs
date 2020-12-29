using ProjectDesignPatern.Exercice2.IPC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectDesignPatern.Exercice2
{
    class MapReduce<K1, V1, K2, V2, V3>
    {
        /**
         * static methods : 
         * Initialize server pipe 
         * 
         * Create Threads with tasks
         *  -> initialize pipe
         *  -> do work
         *  -> return result
         *  
         * Get Results 
         * 
         * 
         * instance methods :
         * Ctor
         *  -> Create Nodes
         *  
         * Splitting
         * Mapping
         * Shuffling
         * Reducing
         * GetResults
         * 
         */
        private static object lockvar = new object();
        private static int cptQ = 0;

        public delegate IEnumerable<KeyValuePair<K2, V2>> MapFunction(K1 key, V1 value);
        public delegate IEnumerable<V3> ReduceFunction(K2 key, IEnumerable<V2> values);

        private ConcurrentQueue<KeyValuePair<K2, V2>> mapResult;

        private MapFunction _map;
        private ReduceFunction _reduce;
        public List<Node> nodes;
        public Thread[] threads;

        ServerPipe serverPipe;

        public MapReduce(int numberOfNodes, MapFunction mapFunction, ReduceFunction reduceFunction)
        {
            this.nodes = new List<Node>();
            this.threads = new Thread[numberOfNodes];

            for (int i = 0; i < numberOfNodes; i++)
            {
                this.nodes.Add(new Node($"clientnode{i}"));
            }

            _map = mapFunction;
            _reduce = reduceFunction;


        }
        public IEnumerable<KeyValuePair<K2, V2>> NodeMap(IEnumerable<KeyValuePair<K1, V1>> input)
        {
            List<IEnumerable<KeyValuePair<K1, V1>>> inputSplitted =
               DataProcess<K1, V1>.SplitInputSimple2(input, this.threads.Length).ToList();
            if (this.threads.Length != inputSplitted.Count)
                throw new Exception("Data not splitted rigth!");

            // Creation queue des resultats
            ConcurrentQueue<KeyValuePair<K2, V2>> mapResult = new ConcurrentQueue<KeyValuePair<K2, V2>>();

            // Initialisation des threads
            int temp = 0;
            ConcurrentQueue<int> temp3 = new ConcurrentQueue<int>();
            for (int i = 0; i < this.threads.Length; i++)
            {
                temp++;
                temp3.Enqueue(i);
                //Console.WriteLine($"i:{i}  cond:{i < this.threads.Length}  length:{this.threads.Length}");
                threads[i] = new Thread(() => { int test ; while(!temp3.TryDequeue(out test)); this.Map2(inputSplitted[test], mapResult); Console.WriteLine($"temp: {test}"); });
            }

            // Depart threads
            for (int i = 0; i < this.threads.Length; i++)
            {
                threads[i].Start();
            }

            // Attente fin threads
            for (int i = 0; i < this.threads.Length; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine($"\n\nNodeMap :\nSize = {input.Count()}   Nb of threads = {this.threads.Length}    DataSplited = {inputSplitted.Count()}");
            inputSplitted.ForEach(x => Console.Write(x.Count() + " "));
            Console.WriteLine($"\nSize result = {mapResult.Count}");

            return mapResult;
        }
        public IEnumerable<KeyValuePair<K2, V3>> NodeReduce(IEnumerable<KeyValuePair<K2, V2>> input)
        {
            IEnumerable<IGrouping<K2, V2>> groups = this.Shuffling2(input);
            //int s = groups.Count();

            List<IEnumerable<IGrouping<K2, V2>>> inputSplitted =
                    DataProcess<K2, V2>.SplitInputSimple2(groups, this.threads.Length).ToList();
            if (this.threads.Length != inputSplitted.Count)
                throw new Exception("Data not splitted rigth!");

            // Creation queue des resultats
            ConcurrentQueue<KeyValuePair<K2, V3>> reduceResult = new ConcurrentQueue<KeyValuePair<K2, V3>>();


            // Initialisation des threads

            int[] temp = new int[this.threads.Length];
            ConcurrentQueue<int> temp3 = new ConcurrentQueue<int>();
            for (int i = 0; i < this.threads.Length; i++)
            {
                temp3.Enqueue(i);
                threads[i] = new Thread(() => { int test; while (!temp3.TryDequeue(out test)) ; this.Reduce2(inputSplitted[test], reduceResult); });
            }

            // Depart threads
            for (int i = 0; i < this.threads.Length; i++)
                threads[i].Start();

            // Attente fin threads
            for (int i = 0; i < this.threads.Length; i++)
                threads[i].Join();


            Console.WriteLine($"\nNodeReduce :\nSize = {groups.Count()}   Nb of threads = {this.threads.Length}    DataSplited = {inputSplitted.Count()}");
            inputSplitted.ForEach(x => Console.Write(x.Count() + " "));
            Console.WriteLine($"\nSize result = {reduceResult.Count}");


            return reduceResult;


        }


        private IEnumerable<KeyValuePair<K2, V2>> Map(IEnumerable<KeyValuePair<K1, V1>> input)
        {
            var q = from pair in input
                    from mapped in _map(pair.Key, pair.Value)
                    select mapped;

            return q;
        }

        private IEnumerable<KeyValuePair<K2, V3>> Reduce(IEnumerable<KeyValuePair<K2, V2>> intermediateValues)
        {
            // First, group intermediate values by key 
            var groups = from pair in intermediateValues
                         group pair.Value by pair.Key into g
                         select g;   // Reduce on each group 
            var reduced = from g in groups
                          let k2 = g.Key
                          from reducedValue in _reduce(k2, g)
                          select new KeyValuePair<K2, V3>(k2, reducedValue);

            return reduced;
        }

        public IEnumerable<KeyValuePair<K2, V3>> Execute(IEnumerable<KeyValuePair<K1, V1>> input)
        {
            return Reduce(Map(input));
        }

        public IEnumerable<KeyValuePair<K2, V3>> NodeExecute(IEnumerable<KeyValuePair<K1, V1>> input)
        {
            return NodeReduce(NodeMap(input));
        }


        private void Map2(IEnumerable<KeyValuePair<K1, V1>> input, ConcurrentQueue<KeyValuePair<K2, V2>> queue)
        {
            var q = from pair in input
                    from mapped in _map(pair.Key, pair.Value)
                    select mapped;


            foreach (var item in q)
            {
                lock (lockvar)
                {
                    queue.Enqueue(item);
                    cptQ++;
                }
            }

        }

        private IEnumerable<IGrouping<K2, V2>> Shuffling2(IEnumerable<KeyValuePair<K2, V2>> intermediateValues)
        {
            var groups = from pair in intermediateValues
                         group pair.Value by pair.Key into g
                         select g;   // Reduce on each group 

            return groups;
        }
        private void Reduce2(IEnumerable<IGrouping<K2, V2>> groups, ConcurrentQueue<KeyValuePair<K2, V3>> queue)
        {

            var reduced = from g in groups
                          let k2 = g.Key
                          from reducedValue in _reduce(k2, g)
                          select new KeyValuePair<K2, V3>(k2, reducedValue);

            foreach (var item in reduced)
            {
                queue.Enqueue(item);
            }
        }









        private static int[][] SplitInputSimple(int[] inputData, int n)
        {
            int sizeArrayMax = inputData.Length / n;
            int index = 0;

            int[][] res = new int[n][];


            for (int i = 0; i < n; i++)
            {
                res[i] = new int[(inputData.Length - index > sizeArrayMax) ? sizeArrayMax : inputData.Length - index];
                for (int j = 0; j < res[i].Length; j++)
                {
                    res[i][j] = inputData[index];
                    index++;
                }
            }
            return res;
        }








    }

    public static class MapReduceMethods
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
