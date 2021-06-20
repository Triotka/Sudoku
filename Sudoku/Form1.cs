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
        public bool solved; //this grid was verified and has right solution user cannot type anymore

       
    }
    
    
    public partial class Form1 : Form
    {
        const int numberOfSudokus = 50; //number of sudokus in file
        const int sudokuSize = 9; //size of sudoku 9x9
        public int sizeOfGrid = 40; //size of square grid
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
        public int[,,] sudokus = new int[50, 9, 9]; //stored sudokus from file
        public int[,] solvedSudoku = new int[sudokuSize, sudokuSize]; //current sudoku solution

        
        //colors to paint sudoku
        Color color1 = Color.BlanchedAlmond;
        Color color2 = Color.White;
        Label pointsLabel; //label to view points
        int points; //count checked right answers
        
        // finds solution to sudoku given and returns the solution
        public int[,] SolveSudoku(int[,] original)
        {
            int[,] solved = (int[,])original.Clone();

            Solve(solved);
            return solved;
        }
        // recursively calling
        private static bool Solve(int [,] game)
        {
            for (int r = 0; r < game.GetLength(0); r++)
            {
                for (int c = 0; c < game.GetLength(1); c++)
                {
                    if (game[r, c] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsValid(game, r, c, num))
                            {
                                game[r, c] = num;

                                if (Solve(game))
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
        // checks if sudoku spaces are valid
        private static bool IsValid(int[,] game, int row, int col, int num)
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

        //creates label for points
        public void CreateLabel()
        {
            pointsLabel = new Label();
            this.Controls.Add(pointsLabel);
            pointsLabel.Location = new Point(buttonPadding * 3 + buttonWidth + sudokuSize * sizeOfGrid, buttonPadding * (6 * 2 + 1));
            pointsLabel.AutoSize = true;
            pointsLabel.Font = new Font("Arial", 14, FontStyle.Bold);

        }

        //creates Radio Buttons for choosing difficulty
        public void CreateRadioButtons()
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

        //loads sudoku certain sudoku of certain number from stored sudokus to screen and finds solution to it
        public void LoadSudoku(int number)
        {
            ChangePoints(0);
            for (int i = 0; i < sudokuSize; i++ )
            {
                for (int j = 0; j < sudokuSize; j++)
                {
                    grids[j, i].ForeColor = Color.Black; 
                    numbers[j, i] = sudokus[number, i, j];
                    if (numbers[j, i] != 0)
                    {
                        loaded[j, i] = true; // this number is loaded it is not set by player
                        grids[j, i].solved = true; //this number is definetely right, it is loaded
                    }
                    else
                    {
                        grids[j, i].solved = false; //not sure if it is right not checked or loaded
                    }
                }
            }
            solvedSudoku = SolveSudoku(numbers);
        }

        // makes numbers visible to player from numbers[,] array
        public void UpdateGridText()
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
        
        // creates buttons
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
            this.checkButton.Click += new System.EventHandler(this.checkButton_Click);

            solutionButton.Text = "Solution";
            this.Controls.Add(solutionButton);
            this.solutionButton.Click += new System.EventHandler(this.solutionButton_Click);
            ChangesSizesButtons();

        }

        //sets sizes and locations to buttons
        public void ChangesSizesButtons()
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

        //change sizes and location to sudoku
        public void ChangeSizesSudoku()
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


        //colors sudoku
        public void ColorSudoku()
        {
            for (int r = 0; r < sudokuSize; r++)
            {
                for (int c = 0; c < sudokuSize; c++)
                {
                    if (r >= 3 && r < 6) // midle rows
                    {
                        if (c >= 3 && c < 6) // middle collumns
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

        //creates sudoku from buttons as grids
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
            UpdateGridText();
            ChangeSizesSudoku();
            ColorSudoku();
          
        }
        
        // reads sudokus from file sudoku.txt which should be in folder where program is built.
        public void ReadSudokus()
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

        //starts a game by loading sudoku according to picked difficulty
        public void StartGame()
        {
            for (int i = 0; i < numOfDifficulties; i++)
            {

                if (radioButtons[i].Checked == true) //if we find picked difficulty,
                                                     //strange it does not work without == true
                {
                    Random r = new Random();
                    int sudokuNumber = r.Next(i * 10, (i + 1) * 10);    //for each difficulty we have 10 sudokus
                    LoadSudoku(sudokuNumber);
                    UpdateGridText();
                    break;
                }
            }
        }

        // change point to number passed and display in on label
        public void ChangePoints(int change)
        {
            points = change;
            pointsLabel.Text = "Score: " + change.ToString();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void solutionButton_Click(object sender, EventArgs e)
        {
            for (int r = 0; r < sudokuSize; r++)
            {
                for (int c = 0; c < sudokuSize; c++)
                {
                    numbers[r, c] = solvedSudoku[r, c]; 
                    grids[r, c].solved = true;  // we used solution button so we cannot type to sudoku
                }
            }
            UpdateGridText(); //display solution
        }

        //can be used to add points after asking for complete solution
        private void checkButton_Click(object sender, EventArgs e)
        {
            
            for (int r = 0; r < sudokuSize; r++)
            {
                for (int c = 0; c < sudokuSize; c++)
                {
                    if (numbers[r, c] != 0 && !(loaded[r,c])) //grids where a user typed something
                    {
                        grids[r, c].solved = true; //we cannot retype we asked for solution
                       
                        if (numbers[r,c] == solvedSudoku[r, c]) //it is a right solution 
                        {
                            if (grids[r,c].ForeColor == Color.Black) // it is new
                            {
                                points++;
                                grids[r, c].ForeColor = Color.MediumSeaGreen; 
                               
                                
                            }
                        }
                        else //bad solution
                        {
                            numbers[r, c] = solvedSudoku[r, c];
                            grids[r, c].Text = numbers[r, c].ToString(); //change to right one
                            grids[r, c].ForeColor = Color.Red;
                        }  
                    }
                }
            }
            ChangePoints(points);   //add points
           
        }

       
        private void clearButton_Click(object sender, EventArgs e)
        {
            ChangePoints(0); //zero out points
            
            for (int r = 0; r < sudokuSize; r++)
            {
                for (int c = 0; c < sudokuSize; c++)
                {
                    grids[r, c].ForeColor = Color.Black;
                    if (loaded[r, c] == false)
                    {
                        numbers[r, c] = 0;
                        grids[r, c].solved = false;
                    }
                       
                        
                }
            }
            UpdateGridText();
        }

        private void grid_keyPressed(object sender, KeyPressEventArgs e)
        {
            var grid = sender as Grid;
            if (grid.solved) //if it was asked for solution you cannot type
            {
                return;
            }    
            if (Char.IsDigit(e.KeyChar)) //key pressed is a number
            {
                 if (e.KeyChar == '0')
                 {
                   
                    numbers[grid.numberInRow, grid.numberInCol] = 0;
                 }
                else
                {
                    
                    numbers[grid.numberInRow, grid.numberInCol] = int.Parse(e.KeyChar.ToString());

                }
                UpdateGridText();
            
            }
        }

        public Form1()
        {
            InitializeComponent();
            ReadSudokus();
            CreateSudoku();
            CreateButtons();
            CreateRadioButtons();
            CreateLabel();
            StartGame();
        }
    }
}
