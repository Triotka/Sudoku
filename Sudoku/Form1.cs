using System;
using System.IO;
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
        const int numberOfSudokus = 50;
        const int sudokuSize = 9;
        public int sizeOfGrid = 40;
        const int buttonWidth = 80;
        const int buttonHeight = 40;
        const int buttonPadding = 20; 
        public int[,] numbers = new int[sudokuSize, sudokuSize];
        public Grid[,] grids = new Grid[sudokuSize, sudokuSize];
        public bool[,] loaded = new bool[sudokuSize, sudokuSize];
        Button checkButton;
        Button startButton;
        Button clearButton;
        public int[,,] sudokus = new int[50, 9, 9];
        Color color1 = Color.BlanchedAlmond;
        Color color2 = Color.White;


        public void loadSudoku(int number)
        {
            for (int i = 0; i < sudokuSize; i++ )
            {
                for (int j = 0; j < sudokuSize; j++)
                {
                    numbers[j, i] = sudokus[number, i, j];
                    if (numbers[j, i] != 0)
                    {
                        loaded[j, i] = true;
                    }
                }
            }
        }

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
        public void CreateButtons()
        {
            checkButton = new Button();
            clearButton = new Button();
            startButton = new Button();

            clearButton.Text = "Clear";
            this.Controls.Add(clearButton);
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);

           
            startButton.Text = "Start";
            this.Controls.Add(startButton);
            this.startButton.Click += new System.EventHandler(this.startButton_Click);


            checkButton.Text = "Check";
            this.Controls.Add(checkButton);
            this.startButton.Click += new System.EventHandler(this.checkButton_Click);

            changesSizesButtons();

        }
        public void changesSizesButtons()
        {
            clearButton.Size = new Size(buttonWidth, buttonHeight);
            clearButton.Location = new Point(buttonPadding, buttonPadding);

            startButton.Size = new Size(buttonWidth, buttonHeight);
            startButton.Location = new Point(buttonPadding, buttonHeight + buttonPadding * 2);

            checkButton.Size = new Size(buttonWidth, buttonHeight);
            checkButton.Location = new Point(buttonPadding, buttonHeight * 2 + buttonPadding * 3);

        }
        public void changeSizesSudoku()
        {
            for (int r = 0; r < sudokuSize; r++)
            {
                for (int c = 0; c < sudokuSize; c++)
                {
                    grids[r, c].Location = new Point(buttonPadding * 2 + buttonWidth + r * sizeOfGrid, c * sizeOfGrid);
                    grids[r, c].Size = new Size(sizeOfGrid, sizeOfGrid);
                }
            }
        }


        public void colorSudoku()
        {
            for (int r = 0; r < sudokuSize; r++)
            {
                for (int c = 0; c < sudokuSize; c++)
                {
                    if (r >= 3 && r < 6)
                    {
                        if (c >= 3 && c < 6)
                        {
                            grids[r, c].BackColor = color1;
                        }
                        else
                        {
                            grids[r, c].BackColor = color2;
                        }
                    }
                    else
                    {
                        if (!(c >= 3 && c < 6))
                        {
                            grids[r, c].BackColor = color1;
                        }
                        else
                        {
                            grids[r, c].BackColor = color2;
                        }
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
                    grids[r, c].KeyPress += grid_keyPressed;
                    grids[r, c].numberInRow = r;
                    grids[r, c].numberInCol = c;
                    this.Controls.Add(grids[r,c]);

                    
                    

                }
            }
            updateGridText();
            changeSizesSudoku();
            colorSudoku();
          
        }
        public Form1()
        {
            InitializeComponent();
            readSudokus();
            loadSudoku(1);
            CreateSudoku();
            CreateButtons();
        }

        public void readSudokus()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = Path.Combine(currentDirectory, "sudoku.txt");
            StreamReader file = new StreamReader(path);


            for (int i = 0; i< numberOfSudokus; i++)
            {
                for (int j = 0; j <= sudokuSize; j++)
                {
                   string line =  file.ReadLine();
                    if (j != 0)
                    {
                        for (int k = 0; k < sudokuSize; k++)
                        {
                            sudokus[i, j - 1, k] = int.Parse(line[k].ToString());
                        }
                    }
                
                   
                }
   
            }
            file.Close();


           
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            
        }


        private void checkButton_Click(object sender, EventArgs e)
        {
            
           
        }
        private void clearButton_Click(object sender, EventArgs e)
        {
            for (int r = 0; r < sudokuSize; r++)
            {
                for (int c = 0; c < sudokuSize; c++)
                {
                    if (loaded[r, c] == false)
                        numbers[r, c] = 0;
                }
            }
            updateGridText();
        }

        private void grid_keyPressed(object sender, KeyPressEventArgs e)
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
