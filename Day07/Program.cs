using System.Diagnostics;

namespace Day07
{

    internal class Hand : IComparable<Hand>
    {
        readonly private int handType;
        readonly private string cards;
        readonly private int bid;
        readonly private bool partTwo;
        readonly static string ranks1 = "23456789TJQKA";
        readonly static string ranks2 = "J23456789TQKA";

        internal Hand(bool partTwo, string cards, int bid)
        {
            this.partTwo = partTwo;
            this.bid = bid;
            this.cards = cards;

            if (partTwo)
            {
                this.handType = GetType2(cards);
                CheckType2(cards, this.handType);
            }
            else
                this.handType = GetType1(cards);
        }

        internal string Cards { get { return cards; } }
        internal int Bid { get { return bid; } }

        internal int HandType { get { return handType; } }

        public int CompareTo(Hand? other)
        {
            // this is always greater than a null value
            if (other == null)
                return 1;

            // first by hand type
            int d = this.handType - other.handType;
            if (d != 0)
                return d;

            // then by card order
            d = 0;
            for (int i = 0; d == 0 && i < this.cards.Length; i++)
            {
                string ranks = partTwo ? ranks2 : ranks1;
                int t = ranks.IndexOf(this.cards[i]);
                int p = ranks.IndexOf(other.cards[i]);

                Debug.Assert(t >= 0);
                Debug.Assert(p >= 0);

                d = t - p;
            }
            if (d != 0)
                return d;

            return 0;
        }

        static int GetType1(string hand)
        {
            Dictionary<char, int> tally = new();
            Dictionary<int, int> tallyCounts = new();

            // tally the cards
            foreach (char c in hand)
            {
                if (!tally.ContainsKey(c))
                    tally[c] = 0;

                tally[c]++;
            }

            // find the max count and populate the tallyCounts
            int maxCount = -1;
            foreach (KeyValuePair<char, int> p in tally)
            {
                maxCount = Math.Max(maxCount, p.Value);

                if (!tallyCounts.ContainsKey(p.Value))
                    tallyCounts[p.Value] = 0;
                tallyCounts[p.Value]++;
            }
            // Console.WriteLine($"{hand} --> {maxCount}");

            if (maxCount == 5)
            {
                // five of a kind is the only possibility
                return 7;
            }
            if (maxCount == 4)
            {
                // four of a kind is the only choice here
                return 6;
            }
            if (maxCount == 3)
            {
                // three of a kind or full house?
                if (tallyCounts.ContainsKey(2))
                {
                    // full house
                    return 5;
                }
                else
                {
                    // three of a kind
                    return 4;
                }
            }
            if (maxCount == 2)
            {
                // two pair, or just one pair?
                if (tallyCounts[2] == 2)
                    return 3;
                else
                    return 2;
            }

            return 1;
        }


        static void CheckType2(string hand, int handType)
        {
            // key is card character, value is count of that card
            Dictionary<char, int> tally = new();

            // key is count of cards, value is number of times we have that count
            Dictionary<int, int> tallyCounts = new();

            // tally the cards
            int wildcardCount = 0;
            foreach (char c in hand)
            {
                if (c == 'J')
                    wildcardCount++;
                else
                {
                    if (!tally.ContainsKey(c))
                        tally[c] = 0;
                    tally[c]++;
                }
            }

            int maxCount = 0;
            foreach (KeyValuePair<char, int> p in tally)
            {
                if (p.Key == 'J')
                    continue;
                maxCount = Math.Max(maxCount, p.Value);

                if (!tallyCounts.ContainsKey(p.Value))
                    tallyCounts[p.Value] = 0;
                tallyCounts[p.Value]++;
            }

            switch (handType)
            {
                case 1:
                    // high card
                    Debug.Assert(wildcardCount == 0);
                    Debug.Assert(tallyCounts[1] == 5); // 23456
                    break;

                case 2:
                    // one pair
                    Debug.Assert(
                        wildcardCount == 1 && tallyCounts[1] == 4 ||
                        wildcardCount == 0 && tallyCounts[2] == 1 && tallyCounts[1] == 3);
                    break;

                case 3:
                    // not two pair: wildcardCount == 1 && tallyCounts[2] == 1 && tallyCounts[1] == 1 
                    //   because 2 cards should use the wild card to make 3 of a kind
                    Debug.Assert(wildcardCount == 0);

                    // two pair
                    Debug.Assert(wildcardCount == 0 && tallyCounts[2] == 2 && tallyCounts[1] == 1);  // KKQQ2
                    break;

                case 4:
                    // three of a kind
                    Debug.Assert(
                        wildcardCount == 0 && tallyCounts[3] == 1 && tallyCounts[1] == 2 ||     // KKK23
                        wildcardCount == 2 && tallyCounts[1] == 3 ||                            // JJK34
                        wildcardCount == 1 && tallyCounts[2] == 1 && tallyCounts[1] == 2);      // JKK23
                    break;

                case 5:
                    // full house
                    Debug.Assert(
                        wildcardCount == 0 && tallyCounts[2] == 1 && tallyCounts[3] == 1 ||     // KKQQQ
                        wildcardCount == 1 && tallyCounts[2] == 2);                             // KKJQQ
                    break;

                case 6:
                    // four of a kind
                    Debug.Assert(
                        wildcardCount == 0 && tallyCounts[4] == 1 && tallyCounts[1] == 1 ||     // KKKK2
                        wildcardCount == 1 && tallyCounts[3] == 1 && tallyCounts[1] == 1 ||     // JKKK2
                        wildcardCount == 2 && tallyCounts[2] == 1 && tallyCounts[1] == 1 ||     // JJKK2
                        wildcardCount == 3 && tallyCounts[1] == 2 // JJJK2
                        );
                    break;

                case 7:
                    // five of a kind
                    Debug.Assert(
                        wildcardCount == 5 ||     // JJJJJ
                        wildcardCount == 4 && tallyCounts[1] == 1 ||    // JJJJK
                        wildcardCount == 3 && tallyCounts[2] == 1 ||    // JJJKK
                        wildcardCount == 2 && tallyCounts[3] == 1 ||    // JJKKK
                        wildcardCount == 1 && tallyCounts[4] == 1       // JKKKK
                        );
                    break;
            }
        }

