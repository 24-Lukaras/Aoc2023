using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2023
{
    internal static class Task07
    {

        public static int GetNormal()
        {
            int result = 0;
            using (var reader = new InputReader(7))
            {
                List<CammelDeck> decks = new List<CammelDeck>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var arr = line.Split(' ');
                    decks.Add(new CammelDeck(arr[0], Convert.ToInt32(arr[1])));
                }

                var ordered = decks.Order().ToArray();

                for (int i = 0; i < ordered.Length; i++)
                {
                    result += (i + 1) * ordered[i].Bid;
                }

            }

            return result;
        }

        public static int GetPlatinum()
        {
            int result = 0;
            using (var reader = new InputReader(7))
            {
                List<CammelDeck> decks = new List<CammelDeck>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var arr = line.Split(' ');
                    decks.Add(new CammelDeck(arr[0], Convert.ToInt32(arr[1]), true));
                }

                var ordered = decks.Order().ToArray();

                for (int i = 0; i < ordered.Length; i++)
                {
                    result += (i + 1) * ordered[i].Bid;
                }

            }

            return result;
        }



    }

    public class CammelDeck : IComparable<CammelDeck>
    {
        public string Input { get; private init; }
        public int Bid { get; private init; }

        private HandType handType;

        const string symbolOrder = "AKQJT98765432";
        const string symbolOrderPlat = "AKQT98765432J";
        bool platinumComparison;

        public CammelDeck(string input, int bid, bool plat = false)
        {
            Input = input;
            Bid = bid;
            platinumComparison = plat;
        }

        public int CompareTo(CammelDeck? other)
        {
            if (other == null) throw new NullReferenceException();

            if (handType == HandType.Undefined) GetHandType();

            var otherHandType = other.GetHandType();
            if (handType > otherHandType)
            {
                return 1;
            }
            else if (handType < otherHandType)
            {
                return -1;
            }

            var coll = platinumComparison ? symbolOrderPlat : symbolOrder;

            for (int i = 0; i < Input.Length; i++)
            {
                char current = Input[i];
                char otherChar = other.Input[i];

                int currentIndex = coll.IndexOf(current);
                int otherIndex = coll.IndexOf(otherChar);

                if (currentIndex < otherIndex)
                {
                    return 1;
                }
                else if (currentIndex > otherIndex)
                {
                    return -1;
                }
            }
            return 0;
        }

        public HandType GetHandType()
        {
            if (handType != HandType.Undefined) return handType;

            var groups = Input.GroupBy(x => x).ToArray();

            if (platinumComparison)
            {
                var jokerGroup = groups.FirstOrDefault(x => x.Key == 'J');

                if (jokerGroup != null && jokerGroup.Count() != 5)
                {
                    var bestReplacement = groups.Where(x => x.Key != 'J').OrderByDescending(x => x.Count()).ThenBy(x => symbolOrderPlat.IndexOf(x.Key)).First();

                    var newInput = Input.Replace('J', bestReplacement.Key);
                    groups = newInput.GroupBy(x => x).ToArray();
                }
            }

            switch (groups.Length)
            {
                default:
                    throw new ArgumentException(nameof(groups));

                case 5:
                    handType = HandType.HighCard;
                    return handType;

                case 4:
                    handType = HandType.OnePair;
                    return handType;

                case 3:
                    handType = (groups.Any(x => x.Count() == 3)) ? HandType.ThreeOfKind : HandType.TwoPair;
                    return handType;

                case 2:
                    handType = (groups.Any(x => x.Count() == 4)) ? HandType.FourOfKind : HandType.FullHouse;
                    return handType;

                case 1:
                    handType = HandType.FiveOfKind;
                    return handType;

            }
        }

        public enum HandType
        {
            Undefined,
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfKind,
            FullHouse,
            FourOfKind,
            FiveOfKind
        }
    }
}
