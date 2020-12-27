using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDesignPatern.Exercice3.Models
{
    public static class DiceHelper // Singleton Design Pattern
    {
        public static bool ThrowDices(Player player, out int dicesValue, int number = 2)
        {
            Random random = new Random();
            List<int> dices = new List<int>();
            int value = 0;
            for (int i = 0;
                i < number;
                i++)
            {
                value = random.Next(1, 6 + 1);
                dices.Add(value);
                Console.WriteLine($"dice : {value}");
            }
            dicesValue = DicesValue(dices);
            return IsSame(dices);

        }
        private static int DicesValue(List<int> dices)
        {
            int value = 0;
            foreach (int dice in dices)
            {
                value += dice;
            }
            return value;
        }
        public static bool IsSame(List<int> dices)
        {
            int value = dices[0];
            foreach (int dice in dices)
            {
                if (dice != value)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
