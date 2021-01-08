using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectDesignPatern.Exercice1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDesignPatern.Exercice1.Models.Tests
{
    [TestClass()]
    public class CustomQueueTests
    {
        [TestMethod()]
        public void EnqueueTest()
        {
            CustomQueue<int> customQueue = new CustomQueue<int>();
            customQueue.Enqueue(11);
            customQueue.Enqueue(11);
            customQueue.Enqueue(11);
            Assert.AreEqual(3, customQueue.Count);
        }

        [TestMethod()]
        public void DequeueTest()
        {
            CustomQueue<int> customQueue = new CustomQueue<int>();
            customQueue.Enqueue(11);
            customQueue.Enqueue(11);
            customQueue.Enqueue(11);
            customQueue.Dequeue();
            Assert.AreEqual(2, customQueue.Count);
        }

    }
}