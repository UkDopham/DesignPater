using System;
using System.Collections.Generic;
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

        public Game(List<Player> players, bool isPrintingDebug = false)
        {
            this._players = players;
            this._isPrintingDebug = isPrintingDebug;
            StartBoard();
            InitilizationPlayer();
            GameHelper.Game = this;
        }
        private void StartBoard()
        {
            if (this._isPrintingDebug)
            {
                Console.WriteLine("Adding position into the board");
            }
            this._board = new CircularList<Position>();
            this._board.Add(new Position("test0", "desc0", PositionType.basic)); //0
            this._board.Add(new Position("test1", "desc1", PositionType.basic)); //1
            this._board.Add(new Position("test2", "desc2", PositionType.basic)); //2
            this._board.Add(new Position("test3", "desc3", PositionType.basic)); //3
            this._board.Add(new Position("test4", "desc4", PositionType.basic)); //4
            this._board.Add(new Position("test5", "desc5", PositionType.basic)); //5
            this._board.Add(new Position("test6", "desc6", PositionType.basic)); //6
            this._board.Add(new Position("test7", "desc7", PositionType.basic)); //7
            this._board.Add(new Position("test8", "desc8", PositionType.basic)); //8
            this._board.Add(new Position("test9", "desc9", PositionType.basic)); //9
            this._board.Add(new Position("test10", "desc10", PositionType.jail)); //10
        }
        private void InitilizationPlayer()
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
            Console.WriteLine($"it's the turn of {player}");
            if (player.PlayerState == PlayerState.alive)
            {
                BasicTurn(player);
            }
            this._board.GetNodeByIndex(player.CurrentPosition).Value.Action(player);
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
            return false; //TODO
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
        #region Dices
        
        #endregion
    }
}
