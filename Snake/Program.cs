using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    public struct snakebody
    {
        public List<int> head_x, head_y;
        public int score;
        
        public int length;
    }
    class board
    {
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

        public board(int r, int c, string sp)
        {
            rows = r;
            columns = c;
            switch(sp)
            {
                case "Easy":
                    {
                        speed = 500;
                        break;
                    }
            }
        }
        public board(int size)
        {
            rows = size;
            columns = size;
        }

        public void initGame()
        {
            first_move = true;
            boody.score = 0;
            Console.CursorVisible = false;
            terminate = false;
            is_started = false;
            game = new List<List<char>>();
            boody.length = 3;
            boody.head_x = new List<int>();
            boody.head_y = new List<int>();

            for (int i = boody.length; i > 0 ; i--)
            {
                boody.head_x.Add((rows / 2) + 1 + i);
                boody.head_y.Add(columns / 2 + 1);
            }
            
            for(int i = 0; i < rows + 2 ; i++)
            {
                game.Add(new List<char>());
                for(int j = 0; j < columns + 2; j++)
                {
                    if (i == 0 || i == rows + 1 || j == 0 || j == rows + 1)
                    {
                        game[i].Add('X'); // creates border
                    }
                    else
                    {
                        game[i].Add(' ');
                    }
                }
                
            }
            

            for (int k = 0; k < boody.length; k++)
            {
                game[boody.head_y[k]][boody.head_x[k]] = '-';
            }
            
            printBoard();

            ChangeapplePos();
            key = default(ConsoleKeyInfo);
        }
        void ChangeapplePos()
        {
            //Console.SetCursorPosition(applePos[0], applePos[1]); Console.Write(' ');

   
            applePos[0] = rnd.Next(1, rows); applePos[1] = rnd.Next(1, columns);
            while(game[applePos[0]][applePos[1]] != ' ')
            {
                applePos[0] = rnd.Next(1, rows); applePos[1] = rnd.Next(1, columns);
            }

            appleIsEaten = false;
            Console.SetCursorPosition(applePos[0], applePos[1]); Console.Write('A'); // Apple 
        }
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
            Console.SetCursorPosition(0, rows + 3); Console.Write("Score = " + boody.score);
            
        }

        bool collisionControl(int next_x, int next_y)
        {
            if (is_started && !terminate)
            {

                if (game[next_x][next_y] == 'X')
                {
                    Console.SetCursorPosition(columns / 2 + 1 - 4, rows / 2 + 1); Console.Write("You Loose");
                    return false;
                }
                else if (boody.head_x.Contains(next_x) && boody.head_y.Contains(next_y))
                {
                    if (boody.head_y[boody.head_x.IndexOf(next_x)] == next_y || boody.head_x[boody.head_y.IndexOf(next_y)] == next_x)
                    {
                        int tmpx = boody.head_x.IndexOf(next_x); int tmpy = boody.head_y.IndexOf(next_y);
                        if (!(boody.head_x.IndexOf(next_x) == 0) || !(boody.head_y.IndexOf(next_y) == 0))
                        {
                            //terminate = !terminate;
                            Console.SetCursorPosition(columns / 2 + 1 - 4, rows / 2 + 1); Console.Write("You Loose"); 

                            return false;
                        }
                    }
                }

                /*f(boody.head_x.IndexOf(next_x) == boody.head_y.IndexOf(next_y))//(boody.head_x.Contains(next_x) && boody.head_y.Contains(next_y)))
                {
                    if(boody.head_x.Contains(next_x) && boody.head_y.Contains(next_y))
                    {
                        */



                else if (next_x == applePos[0] && next_y == applePos[1])
                {
                    appleIsEaten = true;
                }
                return true;
            }
            return false;
        }
        void update_Pos()
        {
            int tmp_x = 0; //used to update direction of snake
            int tmp_y = 0;
            if (Console.KeyAvailable)
            {
                is_started = true;
                key = Console.ReadKey();
            }

            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    {
                        if (direction != "right")
                        {
                            direction = "left";
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
                case ConsoleKey.Q:
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

      
        public void updateBoard()
        {
            appleIsEaten = false;
            while(!terminate)
            {
                Thread.Sleep(speed); // update frequency

                update_Pos();

                if(!terminate && is_started)
                {
                    
                    if (appleIsEaten)
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
                if (appleIsEaten)
                {
                    ChangeapplePos();
                }

            }
            
            Console.SetCursorPosition(0, rows + 3);
          
        }
    }
}
