using Common;

namespace Day04
{
    internal class Program
    {
        static void Main()
        {
            // read input
            var fileLineCount = File.ReadLines(Constants.FILE_NAME).Count();
            int firstBingoBoardLineIndex = 1;
            int bingoBoardDimension = 5;
            int[] drawnNumbers = null;
            BingoBoard[] bingoBoards = new BingoBoard[(fileLineCount - firstBingoBoardLineIndex) / (bingoBoardDimension + 1)];

            for (int i = 0; i < bingoBoards.Length; i++)
            {
                bingoBoards[i] = new BingoBoard(bingoBoardDimension);
            }

            int lineIndex = 0;
            foreach (string line in File.ReadLines(Constants.FILE_NAME))
            {
                // first line contains drawn numbers
                if (lineIndex == 0)
                {
                    drawnNumbers = line
                        .Split(',')
                        .Select(x => int.Parse(x))
                        .ToArray();
                }
                else
                {
                    // there are empty lines between bingo boards
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    // determine board and row
                    int boardIndex = (lineIndex - firstBingoBoardLineIndex) / bingoBoardDimension;

                    int[] rowNumbers = line
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.Parse(x))
                        .ToArray();

                    bingoBoards[boardIndex].AddRow(rowNumbers);
                }

                lineIndex++;
            }

            // simulate games
            Console.WriteLine($"Result of part one: {SimulateGame(drawnNumbers, bingoBoards)}");
            foreach (BingoBoard bingoBoard in bingoBoards)
            {
                bingoBoard.ResetBoard();
            }
            Console.WriteLine($"Result of part two: {SimulateGameLastWinningBoard(drawnNumbers, bingoBoards)}");
        }

        static int SimulateGame(int[] drawnNumbers, BingoBoard[] bingoBoards)
        {
            if (drawnNumbers == null)
            {
                throw new ArgumentNullException(nameof(drawnNumbers));
            }

            if (bingoBoards == null)
            {
                throw new ArgumentNullException(nameof(bingoBoards));
            }

            // find first board that wins
            foreach (int drawnNumber in drawnNumbers)
            {
                foreach (BingoBoard bingoBoard in bingoBoards)
                {
                    bingoBoard.MarkNumber(drawnNumber);
                    if (bingoBoard.IsWin())
                    {
                        return drawnNumber * bingoBoard.GetScore();
                    }
                }
            }

            throw new Exception("No winning board");
        }

        static int SimulateGameLastWinningBoard(int[] drawnNumbers, BingoBoard[] bingoBoards)
        {
            if (drawnNumbers == null)
            {
                throw new ArgumentNullException(nameof(drawnNumbers));
            }

            if (bingoBoards == null)
            {
                throw new ArgumentNullException(nameof(bingoBoards));
            }

            // find last board that wins
            int alreadyFinishedBoards = 0;
            foreach (int drawnNumber in drawnNumbers)
            {
                foreach (BingoBoard bingoBoard in bingoBoards)
                {
                    bingoBoard.MarkNumber(drawnNumber);
                    
                    // only count boards that are not already in winning state
                    if (!bingoBoard.IsInWinningState && bingoBoard.IsWin())
                    {
                        alreadyFinishedBoards++;
                    }

                    if (alreadyFinishedBoards == bingoBoards.Length)
                    {
                        return drawnNumber * bingoBoard.GetScore();
                    }
                }
            }

            throw new Exception("No winning board");
        }
    }

    class BingoBoard
    {
        public int Dimension { get; }

        public BingoCell[][] Cells { get; }

        public bool IsInWinningState { get; set; }
        
        private int LastRowIndex;

        public BingoBoard(int dimension)
        {
            Dimension = dimension;
            Cells = new BingoCell[dimension][];
            IsInWinningState = false;
            LastRowIndex = 0;
        }

        public void AddRow(int[] rowData)
        {
            Cells[LastRowIndex] = rowData
                .Select(x => new BingoCell()
                {
                    Marked = false,
                    Value = x
                })
                .ToArray();

            LastRowIndex++;
        }

        public void MarkNumber(int number)
        {
            foreach (BingoCell[] row in Cells)
            {
                foreach (BingoCell cell in row)
                {
                    if (cell.Value == number)
                    {
                        cell.Marked = true;
                    }
                }
            }
        }

        public bool IsWin()
        {
            // horizontal check
            for (int i = 0; i < Dimension; i++)
            {
                bool rowWin = true;
                for (int j = 0; j < Dimension; j++)
                {
                    rowWin = rowWin && Cells[i][j].Marked;

                    if (!rowWin)
                    {
                        break;
                    }
                }

                if (rowWin)
                {
                    IsInWinningState = true;
                    return true;
                }
            }

            // vertical check
            for (int i = 0; i < Dimension; i++)
            {
                bool colWin = true;
                for (int j = 0; j < Dimension; j++)
                {
                    colWin = colWin && Cells[j][i].Marked;

                    if (!colWin)
                    {
                        break;
                    }
                }

                if (colWin)
                {
                    IsInWinningState = true;
                    return true;
                }
            }

            return false;
        }

        public int GetScore()
        {
            int score = 0;
            foreach (BingoCell[] row in Cells)
            {
                foreach (BingoCell cell in row)
                {
                    if (!cell.Marked)
                    {
                        score += cell.Value;
                    }
                }
            }
            return score;
        }

        public void ResetBoard()
        {
            IsInWinningState = false;
            foreach (BingoCell[] row in Cells)
            {
                foreach (BingoCell cell in row)
                {
                    cell.Marked = false;
                }
            }
        }
    }

    class BingoCell
    {
        public bool Marked { get; set; }

        public int Value { get; set; }
    }
}
