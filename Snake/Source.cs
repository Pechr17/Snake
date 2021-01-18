using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class Source
    {
        
        static void Main(string[] args)
        {
            /*This section is used to test the TCP/IP section
            However this functionality has not been implemented in the snake game
             */
            //SocketClient C1 = new SocketClient();

            //C1.StartClient();
            
                       
            string play, playmode, ans; 
            
            
            board t1 = new board(25,50); //creates a snake game of 25x25 dimension.
            
            do
            { 
                Console.WriteLine("Enter gamemode: Easy, Normal or Hard!");
                playmode = Console.ReadLine();

                t1.changeMode(playmode);
                
                t1.initGame();
                t1.updateBoard();
                
                Console.WriteLine("Do you want to add score to highscore? [y/n]");
                ans = Console.ReadLine();
                if(ans == "y" || ans == "yes")
                {
                    t1.addHighscore();
                }
                Console.WriteLine("Wanna play again? [y/n]");
                play = Console.ReadLine();
                

            } while (play == "y");
            Console.Clear();
            t1.printHighScore(0);
            
        }
    }
}
