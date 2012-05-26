using System;
using System.Drawing;
using System.Windows.Forms;
using CodeGolf.ConwaysGol.Core;

namespace CodeGolf.ConwaysGol
{
    public partial class Form1 : Form
    {
        private const int Dimension = 80;
        private const int CellSize = 10;
        private const int CellPadding = 5;
        private Grid _grid;

        public Form1()
        {
            InitializeComponent();

            SetBounds(Left, Top, (Dimension + 2)*CellSize + CellPadding, (Dimension + 5)*CellSize + CellPadding);

            Cursor = Cursors.WaitCursor;

            StartGame();

            Cursor = Cursors.Arrow;
        }

        private void StartGame()
        {
            _grid = new Grid(Dimension);
            _grid.Genesis(); //Randomly mark cells as living

            timer1.Enabled = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawGridBackground(g);
            DrawGrid(g);
        }

        private void DrawGridBackground(Graphics g)
        {
            g.FillRectangle(Brushes.Black, ClientRectangle);
        }

        public void DrawGrid(Graphics g)
        {
            foreach (Cell cell in _grid.Cells)
            {
                Brush fillBrush = GetCellColor(cell);
                g.FillRectangle(fillBrush, cell.Row*CellSize + CellPadding + 1, cell.Column*CellSize + CellPadding + 1,
                                CellSize - 1, CellSize - 1);
            }
        }

        private static Brush GetCellColor(Cell cell)
        {
            Brush fillPen = Brushes.White;

            switch (cell.CellState)
            {
                case Enums.CellState.Dead:
                    fillPen = Brushes.Black;
                    break;
                case Enums.CellState.DeadLonely:
                    fillPen = Brushes.Blue;
                    //fillPen = Brushes.Black;
                    break;
                case Enums.CellState.DeadOvercrowded:
                    fillPen = Brushes.Red;
                    //fillPen = Brushes.Black;
                    break;
            }

            return fillPen;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _grid.Evolve();
            EndGameIfNoLifeRemains();
            Refresh();
        }

        private void EndGameIfNoLifeRemains()
        {
            if (_grid.LifeFound()) return;

            timer1.Enabled = false;

            DialogResult result = MessageBox.Show(@"All is lost. Start again?", @"Extinction", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
                StartGame();
            else
                Close();
        }
    }
}