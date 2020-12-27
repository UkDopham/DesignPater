using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDesignPatern.Exercice3.Models
{
    public class CircularList<T> // Iterator pattern
    {
        #region Variables
        private CircularNode<T> _currentNode;
        private CircularNode<T> _root;
        #endregion

        #region Proprietes
        public CircularNode<T> Root
        {
            get
            {
                return this._root;
            }
        }
        public int Count
        {
            get
            {
                return GetCount();
            }
        }
        #endregion
        public CircularList()
        {
            this._currentNode = null;
            this._root = null;
        }

        public CircularNode<T> Next()
        {
            if (this._currentNode != null)
            {
                if (this._currentNode.Next != null)
                {
                    this._currentNode = this._currentNode.Next;
                }
            }
            return this._currentNode;
        }
        private int GetCount()
        {
            int count = 0;
            CircularNode<T> current = this._root;
            while (current.Next != null && current.Next != this._root)
            {
                current = current.Next;
                count++;
            }
            return count;
        }
        public CircularNode<T> GetLast()
        {
            CircularNode<T> last = this._root;
            while (last.Next != null && last.Next != this._root)
            {
                last = last.Next;
            }
            return last;
        }
        public CircularNode<T> GetNodeByIndex(int index)
        {
            int currentIndex = 0;
            CircularNode<T> current = this._root;
            CircularNode<T> node = null;
            while (current.Next != null && current.Next != this._root)
            {
                if(currentIndex == index)
                {
                    node = current;
                    break;
                }
                current = current.Next;
                currentIndex++;                
            }
            return node;
        }
        public void Add(T value)
        {
            CircularNode<T> node = new CircularNode<T>(value);
            if (this._root != null)
            {
                CircularNode<T> last = GetLast();
                last.Next = node;
                node.Next = this._root;
            }
            else
            {
                this._root = node;
            }
        }

        public void Remove(T value)
        {

        }
    }
}
