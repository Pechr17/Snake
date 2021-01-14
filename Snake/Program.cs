using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    public struct snakebody
    {
        public int head_x, head_y;
        public int tail_x, tail_y;
        
        public int length;
    }
    class board
    {
        
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
            
            game = new List<List<char>>();
            
            boody.length = 3;

            boody.head_x = rows / 2 + 1; boody.head_y = columns / 2 + 1;
            boody.tail_x = boody.head_x + boody.length; boody.tail_y = boody.head_y;

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
                game[boody.head_y][boody.head_x + k] = '-';
            }
            printBoard();

            key = default(ConsoleKeyInfo);
        }
        void ChangeapplePos()
        {
            Console.SetCursorPosition(applePos[0], applePos[1]); Console.Write(' ');
            applePos[0] = rnd.Next(1, rows); applePos[1] = rnd.Next(1, columns);
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

        
        void update_Pos()
        {
            
            if (Console.KeyAvailable)
            {
                 key = Console.ReadKey();
            }
           
            switch (key.Key)
            {
                    case ConsoleKey.LeftArrow:
                        boody.head_x--;
                                   
                        break;
                    case ConsoleKey.UpArrow:
                        boody.head_y--;

                        break;
                    case ConsoleKey.RightArrow:
                        boody.head_x++;

                        break;
                    case ConsoleKey.DownArrow:
                        boody.head_y++;

                        break;
                    default:
                        break;
            }
        }

      
        public void updateBoard()
        {
            for(int i = 0; i < 50; i ++)
            {
                Thread.Sleep(500); // update frequency
                
                if(appleIsEaten)
                {
                    ChangeapplePos();
                }

                update_Pos();
                Console.SetCursorPosition(boody.head_x, boody.head_y); Console.Write('-');
                Console.SetCursorPosition(boody.tail_x, boody.tail_y); Console.Write(' ');
            }

            Console.SetCursorPosition(columns + 1, rows + 1);
        }
    }
}