        static int GetType2(string hand)
        {
            // key is card character, value is count of that card
            Dictionary<char, int> tally = new();

            // key is count of cards, value is number of times we have that count
            Dictionary<int, int> tallyCounts = new();

            // tally the cards
            int wildcardCount = 0;
            foreach (char c in hand)
            {
                if (c == 'J')
                    wildcardCount++;
                else
                {
                    if (!tally.ContainsKey(c))
                        tally[c] = 0;
                    tally[c]++;
                }
            }

            if (wildcardCount == 5)
                Console.WriteLine("Five");


            // find the max count and populate the tallyCounts
            int maxCount = 0;
            foreach (KeyValuePair<char, int> p in tally)
            {
                if (p.Key == 'J')
                    continue;
                maxCount = Math.Max(maxCount, p.Value);

                if (!tallyCounts.ContainsKey(p.Value))
                    tallyCounts[p.Value] = 0;
                tallyCounts[p.Value]++;
            }
            // Console.WriteLine($"{hand} --> {maxCount}");

            if (maxCount == 5 || maxCount + wildcardCount == 5)
            {
                // five of a kind is the only possibility
                return 7;
            }
            if (maxCount == 4 || maxCount + wildcardCount == 4)
            {
                // four of a kind is the only choice here
                return 6;
            }
            if (maxCount == 3)
            {
                // three of a kind or full house?
                if (tallyCounts.ContainsKey(2) || wildcardCount == 2)
                {
                    // full house
                    return 5;
                }
                else
                {
                    // three of a kind
                    return 4;
                }
            }
            if (maxCount == 2)
            {
                // two pair, or just one pair?
                if (tallyCounts.ContainsKey(2) && tallyCounts[2] == 2 && wildcardCount == 0)
                    return 3;
                else if (tallyCounts.ContainsKey(2) && tallyCounts[2] == 2 && wildcardCount == 1)
                    return 5; // two pair plus a wildcard is a full house
                else if (tallyCounts.ContainsKey(2) && tallyCounts[2] == 1 && wildcardCount == 1)
                    return 4; // one pair plus a wildcard is three of a kind
                else
                    return 2;   // just a pair
            }
            else if (maxCount == 1)
            {
                // high card hand
                if (wildcardCount == 1)
                    // one wildcard is one pair
                    return 2;
                else if (wildcardCount == 2)
                    // two wildcards is three kind
                    return 4;
                else if (wildcardCount == 3)
                    // three wildcards is four kind
                    return 6;
                else if (tallyCounts[1] != 5)
                    throw new Exception($"Bogus one default one on {hand}");
            }

            return 1;
        }


    }

    internal class Program
    {
        static void Part1(bool partTwo, string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            List<Hand> hands = new();
            foreach (string line in lines)
            {
                var parts = line.Split(' ');
                int bid = int.Parse(parts[1]);

                var h = new Hand(partTwo, parts[0], bid);
                hands.Add(h);
                Console.WriteLine($"{h.Cards}, {h.Bid}, {h.HandType}");
            }

            hands.Sort();
            // hands.Reverse();

            int i = 1;
            int sum = 0;
            foreach (Hand hand in hands)
            {
                Console.WriteLine($"{i} {hand.Cards}, {hand.HandType}");

                sum += i * hand.Bid;
                i++;
            }

            Console.WriteLine($"{fileName} sum is {sum}");
        }

        static void Main(string[] args)
        {
            // Part1(false, "part1.txt");
            Part1(true, "part1.txt");
            // Part1(true, "sample1.txt");
        }
    }
}
