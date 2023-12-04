namespace Day04
{
    internal class Program
    {
        static void Part1(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            int sum = 0;

            foreach (string line in lines)
            {
                int colonPos = line.IndexOf(':');
                string cardName = line.Substring(0, colonPos);
                int cardNum = int.Parse(cardName.Substring(cardName.LastIndexOf(' ')));
                string nums = line.Substring(colonPos+2);
                int barPos = nums.IndexOf("|");
                string[] winnerString = nums.Substring(0, barPos).Trim().Split(' ');
                string[] pickedString = nums.Substring(barPos+1).Trim().Split(' ');

                HashSet<int> winners = new();

                int multiplier = 0;
                foreach (string w in winnerString)
                {
                    int winningNumber;
                    if (int.TryParse(w, out winningNumber))
                    {
                        winners.Add(winningNumber);
                    }
                }

                foreach (string p in pickedString)
                {
                    int pickedNumber;
                    if (int.TryParse(p, out pickedNumber))
                    {
                        if (winners.Contains(pickedNumber))
                        { 
                            if (multiplier == 0)
                                multiplier = 1;
                            else
                                multiplier *= 2;
                        }
                    }
                }


                Console.WriteLine($"{cardName}: {multiplier}");
                sum += multiplier;
            }

            Console.WriteLine($"{fileName}: {sum}");
        }


        static void Part2(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            int sum = 0;

            Dictionary<int, int> cardCounts = new();

            foreach (string line in lines)
            {
                int colonPos = line.IndexOf(':');
                string cardName = line.Substring(0, colonPos);
                int cardNum = int.Parse(cardName.Substring(cardName.LastIndexOf(' ')));
                string nums = line.Substring(colonPos + 2);
                int barPos = nums.IndexOf("|");
                string[] winnerString = nums.Substring(0, barPos).Trim().Split(' ');
                string[] pickedString = nums.Substring(barPos + 1).Trim().Split(' ');

                // we have one (more) of this card
                if (!cardCounts.ContainsKey(cardNum))
                    cardCounts.Add(cardNum, 0);
                cardCounts[cardNum]++;

                HashSet<int> winners = new();
                int matches = 0;
                foreach (string w in winnerString)
                {
                    int winningNumber;
                    if (int.TryParse(w, out winningNumber))
                    {
                        winners.Add(winningNumber);
                    }
                }

                foreach (string p in pickedString)
                {
                    int pickedNumber;
                    if (int.TryParse(p, out pickedNumber))
                    {
                        if (winners.Contains(pickedNumber))
                        {
                            matches++;
                        }
                    }
                }

                // subsequent cards get this many matches
                for (int i = 1; i <= matches; i++)
                {
                    if (!cardCounts.ContainsKey(cardNum+i))
                        cardCounts.Add(cardNum+i, 0);
                    cardCounts[cardNum+i] += cardCounts[cardNum];
                }

                Console.WriteLine($"{cardName}: {matches}");
            }

            foreach(KeyValuePair<int, int> pair in cardCounts)
            {
                Console.WriteLine($"{pair.Key}, {pair.Value}");
                sum += pair.Value;
            }

            Console.WriteLine($"{fileName}: {sum}");
        }

        static void Main(string[] args)
        {
            // Part1("sample1.txt");
            // Part1("part1.txt");
            Part2("sample1.txt");
            Part2("part1.txt");
        }
    }
}
