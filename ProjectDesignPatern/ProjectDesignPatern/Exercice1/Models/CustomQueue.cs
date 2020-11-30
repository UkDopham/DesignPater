
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ProjectDesignPatern.Exercice1.Models
{
    public class CustomQueue<T> : IEnumerator, IEnumerable
    {
        #region Variables
        private CustomQueueNode<T> _root;
        private CustomQueueNode<T> _current;
        private int _count;
        #endregion
        public CustomQueue()
        {
            this._root = null;
            this._count = -1;
        }

        #region Proprietes
        public int Count
        {
            get
            {
                return this._count;
            }
        }
        #endregion
        #region Interfaces
        public object Current
        {
            get
            {
                return this._current.Value;
            }
        }
        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }
        public bool MoveNext()
        {
            if (this._current == null)
            {
                this._current = this._root;
            }
            else
            {
                this._current = this._current.NextNode;
            }
            return this._current != null;
        }
        public void Reset()
        {
            this._current = null;
        }
        #endregion
        #region Operations
        public void Enqueue(T value)
        {
            if(this._root == null)
            {
                this._root = new CustomQueueNode<T>(value);
            }
            else
            {
                GetLast(out this._count).NextNode = new CustomQueueNode<T>(value);
            }
            this._count++;
        }
        public T Dequeue()
        {
            T value = default(T);

            if (this._root == null)
            {
                return value;
            }
            else
            {
                value = this._root.Value;
                this._root = this._root.NextNode;
                this._count--;
                return value;
            }
        }
        private CustomQueueNode<T> GetLast(out int count)
        {
            count = 1;
            CustomQueueNode<T> node = this._root;
            while(node.NextNode != null)
            {
                node = node.NextNode;
                count++;
            }
            return node;
        }
        #endregion
        public override string ToString()
        {
            string value = string.Empty;

            foreach(T valueT in this)
            {
                value += $"{valueT}; ";
            }
            
            return value;
        }
    }
}
