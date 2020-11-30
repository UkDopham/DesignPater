using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDesignPatern.Exercice1.Models
{
    public class CustomQueueNode<T>
    {
        #region Variables
        private T _value;
        private CustomQueueNode<T> _nextNode;
        #endregion
        #region Proprietes
        public T Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
        public CustomQueueNode<T> NextNode
        {
            get
            {
                return this._nextNode;
            }
            set
            {
                this._nextNode = value;
            }
        }
        #endregion

        public CustomQueueNode(T value)
        {
            this._value = value;
        }
        public override string ToString()
        {
            return $"{this._value }";
        }
    }
}
