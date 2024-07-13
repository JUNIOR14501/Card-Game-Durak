using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak_Card_Game
{
    public class DurakCard
    {
        public Suits Suit { get; set; }
        public Faces Face { get; set; }

        public string ShowCard()
        {
            return $"{Face} {Suit}";
        }
    }
    public enum Suits
    {
        Черви,
        Бубы,
        Пики,
        Крестья
    }
    public enum Faces
    {
        Шесть = 6,
        Семь = 7,
        Восемь = 8,
        Девять = 9,
        Десять = 10,
        Валет = 11,
        Дева = 12,
        Король = 13,
        Туз = 14,
    }
}
