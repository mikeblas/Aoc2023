using System.Drawing;

namespace Day02
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
                int id = int.Parse(line.Substring(5, colonPos - 5));

                string[] games = line.Substring(colonPos + 1).Split(';');

                // Console.WriteLine($"{id}");
                bool possible = true;
                foreach (string game in games)
                {
                    string[] counts = game.Split(",");

                    foreach (string count in counts)
                    {
                        string c = count.Trim();
                        int spacePos = c.IndexOf(" ");
                        int gemcount = int.Parse(c.Substring(0, spacePos));
                        string color = c.Substring(spacePos + 1);
                        // Console.WriteLine($"   {gemcount}, [{color}]");

                        if (color == "red" && gemcount > 12)
                            possible = false;
                        else if (color == "green" && gemcount > 13)
                            possible = false;
                        else if (color == "blue" && gemcount > 14)
                            possible = false;
                    }

                    if (!possible)
                        break;
                }

                if (possible)
                    sum += id;
            }

            Console.WriteLine($"{fileName}: total is {sum}");
        }


        static void Part2(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            int sum = 0;

            foreach (string line in lines)
            {
                int colonPos = line.IndexOf(':');
                int id = int.Parse(line.Substring(5, colonPos - 5));

                string[] games = line.Substring(colonPos + 1).Split(';');

                // Console.WriteLine($"{id}");
                Dictionary<string, int> maxNeeded = new();

                foreach (string game in games)
                {
                    string[] counts = game.Split(",");

                    foreach (string count in counts)
                    {
                        string c = count.Trim();
                        int spacePos = c.IndexOf(" ");
                        int gemcount = int.Parse(c.Substring(0, spacePos));
                        string color = c.Substring(spacePos + 1);
                        // Console.WriteLine($"   {gemcount}, [{color}]");

                        if (!maxNeeded.ContainsKey(color))
                        {
                            maxNeeded[color] = gemcount;
                        }
                        else
                        {
                            maxNeeded[color] = Math.Max(maxNeeded[color], gemcount);
                        }
                    }
                }

                int product = 1;
                foreach (KeyValuePair<string, int> pair in maxNeeded)
                {
                    // Console.WriteLine($"{pair.Key}: {pair.Value}");
                    product *= pair.Value;
                }

                //Console.WriteLine($"{product}");
                sum += product;

            }

            Console.WriteLine($"{fileName}: total is {sum}");
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
