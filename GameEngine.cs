using System;
using System.Drawing;

namespace TicTacToe
{
    internal class GameEngine
    {
        internal enum GameMode
        {
            None,
            PlayerVsPlayer,
            PlayerVsCPU
        }

        internal enum WhooseTurn
        {
            Player1Human,
            Player2Human,
            Player2CPU
        }

        private GameMode Mode {  get; set; } = GameMode.None;
        private WhooseTurn Turn { get; set; } = WhooseTurn.Player1Human;
        private string Winner { get; set; } = "";
        
        private int player1Score = 0;
        private int player2Score = 0;
        private int numberOfDraws = 0;

        const char EMPTY_CELL = '-';
        const char X_MARK = 'X';
        const char O_MARK = 'O';

        public const string PLAYER_HUMAN_TITLE = "Player";
        public const string PLAYER_CPU_TITLE = "Computer";

        private char[][] gameField = new char[][] {
            new char[] { EMPTY_CELL, EMPTY_CELL, EMPTY_CELL },
            new char[] { EMPTY_CELL, EMPTY_CELL, EMPTY_CELL },
            new char[] { EMPTY_CELL, EMPTY_CELL, EMPTY_CELL }
        };

        public GameMode GetCurrentMode()
        {
            return Mode;
        }

        public bool IsGameStarted()
        {
            return Mode != GameMode.None;
        }

        public WhooseTurn GetCurrentTurn()
        {
            return Turn;
        }

        public string GetWinner()
        {
            return Winner;
        }

        public bool IsPlayer1HumanTurn()
        {
            return Turn == WhooseTurn.Player1Human;
        }

        public void SetPlayer1HumanTurn()
        {
            Turn = WhooseTurn.Player1Human;
        }

        public void ResetScore()
        {
            player1Score = 0;
            player2Score = 0;
            numberOfDraws = 0;
        }

        public void PrepareForNewGame()
        {
            Mode = GameMode.None;
            ResetScore();
        }

        public void StartGame(GameMode gameMode)
        {
            if (gameMode == GameMode.None)
            {
                return;
            }

            ResetScore();

            Mode = gameMode;
            Turn = WhooseTurn.Player1Human;
        }

        public string GetCurrentPlayer1Title()
        {
            switch (Mode)
            {
                case GameMode.PlayerVsCPU:
                    return PLAYER_HUMAN_TITLE;
                case GameMode.PlayerVsPlayer:
                    return PLAYER_HUMAN_TITLE + " 1";
            }
            return "";
        }

        public string GetCurrentPlayer2Title()
        {
            switch (Mode)
            {
                case GameMode.PlayerVsCPU:
                    return PLAYER_CPU_TITLE;
                case GameMode.PlayerVsPlayer:
                    return PLAYER_HUMAN_TITLE + " 2";
            }
            return "";
        }

        public string GetCurrentMarkLabelText()
        {
            if (IsPlayer1HumanTurn())
                return X_MARK.ToString();
            else
                return O_MARK.ToString();
        }

        public Color GetCurrentMarkLabelColor()
        {
            if (IsPlayer1HumanTurn())
                return Color.Gold;
            else
                return Color.Fuchsia;
        }

        public int GetPlayer1Score()
        {
            return player1Score;
        }

        public int GetPlayer2Score()
        {
            return player2Score;
        }

        /// <summary>
        /// Returns a string with the name of the player whose turn it is currently
        /// </summary>
        /// <returns>string with player name</returns>
        public string GetWhooseTurnTitle()
        {
            switch (Mode)
            {
                case GameMode.PlayerVsCPU:
                    return Turn == WhooseTurn.Player1Human ? PLAYER_HUMAN_TITLE : PLAYER_CPU_TITLE;
                case GameMode.PlayerVsPlayer:
                    return Turn == WhooseTurn.Player1Human ? PLAYER_HUMAN_TITLE + " 1" : PLAYER_HUMAN_TITLE + " 2";
            }
            return "";
        }

        /// <summary>
        /// Returns a string with the name of the player for whom the next move will be
        /// </summary>
        /// <returns>string with player name</returns>
        public string GetWhooseNextTurnTitle()
        {
            switch (Mode)
            {
                case GameMode.PlayerVsCPU:
                    return Turn == WhooseTurn.Player1Human ? PLAYER_CPU_TITLE : PLAYER_HUMAN_TITLE;
                case GameMode.PlayerVsPlayer:
                    return Turn == WhooseTurn.Player1Human ? PLAYER_HUMAN_TITLE + " 2" : PLAYER_HUMAN_TITLE + " 1";
            }
            return "";
        }

        /// <summary>
        /// Clears the playing field by filling each cell with a sign
        /// of an empty cell (the default is the '-' symbol)
        /// </summary>
        
        public void ClearGameField()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    gameField[row][col] = EMPTY_CELL;
                }
            }
        }

        public void MakeTurnAndFillGameFieldCell(int row, int column)
        {
            if (IsPlayer1HumanTurn())
            {
                gameField[row][column] = X_MARK;
                if (Mode == GameMode.PlayerVsCPU)
                {
                    Turn = WhooseTurn.Player2CPU;
                }
                else if (Mode == GameMode.PlayerVsPlayer)
                {
                    Turn = WhooseTurn.Player2Human;
                }
            }
            else
            {
                gameField[row][column] = O_MARK;
                Turn = WhooseTurn.Player1Human;
            }
        }

        private Cell GetHorizontalCellForAttackOrDefense(char checkMark)
        {
            for (int row = 0; row < 3; row++)
            {
                int currentSumHorizontal = 0;
                int freeCol = -1;
                for (int col = 0; col < 3; col++)
                {
                    if (gameField[row][col] == EMPTY_CELL)
                    {
                        freeCol = col;
                    }
                    currentSumHorizontal += gameField[row][col] == checkMark ? 1 : 0;
                }

                if (currentSumHorizontal == 2 && freeCol >= 0)
                {
                    return Cell.From(row, freeCol);
                }
            }
            return Cell.ErrorCell();
        }
    }
}
