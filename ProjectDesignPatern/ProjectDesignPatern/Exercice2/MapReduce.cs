using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProjectDesignPatern.Exercice2
{
    class MapReduce<K1, V1, K2, V2, V3>
    {
        private bool debug { get; }

        public delegate IEnumerable<KeyValuePair<K2, V2>> MapFunction(K1 key, V1 value);
        public delegate IEnumerable<V3> ReduceFunction(K2 key, IEnumerable<V2> values);

        private MapFunction mapF;
        private ReduceFunction reduceF;

        private Thread[] threads { get; set; }

        public MapReduce(int numberOfNodes, MapFunction mapFunction, ReduceFunction reduceFunction, bool debug = false)
        {
            this.threads = new Thread[numberOfNodes];

            this.mapF = mapFunction;
            this.reduceF = reduceFunction;

            this.debug = debug;
        }
        public IEnumerable<KeyValuePair<K2, V2>> SplitingMapping(IEnumerable<KeyValuePair<K1, V1>> input)
        {
            List<IEnumerable<KeyValuePair<K1, V1>>> inputSplitted =
               DataSplitter<K1, V1>.Splitting(input, this.threads.Length).ToList();

            if (this.threads.Length < inputSplitted.Count)
                throw new Exception("Data not splited right!");

            // Creation queue des resultats
            ConcurrentQueue<KeyValuePair<K2, V2>> mapResult = new ConcurrentQueue<KeyValuePair<K2, V2>>();

            // Initialisation des threads
            ConcurrentQueue<int> temp = new ConcurrentQueue<int>();
            for (int i = 0; i < inputSplitted.Count; i++)
            {
                temp.Enqueue(i);
                threads[i] = new Thread(() => { int test; while (!temp.TryDequeue(out test)) ; this.Map(inputSplitted[test], mapResult); });
            }

            // Depart threads
            for (int i = 0; i < inputSplitted.Count; i++)
                threads[i].Start();

            // Attente fin threads
            for (int i = 0; i < inputSplitted.Count; i++)
                threads[i].Join();


            if (this.debug)
            {
                Console.WriteLine($"\n\nNodeMap :\nSize = {input.Count()}   Nb of threads = {this.threads.Length}    DataSplited = {inputSplitted.Count()}");
                inputSplitted.ForEach(x => Console.Write(x.Count() + " "));
                Console.WriteLine($"\nSize result = {mapResult.Count}");
            }

            return mapResult;
        }
        public IEnumerable<KeyValuePair<K2, V3>> ShufflingReducing(IEnumerable<KeyValuePair<K2, V2>> input)
        {
            IEnumerable<IGrouping<K2, V2>> groups = this.Shuffling(input);

            List<IEnumerable<IGrouping<K2, V2>>> inputSplitted =
                    DataSplitter<K2, V2>.Splitting(groups, this.threads.Length).ToList();
            if (this.threads.Length < inputSplitted.Count)
                throw new Exception("Data not splitted right!");

            // Creation queue des resultats
            ConcurrentQueue<KeyValuePair<K2, V3>> reduceResult = new ConcurrentQueue<KeyValuePair<K2, V3>>();


            // Initialisation des threads
            ConcurrentQueue<int> temp3 = new ConcurrentQueue<int>();
            for (int i = 0; i < inputSplitted.Count; i++)
            {
                temp3.Enqueue(i);
                threads[i] = new Thread(() => { int test; while (!temp3.TryDequeue(out test)) ; this.Reduce(inputSplitted[test], reduceResult); });
            }

            // Depart threads
            for (int i = 0; i < inputSplitted.Count; i++)
                threads[i].Start();

            // Attente fin threads
            for (int i = 0; i < inputSplitted.Count; i++)
                threads[i].Join();

            if (this.debug)
            {
                Console.WriteLine($"\nNodeReduce :\nSize = {groups.Count()}   Nb of threads = {this.threads.Length}    DataSplited = {inputSplitted.Count()}");
                inputSplitted.ForEach(x => Console.Write(x.Count() + " "));
                Console.WriteLine($"\nSize result = {reduceResult.Count}");
            }

            return reduceResult;


        }


        public IEnumerable<KeyValuePair<K2, V3>> Execute(IEnumerable<KeyValuePair<K1, V1>> input)
        {
            return ShufflingReducing(SplitingMapping(input));
        }


        private void Map(IEnumerable<KeyValuePair<K1, V1>> input, ConcurrentQueue<KeyValuePair<K2, V2>> queue)
        {
            var q = from pair in input
                    from mapped in mapF(pair.Key, pair.Value)
                    select mapped;

            foreach (var item in q)
                queue.Enqueue(item);
        }
        private IEnumerable<IGrouping<K2, V2>> Shuffling(IEnumerable<KeyValuePair<K2, V2>> intermediateValues)
        {
            var groups = from pair in intermediateValues
                         group pair.Value by pair.Key into g
                         select g;

            return groups;
        }
        private void Reduce(IEnumerable<IGrouping<K2, V2>> groups, ConcurrentQueue<KeyValuePair<K2, V3>> queue)
        {

            var reduced = from g in groups
                          let k2 = g.Key
                          from reducedValue in reduceF(k2, g)
                          select new KeyValuePair<K2, V3>(k2, reducedValue);

            foreach (var item in reduced)
                queue.Enqueue(item);

        }



    }

}
