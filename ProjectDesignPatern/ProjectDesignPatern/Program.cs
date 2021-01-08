
using ProjectDesignPatern.Exercice1.Models;
using ProjectDesignPatern.Exercice2;
using ProjectDesignPatern.Exercice3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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


            // initialisation des input
            List<KeyValuePair<string, string>> inputData = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("1", "2 4 5 6 3 5 6"),
                new KeyValuePair<string, string>("2", "1 3 6 4 6 5 5"),
                new KeyValuePair<string, string>("3", "6 4 2 3 4 6 5") };


            List<KeyValuePair<string, string>> inputData2 = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < 10; i++)// on met dix fois le texte dans l'inputData (  + 10 000 000 lignes )
            {
                using (var sr = new StreamReader(@"textMoliere.txt")) // texte des Fourberies de scapin
                {
                    // Read the stream as a string, and write the string to the console.
                    int cpt2 = 0;
                    while (!sr.EndOfStream)
                    {
                        inputData2.Add(new KeyValuePair<string, string>($"{cpt2}", sr.ReadLine().ToString()));
                        cpt2++;
                    }
                }
            }

            // initialisation du MapReduce
            MapReduce<string, string, string, int, int> letters = new MapReduce<string, string, string, int, int>(
                numOfThreads,
                MapReduceMethods.Map_Words, // on peut aussi utiliser "MapReduceMethods.Map_Lettres" (pour compter les lettres)
                MapReduceMethods.Reduce
                );

            // lancement des calculs
            List<KeyValuePair<string, int>> result = letters.Execute(inputData2).ToList();

            result.Sort((a, b) => (a.Value.CompareTo(b.Value) * -1));

            // affichage des resultats
            int max = 10;
            int cpt = 0;
            Console.WriteLine($"\nResultats tries : ({max} premiers resultats affiches)");
            foreach (KeyValuePair<string, int> item in result)
            {
                Console.WriteLine("{0}  ({1})", item.Key, item.Value);
                if (cpt > max) break;
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
