using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class Source
    {
        static void Main(string[] args)
        {
            board t1 = new board(25);
            t1.initGame();
            t1.updateBoard();
        }
    }
}
