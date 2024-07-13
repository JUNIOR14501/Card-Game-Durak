using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.IO;

namespace Durak_Card_Game
{
    public class DurakGameplay
    {
        public List<DurakCard> GameDeck { get; set; }
        public List<DurakCard> ShuffledDeck { get; set; }
        public static List<DurakCard> Player = new List<DurakCard>();
        public List<DurakCard> AIplayer = new List<DurakCard>();
        public List<DurakCard> Swap = new List<DurakCard>();
        public DurakCard trump = new DurakCard();
        public DurakCard aiBid = new DurakCard();
        public DurakCard playerBid = new DurakCard();
        public static bool GameResult;


        public void Play()
        {
            WriteLineFileConsole("Чтобы победить, вы должны выиграть партию в игре \"Дурак\". Давайте начнем!!");
            GetGameDeck();
            Shuffle();
            DealHands();
            ShowTrump();
            TrumpCheckAndStart();
        }

        public void WriteLineFileConsole(string text)
        {
            StreamWriter s = new StreamWriter("protocol.txt", true);
            s.WriteLine(text.ToString());
            Console.WriteLine(text.ToString());
            s.Close();
        }

        //Turns
        public void AIstarts()
        {
            WriteLineFileConsole("\nAI начинает");
            AIturn();
        }
        public void AIturn()
        {
            if ((Player.Count > 0) && (ShuffledDeck.Count > 0))
            {
                Victory();
            }
            else
            {
                
                AIbids();
                PlayerBids();
                PlayerBidsResult();
            }
        }
        public void PlayerStarts()
        {
            WriteLineFileConsole("\nВы начинаете");
            PlayerTurn();
        }
        public void PlayerTurn()
        {
            if ((AIplayer.Count > 0) && (ShuffledDeck.Count > 0))
            {
                PlayerBids();
                AIdefence();
                AIdefenseResult();
            }
            else
            {
                Defeat();
            }
        }
        public bool Victory()
        {
            WriteLineFileConsole("\nВы выйграли!");
            //AskForReplay();
            return GameResult = true;
        }
        public bool Defeat()
        {
            WriteLineFileConsole("\nВы проиграли.");
            //AskForReplay();
            return GameResult = false;
        }

        //Actions
        public void GetGameDeck()
        {
            GameDeck = new List<DurakCard>();
            foreach (Suits suit in Enum.GetValues(typeof(Suits)))
            {
                foreach (Faces face in Enum.GetValues(typeof(Faces)))
                {
                    GameDeck.Add(new DurakCard { Suit = suit, Face = face });
                }
            }
        }
        public void Shuffle()
        {
            ShuffledDeck = GameDeck.OrderBy(c => Guid.NewGuid()).ToList();
        }
        public void DealHands()
        {
            Thread.Sleep(500);
            if (ShuffledDeck.Count > 0)
            {
                while (Player.Count < 6)
                {
                    Player.Add(ShuffledDeck[0]);
                    ShuffledDeck.RemoveAt(0);
                }
                while (AIplayer.Count < 6)
                {
                    AIplayer.Add(ShuffledDeck[0]);
                    ShuffledDeck.RemoveAt(0);
                }
            }
            WriteLineFileConsole($"\nСдача карт. {ShuffledDeck.Count.ToString()} карты, оставшиеся в колоде");
        }
        public void ShowTrump()
        {
            Swap.Add(ShuffledDeck[0]);
            ShuffledDeck.RemoveAt(0);
            ShuffledDeck.AddRange(Swap);
            Swap.Clear();
            trump = ShuffledDeck[ShuffledDeck.Count - 1];
            WriteLineFileConsole($"\n{trump.Suit} - козырь нашей игры.");
        }
        public void TrumpCheckAndStart()
        {
            WriteLineFileConsole("\nПроверяем, есть ли у вас на руках самый низкий козырь...");
            Thread.Sleep(500);
            var playerTrump = Player.OrderBy(o => o.Face).FirstOrDefault(s => s.Suit == trump.Suit);
            var aiTrump = AIplayer.OrderBy(o => o.Face).FirstOrDefault(s => s.Suit == trump.Suit);
            if (playerTrump == null)
            {
                AIstarts();
            }
            else
            {
                if (aiTrump == null)
                {
                    PlayerStarts();
                }
                else
                {
                    if (playerTrump.Face < aiTrump.Face)
                    {
                        PlayerStarts();
                    }
                    else
                    {
                        AIstarts();
                    }
                }
            }
        }
        public void AIbids()
        {
            Thread.Sleep(500);
            AIplayer.OrderBy(f => f.Face);
            aiBid = AIplayer.ElementAt(0);
            Swap.Add(AIplayer[0]);
            AIplayer.RemoveAt(0);
            WriteLineFileConsole($"\nAI кладет карту: {aiBid.ShowCard()}");
        }
        public void AIdefence()
        {
            Thread.Sleep(500);
            aiBid = AIplayer.FirstOrDefault(c => (c.Face > playerBid.Face && c.Suit == playerBid.Suit) ||
                                        (c.Face <= playerBid.Face && c.Suit == trump.Suit) ||
                                        (c.Face > playerBid.Face && c.Suit == trump.Suit) ||
                                        (c.Face <= playerBid.Face && c.Suit != trump.Suit));
            WriteLineFileConsole($"\nAI кроется картой: {aiBid.ShowCard()}");
            Swap.Add(aiBid);
            AIplayer.Remove(aiBid);
        }
        public void PlayerBids()
        {
            WriteLineFileConsole($"\nВыберите карту. Введите индекс и нажмите <Enter> (Козырь игры - {trump.Suit})");
            ShowPlayerHand();
            MakingBid();
        }
        public void MakingBid()
        {
            int.TryParse(Console.ReadLine(), out int PlChoice); 
            if ((PlChoice <= Player.Count) && (PlChoice > 0))
            {
                playerBid = Player.ElementAt(PlChoice);
                Swap.Add(playerBid);
                WriteLineFileConsole($"\nВаша карта: {playerBid.ShowCard()}");
                Player.RemoveAt(PlChoice);
            }
            else
            {
                WriteLineFileConsole("Вы должны выбрать карту из колоды по ее номеру");
                MakingBid();
            }
        }
        public void AItakes()
        {
            AIplayer.AddRange(Swap);
            Swap.Clear();
            WriteLineFileConsole("\nAI берет карту");
            Thread.Sleep(500);
            DealHands();
        }
        public void PlayerTakes()
        {
            Player.AddRange(Swap);
            Swap.Clear();
            WriteLineFileConsole("\nВы взяли карту");
            Thread.Sleep(500);
            DealHands();
        }
        public void Discard()
        {
            Swap.Clear();
            WriteLineFileConsole("\n---Бита---");
            Thread.Sleep(500);
            DealHands();
        }

