using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak_Card_Game
{
    class StartDurakGame
    {
        public bool PlayDurak()
        {
            DurakGameplay game = new DurakGameplay();
            game.Play();
            return DurakGameplay.GameResult;
        }
    }
}
