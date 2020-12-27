using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDesignPatern.Exercice3.Models
{
    public class Player
    {
        #region Variables
        private string _name;
        private int _currentPosition; // plus opti de stock juste l'index que la position
        private int _money;
        private int _jailCount = 0;
        private PlayerState _playerState;
        #endregion

        #region Proprietes
        public string Name
        {
            get
            {
                return this._name;
            }
        }
        public int Money
        {
            get
            {
                return this._money;
            }
            set
            {
                this._money = value;
            }
        }
        public int CurrentPosition
        {
            get
            {
                return this._currentPosition;
            }
            set
            {
                this._currentPosition = value;
            }
        }
        public int JailCount
        {
            get
            {
                return this._jailCount;
            }
            set
            {
                this._jailCount = value;
            }
        }
        public PlayerState PlayerState
        {
            get
            {
                return this._playerState;
            }
            set
            {
                this._playerState = value;
            }
        }
        #endregion
        public Player(string name)
        {
            this._name = name;
        }

        public override string ToString()
        {
            return $"{this._name} {this._currentPosition}";
        }
    }
}