        public void PlayerBidsResult()
        {
            if (PlayerCardHigher())
            {
                Discard();
                PlayerTurn();
            }
            else
            {
                PlayerTakes();
                AIturn();
            }
        }
        public void AIdefenseResult()
        {
            if (AIcardHigher())
            {
                Discard();
                AIturn();
            }
            else
            {
                AItakes();
                PlayerTurn();
            }
        }
        public void AskForReplay()
        {
            WriteLineFileConsole("Повторить? (Y/N)");
            string answer = Console.ReadLine();
            answer.ToLower();
            while (answer != "y" || answer != "n")
                switch (answer)
                {
                    case "y":
                        {
                            Play();
                            break;
                        }
                    case "n":
                        {
                            WriteLineFileConsole("Конец");
                            break;
                        }
                    default:
                        {
                            WriteLineFileConsole("Press Y or N");
                            break;
                        }
                }
        }

        //Conditions
        public bool AIFaceLower()
        {
            bool f = (aiBid.Face < playerBid.Face);
            return f;
        }
        public bool PlayerFaceLower()
        {
            bool f = (aiBid.Face > playerBid.Face);
            return f;
        }
        public bool EqualFaces()
        {
            bool f = (aiBid.Face == playerBid.Face);
            return f;
        }
        public bool SameSuit()
        {
            bool s = (aiBid.Suit == playerBid.Suit);
            return s;
        }
        public bool DiffSuits()
        {
            bool s = (aiBid.Suit != playerBid.Suit);
            return s;
        }
        public bool AIbidIsTrump()
        {
            bool s = (aiBid.Suit == trump.Suit);
            return s;
        }
        public bool PlayerBidIsTrump()
        {
            bool s = (playerBid.Suit == trump.Suit);
            return s;
        }
        public bool AIcardHigher()
        {
            bool c = (PlayerFaceLower() && SameSuit()) ||
                (PlayerFaceLower() && AIbidIsTrump()) ||
                (EqualFaces() && AIbidIsTrump()) ||
                (AIFaceLower() && AIbidIsTrump());
            return c;
        }
        public bool PlayerCardHigher()
        {
            bool c = (AIFaceLower() && SameSuit()) ||
                (AIFaceLower() && PlayerBidIsTrump()) ||
                (EqualFaces() && PlayerBidIsTrump()) ||
                (PlayerFaceLower() && PlayerBidIsTrump());
            return c;
        }

        //(un)comment to (see)hide display of hands and(or) deck. don't forget to insert/remove the methods' calls!
        public void ShowPlayerHand()
        {
            WriteLineFileConsole("\nВаши карты это:\n");
            foreach (DurakCard pCards in Player)
            {
                WriteLineFileConsole($"{Player.IndexOf(pCards)} - {pCards.ShowCard()}");
            }
        }
        public void ShowAIplayerHand()
        {
            WriteLineFileConsole("\nAI карты:\n");
            foreach (DurakCard aiCards in AIplayer)
            {
                WriteLineFileConsole($"{AIplayer.IndexOf(aiCards)} - {aiCards.ShowCard()}");
            }
        }
        public void ShowShuffledDeck()
        {
            WriteLineFileConsole("\nПеретасованная колода - это:\n");
            foreach (DurakCard card in ShuffledDeck)
            {
                WriteLineFileConsole($"{ShuffledDeck.IndexOf(card)} -  {card.ShowCard()}");
            }
        }
    }
}
