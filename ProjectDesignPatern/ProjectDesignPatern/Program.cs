
using ProjectDesignPatern.Exercice1.Models;
using ProjectDesignPatern.Exercice2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ProjectDesignPatern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Exercice2();
        }
        private static void Exercice1()
        {
            CustomQueue<int> customQueue = new CustomQueue<int>();
            customQueue.Enqueue(1);
            Console.WriteLine(customQueue);
            customQueue.Enqueue(2);
            customQueue.Enqueue(5);
            customQueue.Enqueue(6);
            customQueue.Enqueue(8);
            customQueue.Enqueue(3);
            Console.WriteLine(customQueue);
            Console.WriteLine(customQueue.Dequeue());
            Console.WriteLine(customQueue);
        }
        private static void Exercice2()
        {
            Console.WriteLine("Exercice 2");


            //int tailleInput = 6;
            //int n = 0;
            //List<int> inputData0 = new List<int>();
            //for (int i = 0; i < tailleInput; i++)
            //{
            //    inputData0.Add(n);
            //    n++;
            //    if (n >= 5) n = 0;
            //}

            //int nb = 3;
            //List<List<int>> groups = SplitInputSimple2(inputData0, inputData0.Count/nb);

            //Console.WriteLine("Raw data");
            //foreach (int item in inputData0)
            //{
            //    Console.Write(item + " ");
            //}

            //Console.WriteLine($"\nSplitted data ({nb})");
            //foreach (List<int> item in groups)
            //{
            //    foreach (int i in item)
            //    {
            //        Console.Write($"{i} ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.ReadKey();

            Console.WriteLine("Test MapReduce : Word Counting");


            List<KeyValuePair<string, string>> inputData = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("1", "Test working data the data the Test is"),
                new KeyValuePair<string, string>("2", "Test the is the working ! the"),
                new KeyValuePair<string, string>("3", "is data Test the Test data") };

            List<KeyValuePair<string, string>> inputData2 = new List<KeyValuePair<string, string>>();
            Console.WriteLine("\nInput data :");
            foreach (var item in inputData)
            {
                Console.Write($"{item.Key}  |  {item.Value}  ");
            }
            Console.WriteLine();

            using (var sr = new StreamReader(@"textWikipedia.txt"))
            {
                // Read the stream as a string, and write the string to the console.
                int cpt2 = 0;
                while (!sr.EndOfStream)
                {
                    inputData2.Add(new KeyValuePair<string, string>($"{cpt2}", sr.ReadLine().ToString()));
                    cpt2++;
                }

            }

            if (false)
                inputData2.ForEach(x => Console.WriteLine(x));



            MapReduce<string, string, string, int, int> letters = new MapReduce<string, string, string, int, int>(
                8,
                MapReduceMethods.MapFromMem,
                MapReduceMethods.Reduce
                );
            List<KeyValuePair<string, int>> result = letters.NodeExecute(inputData2).ToList();
            result.Sort(
                (a, b) => (a.Value.CompareTo(b.Value) * -1)
                );

            Console.WriteLine("\nResult :");
            int cpt = 0;
            foreach (KeyValuePair<string, int> item in result)
            {
                Console.WriteLine("{0}  ({1})", item.Key, item.Value);
                if (cpt > 10) break;
                cpt++;
            }

        }


        public static List<List<int>> SplitInputSimple2(List<int> inputData, int n)
        {
            return inputData
     .Select((x, i) => new { Index = i, Value = x })
     .GroupBy(x => x.Index / n)
     .Select(x => x.Select(v => v.Value).ToList())
     .ToList();
        }
    }
}
