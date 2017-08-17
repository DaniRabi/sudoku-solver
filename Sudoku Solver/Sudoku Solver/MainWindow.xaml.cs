using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku_Solver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<int> sequence;
        public CellGrids grids;
        private int size;
        private int root;

        public MainWindow()
        {
            InitializeComponent();

            size = 9;
            MessageBoxResult result = MessageBox.Show("Do you want a 16x16 sudoku solver?", "Size", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes == result)
                size = 16;

            root = (int)Math.Sqrt(size);
            sequence = new List<int>();
            int from = 0;
            int to = size;

            for (int n = 0; n < root; n++)
            {
                for (int i = 0; i < root; i++)
                    for (int j = from; j < to; j++)
                        sequence.Add(j / root);

                from += size;
                to += size;
            }

            CreateBoard();
        }

        private void CreateBoard()
        {
            for (int i = 0; i < size; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                board.ColumnDefinitions.Add(col);

                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                board.RowDefinitions.Add(row);
            }

            grids = new CellGrids(size);

            for (int i = 0; i < Math.Pow(size, 2); i++)
            {
                Cell cell = new Cell(i, size);
                Grid.SetRow(cell, i / size);
                Grid.SetColumn(cell, i % size);
                board.Children.Add(cell);

                int grid = sequence[i];
                grids.Add(cell, grid);
            }
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            List<Cell> cells = board.Children.Cast<Cell>().ToList();
            List<Cell> emptyCells = cells.FindAll(c => c.IsEmpty());
            //while (emptyCells.Count > 0)
            //{
                foreach (Cell cell in emptyCells)
                {
                    int row = Grid.GetRow(cell); // cell.Location / 9;
                    int col = Grid.GetColumn(cell); //cell.Location % 9;
                    for (int i = 1; i <= size; i++)
                    {
                        if (IsInRow(i, row) || IsInCol(i, col))
                        {
                            cell.RemoveOption(i);
                            if (!cell.IsEmpty())
                                break;
                        }
                        if (cell.HasOption(i) && (!OptionIsInRow(i, row, cell.Location) || !OptionIsInCol(i, col, cell.Location)))
                        {
                            cell.Set(i);
                            break;
                        }
                    }
                }

                grids.Update();

            //    emptyCells = cells.FindAll(c => c.IsEmpty());
            //}
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Math.Pow(size, 2); i++)
            {
                Cell cell = board.Children[i] as Cell;
                cell.Reset();
            }
        }

        private bool IsInRow(int num, int row)
        {
            return IsInRowHelp(num, row * size, 0);
        }
        private bool IsInRowHelp(int num, int rowIndex, int i)
        {
            if (i == size)
                return false;
            Cell cell = board.Children[rowIndex + i] as Cell;
            if (cell.Number == num)
                return true;
            return IsInRowHelp(num, rowIndex, i + 1);
        }
        private bool IsInCol(int num, int col)
        {
            return IsInColHelp(num, col, 0);
        }
        private bool IsInColHelp(int num, int col, int i)
        {
            if (i >= Math.Pow(size, 2))
                return false;
            Cell cell = board.Children[col + i] as Cell;
            if (cell.Number == num)
                return true;
            return IsInColHelp(num, col, i + size);
        }

        private bool OptionIsInRow(int option, int row, int cellIndex)
        {
            return OptionIsInRowHelp(option, row * size, 0, cellIndex);
        }
        private bool OptionIsInRowHelp(int option, int rowIndex, int i, int cellIndex)
        {
            if (i == size)
                return false;
            if (rowIndex + i != cellIndex)
            {
                Cell cell = board.Children[rowIndex + i] as Cell;
                if (cell.HasOption(option))
                    return true;
            }
            return OptionIsInRowHelp(option, rowIndex, i + 1, cellIndex);
        }
        private bool OptionIsInCol(int option, int col, int cellIndex)
        {
            return OptionIsInColHelp(option, col, 0, cellIndex);
        }
        private bool OptionIsInColHelp(int option, int col, int i, int cellIndex)
        {
            if (i >= Math.Pow(size, 2))
                return false;
            if (col + i != cellIndex)
            {
                Cell cell = board.Children[col + i] as Cell;
                if (cell.HasOption(option))
                    return true;
            }
            return OptionIsInColHelp(option, col, i + size, cellIndex);
        }
    }
}
