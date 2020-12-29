using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ProjectDesignPatern.Exercice3.Models
{
    public class CircularNode<T> 
    {
        #region Variables
        private CircularNode<T> _next;
        private T _value;
        #endregion

        #region Proprietes
        public CircularNode<T> Next
        {
            get
            {
                return this._next;
            }
            set
            {
                this._next = value;
            }
        }
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
        #endregion

        public CircularNode(T value)
        {
            this._value = value;
        }

        public override string ToString()
        {
            return this._value.ToString();
        }

    }
}
