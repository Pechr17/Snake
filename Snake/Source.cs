using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class Source
    {
        
        static void Main(string[] args)
        {
            string play;
            board t1 = new board(25);
            do
            {
                t1.initGame();
                t1.updateBoard();

                //getScore()

                Console.WriteLine("Wanna play again? [y/n]");
                play = Console.ReadLine();

            } while (play == "y");
            
        }
    }
}
