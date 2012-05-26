using System;

namespace CodeGolf.ConwaysGol.Core
{
    public class Grid
    {
        public Cell[,] Cells;
        public int Dimension { get; private set; }

        public Grid(int dimension)
        {
            Dimension = dimension;
            Initialize();
            Genesis();
        }

        public void Initialize()
        {
            Cells = new Cell[Dimension,Dimension];

            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension; j++)
                {
                    Cells[i, j] = new Cell(i, j);
                }
            }
        }

        public void Genesis()
        {
            // Mark ~15% of the cells to alive
            var random = new Random();
            foreach (Cell cell in Cells)
            {
                cell.CellState = random.Next(100) < 15 ? Enums.CellState.Alive : Enums.CellState.Dead;
            }
        }

        public void Evolve()
        {
            SetNextState();
            UpdateCurrentState();
        }
        
        private void SetNextState()
        {
            foreach (Cell cell in Cells)
            {
                if (NeighborsCount(cell) <= 1)
                {
                    cell.NextState = cell.CellState == Enums.CellState.Alive ? Enums.CellState.DeadLonely : Enums.CellState.Dead;
                }
                else if (NeighborsCount(cell) == 2)
                {
                    cell.NextState = cell.CellState == Enums.CellState.Alive ? cell.CellState : Enums.CellState.Dead;
                }
                else if (NeighborsCount(cell) == 3)
                {
                    cell.NextState = Enums.CellState.Alive;
                }
                else // 4+
                {
                    cell.NextState = cell.CellState == Enums.CellState.Alive ? Enums.CellState.DeadOvercrowded : Enums.CellState.Dead;
                }
            }
        }

        private void UpdateCurrentState()
        {
            foreach (Cell cell in Cells)
            {
                cell.CellState = cell.NextState;
            }
        }

        private int NeighborsCount(Cell cell)
        {
            int neighbors = 0;

            for (int countRow = -1; countRow <= 1; countRow++)
            {
                for (int countCol = -1; countCol <= 1; countCol++)
                {
                    if (InGridBounds(countRow, countCol, cell) && !IsCell(countRow, countCol))
                    {
                        if (Cells[cell.Row + countRow, cell.Column + countCol].CellState == Enums.CellState.Alive)
                            neighbors++;
                    }
                }
            }

            return neighbors;
        }

        private bool InGridBounds(int countRow, int countCol, Cell cell)
        {
            return (cell.Row + countRow < Dimension) &&
                   (cell.Row + countRow >= 0) &&
                   (cell.Column + countCol < Dimension) &&
                   (cell.Column + countCol >= 0);
        }

        private static bool IsCell(int countRow, int countCol)
        {
            return (countCol == 0 && countRow == 0);
        }

        public bool LifeFound()
        {
            foreach (Cell cell in Cells)
            {
                if (cell.CellState != Enums.CellState.Dead) return true;
            }

            return false;
        }
    }
}