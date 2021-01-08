using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectDesignPatern.Exercice3.Models
{
    public class Game // Observer Design Pattern
    {
        #region Variables
        private List<Player> _players = new List<Player>();
        private CircularList<Position> _board;
        private int _currentTurn = 0;
        private bool _isPrintingDebug;
        #endregion

        public Game(List<Player> players, int startingMoney, bool isPrintingDebug = false)
        {
            this._players = players;
            this._isPrintingDebug = isPrintingDebug;
            StartBoard();
            InitilizationPlayer(startingMoney);
            GameHelper.Game = this;
        }
        private void StartBoard()
        {
            if (this._isPrintingDebug)
            {
                Console.WriteLine("Adding position into the board");
            }
            this._board = new CircularList<Position>();
            this._board.Add(new Position("Chance", PositionType.luck)); //0
            this._board.Add(new Position("Mediterranean Avenue", PositionType.basic, 60)); //1
            this._board.Add(new Position("Baltic Avenue", PositionType.basic, 60)); //2
            this._board.Add(new Position("Chance", PositionType.luck)); //3
            this._board.Add(new Position("Oriental Avenue", PositionType.basic, 100)); //4
            this._board.Add(new Position("Vermont Avenue", PositionType.basic, 100)); //5
            this._board.Add(new Position("Chance", PositionType.luck)); //6
            this._board.Add(new Position("Connecticut Avenue", PositionType.basic, 120)); //7
            this._board.Add(new Position("St. Charles Place", PositionType.basic, 140)); //8
            this._board.Add(new Position("Jail", PositionType.jail)); //9
            this._board.Add(new Position("States Avenue", PositionType.basic, 140)); //10
            this._board.Add(new Position("Virginia Avenue", PositionType.basic, 160)); //11
            this._board.Add(new Position("Chance", PositionType.luck)); //12
            this._board.Add(new Position("St. James Place", PositionType.basic, 180)); //13
            this._board.Add(new Position("Tennessee Avenue", PositionType.basic, 180)); //14
            this._board.Add(new Position("Chance", PositionType.luck)); //15
            this._board.Add(new Position("New York Avenue", PositionType.basic, 200)); //16
            this._board.Add(new Position("Chance", PositionType.luck)); //17
            this._board.Add(new Position("Kentucky Avenue", PositionType.basic, 220)); //18
            this._board.Add(new Position("Chance", PositionType.luck)); //19
            this._board.Add(new Position("Indiana Avenue", PositionType.basic, 220)); //20
            this._board.Add(new Position("Illinois Avenue", PositionType.basic, 240)); //21
            this._board.Add(new Position("Chance", PositionType.luck)); //22
            this._board.Add(new Position("Atlantic Avenue", PositionType.basic, 260)); //23
            this._board.Add(new Position("Ventonor Avenue", PositionType.basic, 260)); //14
            this._board.Add(new Position("Chance", PositionType.luck)); //25
            this._board.Add(new Position("Marvin Gardens", PositionType.basic, 280)); //26
            this._board.Add(new Position("Chance", PositionType.luck)); //27
            this._board.Add(new Position("Pacific Avenue", PositionType.basic, 300)); //28
            this._board.Add(new Position("Go To Jail", PositionType.gotojail)); //29
            this._board.Add(new Position("North Carolina Avenue", PositionType.basic, 300)); //30
            this._board.Add(new Position("Chance", PositionType.luck)); //31
            this._board.Add(new Position("Pennsylvania Avenue", PositionType.basic, 320)); //32
            this._board.Add(new Position("Chance", PositionType.luck)); //33
            this._board.Add(new Position("Park Place", PositionType.basic, 350)); //34
            this._board.Add(new Position("Boardwalk", PositionType.basic, 400)); //35
            this._board.Add(new Position("Chance", PositionType.luck)); //36
            this._board.Add(new Position("St. Carly", PositionType.basic, 440)); //37
            this._board.Add(new Position("Loop Avenue", PositionType.basic, 440)); //38
            this._board.Add(new Position("South Paris Avenue", PositionType.basic, 460)); //39
            Console.WriteLine(this._board.Count); //count = index of last position
        }
        private void InitilizationPlayer(int startingMoney)
        {
            if (this._isPrintingDebug)
            {
                Console.WriteLine("Settting the player starting position");
            }
            Random random = new Random();
            int value = 0;
            foreach(Player player in this._players)
            {
                value = random.Next(0, this._board.Count);
                player.CurrentPosition = value;
                player.Money = startingMoney;
                if (this._isPrintingDebug)
                {
                    Console.WriteLine($"{player}");
                }
            }
        }

        public void Start()
        {
            while(!IsFinished())
            {
                Turn();
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }

        #region Game mecaniques
        private void Turn()
        {
            Player player = this._players[this._currentTurn % this._players.Count];
            if (player.PlayerState != PlayerState.lost)
            {
                Console.WriteLine($"it's the turn of {player}");
                if (player.PlayerState == PlayerState.alive)
                {
                    BasicTurn(player);
                }
                CircularNode<Position> node = this._board.GetNodeByIndex(player.CurrentPosition);
                if (node != null)
                {
                    node.Value.Action(player);
                }
                else
                {
                    Console.WriteLine("ERROR");
                }
            }
                this._currentTurn++;
        }
        private void BasicTurn(Player player)
        {
            int count = 0;
            int dicesValue = 0;
            bool canThrowAgain = true;
            while (canThrowAgain &&
                count < 3
                && player.CurrentPosition != GameHelper.GoToJailIndex)
            {
                canThrowAgain = DiceHelper.ThrowDices(player, out dicesValue);
                MovePlayer(player, dicesValue);
                count++;
            }
            if (count >= 3)
            {
                player.CurrentPosition = GameHelper.GoToJailIndex;//go jail
                Console.WriteLine("jail");
            }
        }
        public bool IsFinished()
        {
            int playersLeft = this._players.Where(x => x.PlayerState != PlayerState.lost).Count();
            return playersLeft == 1; //TODO
        }
        #endregion
        #region Positions
        public void MovePlayer(Player player, int value)
        {
            player.CurrentPosition = 
                (player.CurrentPosition + value) >= this._board.Count ?
                ((player.CurrentPosition + value) % this._board.Count) :
                player.CurrentPosition + value;
            if (this._isPrintingDebug)
            {
                Console.WriteLine($"value {value}");
                Console.WriteLine($"index {player.CurrentPosition}");
            }
        }
        #endregion
    }
}
