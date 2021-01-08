using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectDesignPatern.Exercice3.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDesignPatern.Exercice3.Models.Tests
{
    [TestClass()]
    public class GameTests
    {

        [TestMethod()]
        public void Counttest()
        {
            CircularList<Position> board = new CircularList<Position>();
            board.Add(new Position("Chance", PositionType.luck)); //0
            board.Add(new Position("Mediterranean Avenue", PositionType.basic, 60)); //1
            Assert.AreEqual(1, board.Count); // count = index of last here
        }
        [TestMethod()]
        public void Lasttest()
        {
            CircularList<Position> board = new CircularList<Position>();
            Position position = new Position("test", PositionType.basic);
            board.Add(position);
            Assert.AreEqual(position.ToString(), board.GetLast().ToString()); 
        }
    }
}