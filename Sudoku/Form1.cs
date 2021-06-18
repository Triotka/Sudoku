using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public class Grid : Button
    {
        public int numberInRow;
        public int numberInCol;

       
    }
    public partial class Form1 : Form
    {
        const int sudokuSize = 9;
        const int sizeOfGrid = 40;
        public int[,] numbers = new int[sudokuSize, sudokuSize];
        public Grid[,] grids = new Grid[sudokuSize, sudokuSize];

        public void updateGridText()
        {
            for (int r = 0; r < sudokuSize; r++)
            {
                for (int c = 0; c < sudokuSize; c++)
                {
                    if (numbers[r, c] == 0)
                    {
                        grids[r, c].Text = "";
                    }
                    else
                    {
                        grids[r,c].Text = numbers[r, c].ToString();
                    }
                }
            }
            
        }
        public void CreateSudoku()
        {
            for (int r = 0; r < sudokuSize; r++)
            {
                for(int c = 0; c < sudokuSize; c++)
                {
                    grids[r,c] = new Grid();
                    grids[r, c].Location = new Point(r * sizeOfGrid, c * sizeOfGrid);
                    grids[r, c].Size = new Size(sizeOfGrid, sizeOfGrid);
                    grids[r, c].KeyPress += cell_keyPressed;
                    grids[r, c].numberInRow = r;
                    grids[r, c].numberInCol = c;
                    this.Controls.Add(grids[r,c]);
                    

                }
            }
            updateGridText();
        }
        public Form1()
        {
            InitializeComponent();
            CreateSudoku();
        }

        private void cell_keyPressed(object sender, KeyPressEventArgs e)
        {
            var grid = sender as Grid;

            if (Char.IsDigit(e.KeyChar))
            {
                 if (e.KeyChar == '0')
                 {
                   
                    numbers[grid.numberInRow, grid.numberInCol] = 0;
                 }
                else
                {
                    // grid.Text = e.KeyChar.ToString();
                    numbers[grid.numberInRow, grid.numberInCol] = int.Parse(e.KeyChar.ToString());

                }
                updateGridText();
            
            }
        }
     }
}
