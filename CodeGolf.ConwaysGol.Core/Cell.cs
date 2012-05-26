namespace CodeGolf.ConwaysGol.Core
{
    public class Cell
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public Enums.CellState CellState { get; set; }
        public Enums.CellState NextState { get; set; }

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}