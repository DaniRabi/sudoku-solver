using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Cell.xaml
    /// </summary>
    public partial class Cell : UserControl
    {
        private static int boardSize; // number of options

        private List<int> options;

        //private int number;
        public int Number
        {
            get
            {
                if (input.Text == string.Empty)
                    return 0;
                return int.Parse(input.Text);
            }
            set
            {
                input.Text = value.ToString();
            }
        }

        private int location;
        public int Location
        {
            get { return location; }
        }

        public Cell(int location, int size)
        {
            InitializeComponent();
            boardSize = size;

            //if (size < 10)
            //    input.MaxLength = 1;
            //else input.MaxLength = 2;

            Reset();
            this.location = location;   
        }

        public void Reset()
        {
            input.Text = "";
            options = new List<int>();
            for (int i = 1; i <= boardSize; i++)
                options.Add(i);
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Regex regex = new Regex("[^0-9]");
            //e.Handled = !regex.IsMatch(input.Text);
            try
            {
                int.Parse(input.Text);
                options.Clear();
            }
            catch
            {
                input.Text = "";
            }
        }

        public void RemoveOption(int num)
        {
            if (!HasOption(num) || num > boardSize || num < 1)
                return;

            if (options.Count > 1 && HasOption(num))
                options.Remove(num);

            if (options.Count == 1)
                Set(options.First());
        }

        public bool HasOption(int option)
        {
            return options.Contains(option);
        }

        public void Set(int num)
        {
            Number = num;
            options.Clear();
        }

        public bool IsEmpty()
        {
            return options.Count > 1;
        }
    }
}
