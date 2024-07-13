using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak_Card_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            StartDurakGame game = new StartDurakGame();
            game.PlayDurak();
            Console.ReadLine();
        }
    }
}
