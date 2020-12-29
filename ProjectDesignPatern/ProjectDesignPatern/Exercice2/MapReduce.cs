using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

        public Thread[] threads;


        public MapReduce(int numberOfNodes, MapFunction mapFunction, ReduceFunction reduceFunction)
        {
            this.threads = new Thread[numberOfNodes];

            _map = mapFunction;
            _reduce = reduceFunction;
        }
        public IEnumerable<KeyValuePair<K2, V2>> NodeMap(IEnumerable<KeyValuePair<K1, V1>> input)
        {
            List<IEnumerable<KeyValuePair<K1, V1>>> inputSplitted =
               DataProcess<K1, V1>.SplitInputSimple2(input, this.threads.Length).ToList();

            if (this.threads.Length != inputSplitted.Count)
                throw new Exception("Data not splitted right!");

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
                threads[i] = new Thread(() => { int test; while (!temp3.TryDequeue(out test)) ; this.Map(inputSplitted[test], mapResult); Console.WriteLine($"temp: {test}"); });
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
            IEnumerable<IGrouping<K2, V2>> groups = this.Shuffling(input);
            //int s = groups.Count();

            List<IEnumerable<IGrouping<K2, V2>>> inputSplitted =
                    DataProcess<K2, V2>.SplitInputSimple2(groups, this.threads.Length).ToList();
            if (this.threads.Length != inputSplitted.Count)
                throw new Exception("Data not splitted right!");

            // Creation queue des resultats
            ConcurrentQueue<KeyValuePair<K2, V3>> reduceResult = new ConcurrentQueue<KeyValuePair<K2, V3>>();


            // Initialisation des threads

            int[] temp = new int[this.threads.Length];
            ConcurrentQueue<int> temp3 = new ConcurrentQueue<int>();
            for (int i = 0; i < this.threads.Length; i++)
            {
                temp3.Enqueue(i);
                threads[i] = new Thread(() => { int test; while (!temp3.TryDequeue(out test)) ; this.Reduce(inputSplitted[test], reduceResult); });
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


        public IEnumerable<KeyValuePair<K2, V3>> NodeExecute(IEnumerable<KeyValuePair<K1, V1>> input)
        {
            return NodeReduce(NodeMap(input));
        }


        private void Map(IEnumerable<KeyValuePair<K1, V1>> input, ConcurrentQueue<KeyValuePair<K2, V2>> queue)
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

        private IEnumerable<IGrouping<K2, V2>> Shuffling(IEnumerable<KeyValuePair<K2, V2>> intermediateValues)
        {
            var groups = from pair in intermediateValues
                         group pair.Value by pair.Key into g
                         select g;   // Reduce on each group 

            return groups;
        }
        private void Reduce(IEnumerable<IGrouping<K2, V2>> groups, ConcurrentQueue<KeyValuePair<K2, V3>> queue)
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



    }

}
