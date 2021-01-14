using System;
using System.Collections.Generic;
namespace Snake
{
    class board
    {
        public int rows, columns;
        public List<List<char>> game; //the board
        private static Random rnd = new Random(); //used to generate apple position
      
        private bool appleIsEaten = true;
        public board(int r, int c)
        {
            rows = r;
            columns = c;
        }

        public void initGame()
        {
            game = new List<List<char>>();
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
        }
        void ChangeapplePos()
        {
            game[rnd.Next(1, rows)][rnd.Next(1, columns)] = 'O'; // Apple 
        }
        public void printBoard()
        {
            if (appleIsEaten)
            {
                ChangeapplePos();
            }
            for (int i = 0; i < rows+2; i++)
            {
                for(int j = 0; j < columns+2; j++)
                {
                    Console.Write(game[i][j]);
                }
                Console.WriteLine();
            }
            
        }

        public void updateBoard()
        {

        }
    }
}
