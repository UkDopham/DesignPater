using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDesignPatern.Exercice3.Models
{
    public static class GameHelper // Singleton Design Pattern
    {
        public static int JailIndex = 10;
        public static int GoToJailIndex = 30;
        public static Game Game;
    }
}
