using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    public struct snakebody
    {
        public List<int> head_x, head_y;
        //public int tail_x, tail_y;
        
        public int length;
    }
    class board
    {
        bool terminate; 
        bool is_started;
        public int rows, columns;
        public List<List<char>> game; //the board
        private static Random rnd = new Random(); //used to generate apple position
        private int[] applePos = new int[2]; // stores the coordinate of the apple
        private bool appleIsEaten = true;
        private ConsoleKeyInfo key;
        snakebody boody = new snakebody();

        public board(int r, int c)
        {
            rows = r;
            columns = c;
        }
        public board(int size)
        {
            rows = size;
            columns = size;
        }

        public void initGame()
        {

            Console.CursorVisible = false;
            terminate = false;
            is_started = false;
            game = new List<List<char>>();
            
            boody.length = 3;
            boody.head_x = new List<int>();
            boody.head_y = new List<int>();

            for (int i = boody.length; i > 0 ; i--)
            {
                boody.head_x.Add((rows / 2) + 1 - i);
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
            
        }

        bool collisionControl()
        {
            if (is_started && !terminate)
            {
                if ((game[boody.head_x[boody.length - 1]][boody.head_y[boody.length - 1]] == 'X' || game[boody.head_x[boody.length - 1]][boody.head_y[boody.length - 1]] == '-'))
                {
                    terminate = !terminate;
                    Console.SetCursorPosition(columns / 2 + 1 - 4, rows / 2 + 1); Console.Write("You Loose");
                    return false;
                }
                else if (boody.head_x[boody.length - 1] == applePos[0] && boody.head_y[boody.length - 1] == applePos[1])
                {
                    appleIsEaten = true;
                }
                return true;
            }
            return false;
        }
        void update_Pos()
        {
            int tmp = 0; //used to update direction of snake
            if (Console.KeyAvailable)
            {
                is_started = true;
                key = Console.ReadKey();
            }
            
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    {
                        tmp = boody.head_x[boody.length - 1];
                        tmp--;
                        boody.head_x.Add(tmp);
                        boody.head_y.Add(boody.head_y[boody.length-1]);

                        break;
                    }
                case ConsoleKey.UpArrow:
                    {
                        tmp = boody.head_y[boody.length - 1];
                        tmp--;
                        boody.head_x.Add(boody.head_x[boody.length-1]);
                        boody.head_y.Add(tmp);

                        break;
                    }
                case ConsoleKey.RightArrow:
                    {
                        tmp = boody.head_x[boody.length - 1];
                        tmp++;
                        boody.head_x.Add(tmp);
                        boody.head_y.Add(boody.head_y[boody.length-1]);

                        break;
                    }
                case ConsoleKey.DownArrow:
                    {
                        tmp = boody.head_y[boody.length - 1];
                        tmp++;
                        boody.head_x.Add(boody.head_x[boody.length-1]);
                        boody.head_y.Add(tmp);

                        break;
                    }
                case ConsoleKey.Q:
                    {
                        terminate = true;
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
                Thread.Sleep(500); // update frequency

                update_Pos();

                if(collisionControl())
                {
                    Console.SetCursorPosition(boody.head_x[boody.length], boody.head_y[boody.length]); Console.Write('-');

                    if (appleIsEaten)
                    {
                        boody.length++;
                    }
                    else 
                    { 
                        Console.SetCursorPosition(boody.head_x[0], boody.head_y[0]); Console.Write(' ');
                        boody.head_x.RemoveAt(0); boody.head_y.RemoveAt(0);
                    }
                }
                if (appleIsEaten)
                {
                    ChangeapplePos();
                }

            }
            
            Console.SetCursorPosition(0, rows + 2);
          
        }
    }
}
