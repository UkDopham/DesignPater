
using ProjectDesignPatern.Exercice1.Models;
using ProjectDesignPatern.Exercice2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ProjectDesignPatern.Exercice3.Models;
using System;
using System.Collections.Generic;

namespace ProjectDesignPatern
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please select the exercice (1/2/3)");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Exercice1();
                        break;

                    case "2":
                        Exercice2();
                        break;

                    case "3":
                        Exercice3();
                        break;

                    default:
                        Console.WriteLine("wrong input");
                        break;
                }
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
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

            Console.WriteLine("Test MapReduce : Word Counting");
            int numOfThreads = 4;

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

            using (var sr = new StreamReader(@"textMoliere.txt"))
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
                numOfThreads,
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


        private static void Exercice3()
        {
            List<Player> players = new List<Player>()
            {
                new Player("player0"),
                new Player("player1"),
            };
            Game game = new Game(players, 10000, true);
            game.Start();
        }
    }
    
}
