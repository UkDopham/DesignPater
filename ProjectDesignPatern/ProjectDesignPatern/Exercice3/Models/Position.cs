using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ProjectDesignPatern.Exercice3.Models
{
    public class Position // State Pattern
    {
        #region Variables
        private string _name;
        private string _description;
        private PositionType _positionType;
        private Player _owner = null;
        private int _defaultPrice;
        #endregion

        #region Proprietes
        private int DefaultPrice
        {
            get
            {
                return this._defaultPrice;
            }
            set
            {
                this._defaultPrice = value;
            }
        }
        #endregion

        public Position(
            string name, 
            string description, 
            PositionType positionType)
        {
            this._name = name;
            this._description = description;
            this._positionType = positionType;            
        }
        #region Actions
        public void Action(Player player)
        {
            switch(this._positionType)
            {
                case PositionType.basic:
                    BasicAction(player);
                    break;

                case PositionType.luck:
                    LuckAction(player);
                    break;

                case PositionType.jail:
                    JailAction(player);
                    break;

                case PositionType.gotojail:
                    GoToJailAction(player);
                    break;
            }
            
        }
        private void BasicAction(Player player)
        {
            if (player == this._owner)
            {
                //Upgrade
            }
            else
            {
                if (this._owner != null)
                {
                    //Pay
                }
                else
                {
                    //Buy
                }
            }
        }
        private void JailAction(Player player)
        {
            if (player.PlayerState == PlayerState.jailed)
            {
                int dicesValue = 0;
                bool isSame = DiceHelper.ThrowDices(player, out dicesValue);
                player.JailCount++;
                if (isSame ||
                    player.JailCount == 3)
                {
                    player.PlayerState = PlayerState.alive;
                    player.JailCount = 0;
                    Console.WriteLine($"{player} is free !");
                    GameHelper.Game.MovePlayer(player, dicesValue);
                }
            }
            else
            {
                Console.WriteLine($"{player} is visiting the prison");
            }
        }
        private void LuckAction(Player player)
        {

        }
        private void GoToJailAction(Player player)
        {
            player.CurrentPosition = GameHelper.JailIndex;
            player.PlayerState = PlayerState.jailed;
        }
        #endregion
        public override string ToString()
        {
            return $"[ {this._name} - {this._description} - {this._positionType} ]";
        }
    }
}
