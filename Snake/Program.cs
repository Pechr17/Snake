using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Snake
{
    /// <summary>
    /// The embodiment of the snake. 
    /// The struct consists of two lists containing the x and y coordinate of each segment of the snake.
    /// A score is also associated with the snake, which is incremented each time an apple is eaten
    /// The length of the snake is also saved
    /// </summary>
    public struct snakebody
    {
        public List<int> head_x, head_y;
        public int score;
        public int length;
    }
    class board
    {
        //Various global variables used throughout the program
        int speed;
        bool terminate; 
        bool is_started; bool first_move;
        public int rows, columns;
        public List<List<char>> game; //the board
        private static Random rnd = new Random(); //used to generate apple position
        private int[] applePos = new int[2]; // stores the coordinate of the apple
        private bool appleIsEaten = true;
        private ConsoleKeyInfo key;
        snakebody boody = new snakebody();
        
        string direction = "start";
        string speedMode;
        public List<int> high_score = new List<int>();
        public Dictionary<string, int> high_score2 = new Dictionary<string, int>(); 

        public board()
        {
            rows = 25;
            columns = 25;
        }
        /// <summary>
        /// Costum constructor setting the size of the snake game
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        
        public board(int r, int c) 
        {
            rows = r;
            columns = c;
                       
        }
        /// <summary>
        /// Sets the update frequency of the game and thus the speed of the snake.
        /// </summary>
        /// <param name="sp">Choose between Easy, Normal og Hard</param> 
        public void changeMode(string sp)
        {
            speedMode = sp;
            switch (sp)
            {
                case "Easy":
                case "easy":
                    {
                        speed = 400;
                        break;
                    }
                case "Normal":
                case "normal":
                    {
                        speed = 200;
                        break;
                    }
                case "Hard":
                case "hard":
                    {
                        speed = 100;
                        break;
                    }
                default:
                    {
                        speedMode = "Easy";
                        speed = 400;
                        Console.WriteLine("Input not recognised. Gamemode set to {0}.", speedMode);
                        break;
                    }
            }
        }
        /// <summary>
        /// Initialises a game of rows x columns dimensions with the snake body in the middle
        /// </summary>
        public void initGame()
        {
            first_move = true;
            boody.score = 0;
            Console.CursorVisible = false;
            terminate = false;
            is_started = false;
            game = new List<List<char>>(); //Matrix representing the game
            WriteHighscore();
            boody.length = 3;
            boody.head_x = new List<int>();
            boody.head_y = new List<int>();
            direction = "start";

                       
            for(int i = 0; i < rows + 2 ; i++)
            {
                game.Add(new List<char>());
                for(int j = 0; j < columns + 2; j++)
                {
                    if (i == 0 || i == rows + 1 || j == 0 || j == columns + 1)
                    {
                        game[i].Add('X'); // creates border
                    }
                    else
                    {
                        game[i].Add(' ');
                    }
                }              
            }

            /*for (int k = 0; k < boody.length; k++)
            {
                game[boody.head_x[k]][boody.head_y[k]] = '-';
            }*/
            //Prints the board
            printBoard();

            //creates the snake
            int k = 0;
            for (int i = boody.length; i > 0; i--, k++)
            {
                boody.head_x.Add((columns / 2) + 1 + i);
                boody.head_y.Add(rows / 2 + 1);
                Console.SetCursorPosition(boody.head_x[k], boody.head_y[k]); Console.Write('-');
            }
            //Sets a random position for the apple
            ChangeapplePos();

            //Sets the key input to default
            key = default(ConsoleKeyInfo);
        }
        /// <summary>
        /// Generates a random position for the apple.
        /// The position is stored in applePos[][]
        /// </summary>
        void ChangeapplePos()
        {
            applePos[0] = rnd.Next(1, columns); applePos[1] = rnd.Next(1, rows);
            while(boody.head_x.Contains(applePos[0]) && boody.head_y.Contains(applePos[1]))
            {
                if (boody.head_y[boody.head_x.IndexOf(applePos[0])] == applePos[1] || boody.head_x[boody.head_y.IndexOf(applePos[1])] == applePos[0])
                {
                    applePos[0] = rnd.Next(1, columns); 
                    applePos[1] = rnd.Next(1, rows);
                }
                else
                {
                    break;
                }
            }

            appleIsEaten = false;
            Console.SetCursorPosition(applePos[0], applePos[1]); Console.Write('A'); // Apple 
            
        }
        /// <summary>
        /// Prints the board
        /// </summary>
        public void printBoard()
        {
            Console.Clear();
            for (int i = 0; i < rows+2; i++)
            {
                for(int j = 0; j < columns+2; j++)
                {
                    Console.Write(game[i][j]);
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, rows + 2); Console.WriteLine("Gamemode: " + speedMode);
            Console.Write("Score = " + boody.score + "       High Score = " + high_score[high_score.Count - 1]);
            
            printHighScore(columns);
            
        }
        /// <summary>
        /// Checks if the snake is coliding with the wall or the snake itself.
        /// </summary>
        /// <param name="next_x">Next x position of the snake head</param>
        /// <param name="next_y">Next y position of the snake head</param>
        /// <returns>False if the snake will colide. 
        /// True if the snake will not colide with an object.</returns>
        bool collisionControl(int next_x, int next_y)
        {
            if (is_started && !terminate)
            {
                if (game[next_y][next_x] == 'X')
                {
                    Console.SetCursorPosition(columns / 2 + 1 - 4, rows / 2 + 1); Console.Write("You Lose");
                    Console.SetCursorPosition(columns / 2 + 1 - 8, rows / 2 + 2); Console.Write("Your score was: " + boody.score);
                    return false;
                }
                else if (boody.head_x.Contains(next_x) && boody.head_y.Contains(next_y))
                {
                    if (boody.head_y[boody.head_x.IndexOf(next_x)] == next_y || boody.head_x[boody.head_y.IndexOf(next_y)] == next_x)
                    {
                        int tmpx = boody.head_x.IndexOf(next_x); int tmpy = boody.head_y.IndexOf(next_y);
                        if (!(boody.head_x.IndexOf(next_x) == 0) || !(boody.head_y.IndexOf(next_y) == 0))
                        {
                           Console.SetCursorPosition(columns / 2 + 1 - 4, rows / 2 + 1); Console.Write("You Lose");
                           Console.SetCursorPosition(columns / 2 + 1 - 8, rows / 2 + 2); Console.Write("Your score was: " + boody.score);

                           return false;
                        }
                    }
                }
                else if (next_x == applePos[0] && next_y == applePos[1])
                {
                    appleIsEaten = true;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Updates the position of the snake. This is where the magic happens!
        /// </summary>
        void update_Pos()
        {
            int tmp_x = 0; //used to update direction of snake
            int tmp_y = 0;
            if (Console.KeyAvailable) //Checks if a key is pressed
            {
                is_started = true; //starts the game when a key is pressed
                key = Console.ReadKey(true);
            }

            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    {
                        if (direction != "right") //Used to ensure that the snake cannot go in opposite direction
                        {
                            direction = "left";
                            tmp_x = boody.head_x[boody.length - 1];
                            tmp_x--;
                            tmp_y = boody.head_y[boody.length - 1];
                            if (collisionControl(tmp_x, tmp_y)) //checks if the next position is collision free prior to making the move
                            {
                                boody.head_x.Add(tmp_x);
                                boody.head_y.Add(tmp_y);
                            }
                            else
                            {
                                terminate = true;
                            }
                        }
                        else if (direction == "right")
                        {
                            tmp_x = boody.head_x[boody.length - 1];
                            tmp_y = boody.head_y[boody.length - 1];
                            tmp_x++;
                            if (collisionControl(tmp_x, tmp_y))
                            {
                                boody.head_x.Add(tmp_x);
                                boody.head_y.Add(tmp_y);
                            }
                            else
                            {
                                terminate = true;
                            }
                        }

                        break;
                    }
                case ConsoleKey.UpArrow:
                    {
                        if (direction != "down")
                        {
                            direction = "up";
                            tmp_x = boody.head_x[boody.length - 1];
                            tmp_y = boody.head_y[boody.length - 1];
                            tmp_y--;
                            if (collisionControl(tmp_x, tmp_y))
                            {
                                boody.head_x.Add(tmp_x);
                                boody.head_y.Add(tmp_y);
                            }
                            else
                            {
                                terminate = true;
                            }
                        }
                        else if (direction == "down")
                        {
                            tmp_y = boody.head_y[boody.length - 1];
                            tmp_x = boody.head_x[boody.length - 1];
                            tmp_y++;
                            if (collisionControl(tmp_x, tmp_y))
                            {
                                boody.head_x.Add(tmp_x);
                                boody.head_y.Add(tmp_y);
                            }
                            else
                            {
                                terminate = true;
                            }
                        }
                        break;
                    }
                case ConsoleKey.RightArrow:
                    {
                        if (direction != "left")
                        {
                            direction = "right";
                            if (first_move) //If the first direction is in right direction, the list is reversed
                            {
                                boody.head_x.Reverse();
                                first_move = false;
                            }

                            tmp_x = boody.head_x[boody.length - 1];
                            tmp_y = boody.head_y[boody.length - 1];
                            tmp_x++;
                            if (collisionControl(tmp_x, tmp_y))
                            {
                                boody.head_x.Add(tmp_x);
                                boody.head_y.Add(tmp_y);
                            }
                            else
                            {
                                terminate = true;
                            }
                        }
                        else if (direction == "left")
                        {
                            tmp_x = boody.head_x[boody.length - 1];
                            tmp_x--;
                            tmp_y = boody.head_y[boody.length - 1];
                            if (collisionControl(tmp_x, tmp_y))
                            {
                                boody.head_x.Add(tmp_x);
                                boody.head_y.Add(tmp_y);
                            }
                            else
                            {
                                terminate = true;
                            }
                        }

                        break;
                    }
                case ConsoleKey.DownArrow:
                    {
                        if (direction != "up")
                        {
                            direction = "down";
                            tmp_y = boody.head_y[boody.length - 1];
                            tmp_x = boody.head_x[boody.length - 1];
                            tmp_y++;
                            if (collisionControl(tmp_x, tmp_y))
                            {
                                boody.head_x.Add(tmp_x);
                                boody.head_y.Add(tmp_y);
                            }
                            else
                            {
                                terminate = true;
                            }
                        }
                        else if (direction == "up")
                        {
                            direction = "up";
                            tmp_x = boody.head_x[boody.length - 1];
                            tmp_y = boody.head_y[boody.length - 1];
                            tmp_y--;
                            if (collisionControl(tmp_x, tmp_y))
                            {
                                boody.head_x.Add(tmp_x);
                                boody.head_y.Add(tmp_y);
                            }
                            else
                            {
                                terminate = true;
                            }
                        }

                        break;
                    }
                case ConsoleKey.Q: //used to make it possible to quit the game
                    {
                        terminate = true;
                        Console.SetCursorPosition(0, rows + 2);
                        Console.WriteLine("you quit the game");
                        break;
                    }
                default:
                    break;
            }
        }
        /// <summary>
        /// prints the highest score
        /// </summary>
        public void WriteHighscore()
        {
            high_score.Add(boody.score);
            high_score.Sort();
                      
        }
        /// <summary>
        /// Adds a dictionary containing a user and a score.
        /// </summary>
        public void addHighscore()
        {
            Console.WriteLine("Add score to highscore - type username: ");
            string user = Console.ReadLine();
            if(high_score2.ContainsKey(user))
            {
                Console.WriteLine("Username is not unique. Want to update score of {0}? [y/n]", user);
                string tmp = Console.ReadLine();
                if(tmp == "y" || tmp == "yes")
                {
                    high_score2[user] = boody.score;
                }
                else
                {
                    do
                    {
                        Console.WriteLine("Choose new username...:");
                        user = Console.ReadLine();
                    } while (high_score2.ContainsKey(user));
                    high_score2.Add(user, boody.score);
                }
            }
            else
            {
                high_score2.Add(user, boody.score);
            }
            
        }
        /// <summary>
        /// Prints the highscore list. 
        /// </summary>
        /// <param name="start">Is used to move the highscore list to the side.</param>
        public void printHighScore(int start)
        {
            Console.SetCursorPosition(start + 3, 0);
            Console.WriteLine("Highscore: ");
            Console.SetCursorPosition(start + 3, 1);
            Console.WriteLine("User - Score ");
            int i = high_score2.Count;
            foreach (KeyValuePair<string,int> user in high_score2.OrderBy(key => key.Value)) //orders the highscore from smallest value to largest value
            {
                Console.SetCursorPosition(start + 3, 1 + i); Console.WriteLine("{0}. {1} - {2}", i, user.Key, user.Value); //prints the scores in reverse order
                i--;
            }
        }
        /// <summary>
        /// Updates the board thus enabling the snake to move. 
        /// </summary>
        public void updateBoard()
        {
            appleIsEaten = false;
            while(!terminate)
            {
                Thread.Sleep(speed); // update frequency

                update_Pos();

                if(!terminate && is_started)
                {
                    
                    if (appleIsEaten) //if the apple is eaten the length of the snake is incremented, thus the tail are not to be removed.
                    {
                        Console.SetCursorPosition(boody.head_x[boody.length], boody.head_y[boody.length]); Console.Write('-');
                        boody.length++;
                        boody.score++;
                        Console.SetCursorPosition(0, rows + 3); Console.Write("Score = " + boody.score);
                    }
                    else 
                    {
                        Console.SetCursorPosition(boody.head_x[0], boody.head_y[0]); Console.Write(' ');
                        boody.head_x.RemoveAt(0); boody.head_y.RemoveAt(0);
                        Console.SetCursorPosition(boody.head_x[boody.length - 1], boody.head_y[boody.length - 1]); Console.Write('-');
                    }
                    

                }
                if (appleIsEaten)//If the apple is eaten, the position of the apple is moved.
                {
                    ChangeapplePos();
                }

            }
            
            WriteHighscore();
            Console.SetCursorPosition(0, rows + 3);
          
        }
    }
}
