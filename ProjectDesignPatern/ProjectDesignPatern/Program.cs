
using ProjectDesignPatern.Exercice1.Models;
using System;

namespace ProjectDesignPatern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Exercice1();
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
    }
}
