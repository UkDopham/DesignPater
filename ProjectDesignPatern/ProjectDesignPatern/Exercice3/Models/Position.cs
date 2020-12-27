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
        private PositionType _positionType;
        private Player _owner = null;
        private int _defaultPrice;
        private int _currentLevel = 0;
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
            PositionType positionType,
            int defaultPrice)
        {
            this._name = name;
            this._positionType = positionType;
            this._defaultPrice = defaultPrice;
        }
        public Position(
            string name,
            PositionType positionType)
        {
            this._name = name;
            this._positionType = positionType;
        }
        #region Actions
        public void Action(Player player)
        {
            Console.WriteLine(this._positionType);
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
        private void Upgrade(Player player)
        {
            int price = GetPrice();
            if (price <= player.Money)
            {
                Console.WriteLine($"Do you want to upgrade {this._name}  for {price} $ ? (yes/no)");
                Console.WriteLine($"You have {player.Money} $.");
                string input = Console.ReadLine();
                input = input.ToLower();
                if(input == "yes")
                {
                    player.Money -= price;
                    this._currentLevel++;
                    Console.WriteLine($"You have upgrade {this._name}.");
                }
            }
            else
            {
                Console.WriteLine("You cannot buy.");
            }

        }
        private void Pay(Player player)
        {
            int price = GetPrice();
            Console.WriteLine($"You have enter the parcel of {this._owner}, you have to pay {price}");
            if(price <= player.Money) // can pay
            {
                player.Money -= price;
                this._owner.Money += price;
            }
            else
            {
                player.PlayerState = PlayerState.lost;
                Console.WriteLine("You have lost, you don't have enough money to pay.");
                this._owner.Money += player.Money;
            }
        }
        private void Buy(Player player)
        {
            int price = GetPrice();
            if (price <= player.Money)
            {
                Console.WriteLine($"Do you want to buy {this._name}  for {price} $ ? (yes/no)");
                Console.WriteLine($"You have {player.Money} $.");
                string input = Console.ReadLine();
                input = input.ToLower();
                if (input == "yes")
                {
                    player.Money -= price;
                    this._owner = player;
                    Console.WriteLine($"You have buy {this._name}.");
                }
            }
            else
            {
                Console.WriteLine("You cannot buy.");
            }
        }
        private void BasicAction(Player player)
        {
            if (player == this._owner)
            {
                Upgrade(player);
                //Upgrade
            }
            else
            {
                if (this._owner != null)
                {
                    Pay(player);
                    //Pay
                }
                else
                {
                    Buy(player);
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
        private int GetPrice()
        {
            int value =  (int)(this._defaultPrice * Math.Exp(this._currentLevel));
            //this._currentLevel++;
            return value;
        }
        public override string ToString()
        {
            return $"[ {this._name} - {this._positionType} ]";
        }
    }
}
