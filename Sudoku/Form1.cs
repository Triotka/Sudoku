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
        const int buttonWidth = 100;
        const int buttonHeight = 40;
        const int buttonPadding = 20;
        const int numOfDifficulties = 5;
        public int[,] numbers = new int[sudokuSize, sudokuSize];
        public Grid[,] grids = new Grid[sudokuSize, sudokuSize];
        public bool[,] loaded = new bool[sudokuSize, sudokuSize];
        public RadioButton[] radioButtons = new RadioButton [numOfDifficulties];
        Button checkButton;
        Button startButton;
        Button clearButton;
        Button solutionButton;
        public int[,,] sudokus = new int[50, 9, 9];
        public int[,] solvedSudoku = new int[sudokuSize, sudokuSize];
        Color color1 = Color.BlanchedAlmond;
        Color color2 = Color.White;
        Label label;
        






        public static int[,] solveSudoku(int[,] original)
        {
            int[,] solved = (int[,])original.Clone();

            solve(solved);
            return solved;
        }
        private static bool solve(int [,] game)
        {
            for (int r = 0; r < game.GetLength(0); r++)
            {
                for (int c = 0; c < game.GetLength(1); c++)
                {
                    if (game[r, c] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (isValid(game, r, c, num))
                            {
                                game[r, c] = num;

                                if (solve(game))
                                {
                                    return true;
                                }

                                else
                                {
                                    game[r, c] = 0;
                                }
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }
        private static bool isValid(int[,] game, int row, int col, int num)
        {
            for (int i = 0; i < 9; i++)
            {
                //check row  
                if (game[i, col] != 0 && game[i, col] == num)
                    return false;
                //check column  
                if (game[row, i] != 0 && game[row, i] == num)
                    return false;
                //check 3*3 block  
                if (game[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] != 0 && game[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] == num)
                    return false;
            }
            return true;
        }

        public void createLabel()
        {
            label = new Label();
            this.Controls.Add(label);
            label.Location = new Point(buttonPadding * 3 + buttonWidth + sudokuSize * sizeOfGrid, buttonPadding * (5 * 2 + 1));
            label.Text = "Ahoj";

        }


        




        public void createRadioButtons()
        {
            string[] names = { "Very Easy", "Easy", "Moderate", "Hard", "Very Hard" };
            for (int i = 0; i < numOfDifficulties; i++)
            {
                radioButtons[i] = new RadioButton();
                this.Controls.Add(radioButtons[i]);
                radioButtons[i].Text = names[i];
                radioButtons[i].Location = new Point(buttonPadding * 3 + buttonWidth + sudokuSize * sizeOfGrid, buttonPadding * (i*2 + 1));
                radioButtons[i].AutoSize = true;

            }
            radioButtons[0].Checked = true;
        }
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
            solvedSudoku = solveSudoku(numbers);
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
            solutionButton = new Button();

            clearButton.Text = "Clear";
            this.Controls.Add(clearButton);
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);

           
            startButton.Text = "Start";
            this.Controls.Add(startButton);
            this.startButton.Click += new System.EventHandler(this.startButton_Click);


            checkButton.Text = "Check";
            this.Controls.Add(checkButton);
            this.startButton.Click += new System.EventHandler(this.checkButton_Click);

            solutionButton.Text = "Solution";
            this.Controls.Add(solutionButton);
            this.solutionButton.Click += new System.EventHandler(this.solutionButton_Click);
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

            solutionButton.Size = new Size(buttonWidth, buttonHeight);
            solutionButton.Location = new Point(buttonPadding, buttonHeight * 3 + buttonPadding * 4);

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
            CreateSudoku();
            CreateButtons();
            createRadioButtons();
            createLabel();
            startGame();
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

        public void startGame()
        {
            for (int i = 0; i < numOfDifficulties; i++)
            {

                if (radioButtons[i].Checked == true)
                {

                    Random r = new Random();
                    int sudokuNumber = r.Next(i * 10, (i + 1) * 10);
                    loadSudoku(sudokuNumber);
                    updateGridText();
                    break;
                }
            }
        }
        private void startButton_Click(object sender, EventArgs e)
        {
            startGame();
        }

        private void solutionButton_Click(object sender, EventArgs e)
        {
            for (int r = 0; r < sudokuSize; r++)
            {
                for (int c = 0; c < sudokuSize; c++)
                {
                    numbers[r, c] = solvedSudoku[r, c];
                }
            }
            updateGridText();
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
                    
                    numbers[grid.numberInRow, grid.numberInCol] = int.Parse(e.KeyChar.ToString());

                }
                updateGridText();
            
            }
        }
     }
}
