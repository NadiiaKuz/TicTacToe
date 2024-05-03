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

        private Cell GetVerticalCellForAttackOrDefense(char checkMark)
        {
            for (int col = 0; col < 3; col++)
            {
                int currentSumVert = 0;
                int freeRow = -1;
                for (int row = 0; row < 3; row++)
                {
                    if (gameField[row][col] == EMPTY_CELL)
                    {
                        freeRow = row;
                    }
                    currentSumVert += gameField[row][col] == checkMark ? 1 : 0;
                }

                if (currentSumVert == 2 && freeRow >= 0)
                {
                    return Cell.From(freeRow, col);
                }
            }
            return Cell.ErrorCell();
        }

        private Cell GetDiagonalCellForAttackOrDefense(char checkMark)
        {
            // diagonal 1:
            // * - -
            // - * -
            // - - *
            // cell coordinates: (0; 0), (1; 1), (2; 2)
            // formula for calculating column by row: <column> = row

            // diagonal 2:
            // - - *
            // - * -
            // * - -
            // cell coordinates: (0; 2), (1; 1), (2, 0)
            // formula for calculating column by row: <column> = 2 - row

            int diagonal1Sum = 0;
            int diagonal2Sum = 0;
            int freeCol1 = -1, freeRow1 = -1;
            int freeCol2 = -1, freeRow2 = -1;

            for (int row = 0; row<3; row++)
            {
                diagonal1Sum += gameField[row][row] == checkMark ? 1 : 0;
                diagonal2Sum += gameField[row][2 - row] == checkMark ? 1 : 0;

                if (gameField[row][row] == EMPTY_CELL)
                {
                    freeCol1 = row;
                    freeRow1 = row;
                }

                if (gameField[row][2 - row] == EMPTY_CELL)
                {
                    freeCol2 = 2 - row;
                    freeRow2 = row;
                }

                if (diagonal1Sum == 2 && freeRow1 >= 0 && freeCol1 >= 0)
                {
                    return Cell.From(freeRow1, freeCol1);
                }
                else if (diagonal2Sum == 2 && freeRow2 >= 0 && freeCol2 >= 0)
                {
                    return Cell.From(freeRow2, freeCol2);
                }
            }

            return Cell.ErrorCell();
        }

        private Cell ComputerTryAttackHorizontalCell()
        {
            return GetHorizontalCellForAttackOrDefense(O_MARK);
        }

        private Cell ComputerTryAttackVerticalCell()
        {
            return GetVerticalCellForAttackOrDefense(O_MARK);
        }

        private Cell ComputerTryAttackDiagonalCell()
        {
            return GetDiagonalCellForAttackOrDefense(O_MARK);
        }

        private Cell ComputerTryDefendHorizontalCell()
        {
            return GetHorizontalCellForAttackOrDefense(X_MARK);
        }

        private Cell ComputerTryDefendVerticalCell()
        {
            return GetVerticalCellForAttackOrDefense(X_MARK);
        }

        private Cell ComputerTryDefendDiagonalCell()
        {
            return GetDiagonalCellForAttackOrDefense(X_MARK);
        }

        private Cell ComputerTryAttackCell()
        {
            // We are trying to attack on horizontal cells
            Cell attackedHorizontalCell = ComputerTryAttackHorizontalCell();
            if (!attackedHorizontalCell.IsErrorCell())
            {
                return attackedHorizontalCell;
            }

            // We are trying to attack on vertical cells
            Cell attackedVerticalCell = ComputerTryAttackVerticalCell();
            if (!attackedVerticalCell.IsErrorCell())
            {
                return attackedVerticalCell;
            }

            // We are trying to attack on diagonal cells
            Cell attackedDiagonalCell = ComputerTryAttackDiagonalCell();
            if (!attackedDiagonalCell.IsErrorCell())
            {
                return attackedDiagonalCell;
            }

            // There are no acceptable cells for attack - we return a special cell with an error sign
            return Cell.ErrorCell();
        }

        private Cell ComputerTryDefendCell()
        {
            // We are trying to defend on horizontal cells
            Cell defendedHorizontalCell = ComputerTryDefendHorizontalCell();
            if (!defendedHorizontalCell.IsErrorCell())
            {
                return defendedHorizontalCell;
            }

            // We are trying to defend on vertical cells
            Cell defendedVerticalCell = ComputerTryDefendVerticalCell();
            if (!defendedVerticalCell.IsErrorCell())
            {
                return defendedVerticalCell;
            }

            // We are trying to defend on diagonal cells
            Cell defendedDiagonalCell = ComputerTryDefendDiagonalCell();
            if (!defendedDiagonalCell.IsErrorCell())
            {
                return defendedDiagonalCell;
            }

            // There are no acceptable cells for defense - we return a special cell with an error sign
            return Cell.ErrorCell();
        }

        private Cell ComputerTrySelectRandomFreeCell()
        {
            Random random = new Random();
            int randomRow, randomCol;
            const int max_attempts = 1000;

            int current_attempt = 0;
            do
            {
                randomRow = random.Next(3);
                randomCol = random.Next(3);
                current_attempt++;
            }
            while (gameField[randomRow][randomCol] != EMPTY_CELL && current_attempt <= max_attempts);

            if (current_attempt > max_attempts)
            {
                // We were unable to select a random free cell in 1000 attempts, so we manually select the nearest cell
                // by simply searching through all the cells of the playing field
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (gameField[row][col] == EMPTY_CELL)
                        {
                            // cell is free, we return it immediately
                            return Cell.From(row, col);
                        }
                    }
                }
            }

            return Cell.From(randomRow, randomCol);
        }

        /// <summary>
        /// Returns true if there is at least one unoccupied cell on the playing field and false otherwise
        /// </summary>
        /// <returns>true if there is at least one free cell on the field, otherwise false</returns>
        public bool IsAnyFreeCell()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (gameField[row][col] == EMPTY_CELL)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Cell MakeComputerTurnAndGetCell()
        {
            // Strategy 1 - the computer tries to attack first if it has only one move left to win
            Cell attackedCell = ComputerTryAttackCell();
            if (!attackedCell.IsErrorCell())
            {
                return attackedCell;
            }

            // Strategy 2 - if there are no acceptable squares to attack,
            // the computer will try to find squares to defend to prevent the human from winning
            Cell defendedCell = ComputerTryDefendCell();
            if (!defendedCell.IsErrorCell())
            {
                return defendedCell;
            }

            // Strategy 3 - the computer does not have acceptable cells for attack and defense,
            // so it needs to choose an arbitrary free cell for its next move
            if (IsAnyFreeCell())
            {
                Cell randomFreeCell = ComputerTrySelectRandomFreeCell();
                return randomFreeCell;
            }

            return Cell.ErrorCell();
        }
    }
}
