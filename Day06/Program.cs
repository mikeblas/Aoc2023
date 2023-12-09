using System.Diagnostics.CodeAnalysis;

namespace Day06
{
    internal class Program
    {

        static (long minPress, long maxPress) Race1(int race, long time, long distance)
        {
            long firstWin = -1;
            long lastWin = -1;
            for (long p = 1; p <= time; p++)
            {
                long travel = p * (time - p);
                // Console.WriteLine($"race {race}: pressed = {p}, travel = {travel}");

                if (travel > distance)
                {
                    if (firstWin == -1)
                        firstWin = p;
                    lastWin = p;
                }
            }

            Console.WriteLine($"{firstWin}, {lastWin}");

            return (firstWin, lastWin);
        }


        static void Part1(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            var timesString = lines[0].Split(':')[1].Trim();
            var distancesString = lines[1].Split(':')[1].Trim();

            string[] timeStrings = timesString.Split(" ");
            string[] distanceStrings = distancesString.Split(" ");

            List<int> times = new();
            foreach(string s in timeStrings)
            {
                if (String.IsNullOrEmpty(s)) continue;
                times.Add(int.Parse(s));
            }

            List<int> distances = new();
            foreach (string s in distanceStrings)
            {
                if (String.IsNullOrEmpty(s)) continue;
                distances.Add(int.Parse(s));
            }

            long product = 1;
            for (int i = 0;  i < times.Count; i++)
            {
                (long minPress, long maxPress) = Race1(i, times[i], distances[i]);

                product *= (maxPress - minPress) + 1;
            }
            Console.WriteLine($"product is {product}");
        }


        static void Part2(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            var timesString = lines[0].Split(':')[1].Trim().Replace(" ", "");
            var distancesString = lines[1].Split(':')[1].Trim().Replace(" ", "");


            long time = long.Parse(timesString);
            long distance = long.Parse(distancesString);

            (long minPress,long maxPress) = Race1(1, time, distance);
            Console.WriteLine($"{minPress}, {maxPress}");

            Console.WriteLine($"ways = {maxPress - minPress + 1}");
        }


        static void Main(string[] args)
        {
            // Part1("part1.txt");
            // Part2("sample.txt");
            Part2("part1.txt");
        }
    }
}
