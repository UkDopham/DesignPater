
using ProjectDesignPatern.Exercice1.Models;
using ProjectDesignPatern.Exercice3.Models;
using System;
using System.Collections.Generic;

namespace ProjectDesignPatern
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Please select the exercice (1/2/3)");
                string input = Console.ReadLine();
                switch(input)
                {
                    case "1":
                        Exercice1();
                        break;

                    case "2":
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
