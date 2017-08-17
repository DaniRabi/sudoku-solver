using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    public class CellGrid
    {
        private Cell[] grid;
        private int index;

        public CellGrid(int size)
        {
            grid = new Cell[size];
            index = 0;
        }

        public bool Add(Cell cell)
        {
            if (!IsFull())
            {
                grid[index] = cell;
                index++;
                return true;
            }
            return false;
        }

        public bool IsFull()
        {
            return index >= grid.Length;
        }

        public void Update()
        {
            for (int i = 0; i < grid.Length; i++)
            {
                Cell cell = grid[i];

                if (cell.IsEmpty())
                    for (int j = 1; j <= grid.Length; j++)
                    {
                        if (IsInGrid(j))
                            cell.RemoveOption(j);

                        if (cell.HasOption(j) && !OptionInGrid(j, i))
                        {
                            cell.Set(j);
                            break;
                        }
                    }
            }

        }

        public bool IsInGrid(int num)
        {
            return IsInGridHelp(num, 0);
        }
        private bool IsInGridHelp(int num, int index)
        {
            if (index == grid.Length)
                return false;
            if (grid[index].Number == num)
                return true;
            return IsInGridHelp(num, index + 1);
        }

        private bool OptionInGrid(int option, int cellIndex)
        {
            return OptionInGridHelp(option, 0, cellIndex);
        }
        private bool OptionInGridHelp(int option, int index, int cellIndex)
        {
            if (index == grid.Length)
                return false;
            if (index != cellIndex && grid[index].HasOption(option))
                    return true;
            return OptionInGridHelp(option, index + 1, cellIndex);
        }

    }
    public class CellGrids : List<CellGrid>
    {
        public CellGrids(int size) : base(size)
        {
            for (int i = 0; i < size; i++)
            {
                CellGrid grid = new CellGrid(size);
                Add(grid);
            }
        }

        public bool Add(Cell cell, int grid)
        {
            if (this[grid].Add(cell))
                return true;
            return false;
            //foreach (CellGrid grid in this)
            //    if (grid.Add(cell))
            //        return;
        }

        public void Update()
        {
            foreach (CellGrid grid in this)
                grid.Update();
        }
    }
}
