namespace Day03
{
    internal class Program
    {
        static int FindNumberInside(string[] lines, int y, int x)
        {
            if (y < 0 || y >= lines.Length)
                return 0;
            if (x < 0 || x >= lines[y].Length)
                return 0;

            // not in a number
            if (!Char.IsDigit(lines[y][x]))
                return 0;

            // yes we are! go both ways to find start and end
            int start = x;
            while (start-1 >= 0 && Char.IsDigit(lines[y][start-1]))
                start--;

            int end = x;
            while (end+1 < lines[y].Length && Char.IsDigit(lines[y][end+1]))
                end++;

            string str = lines[y].Substring(start, end - start + 1);
            int n = int.Parse(str);
            // Console.WriteLine(n);
            return n;
        }

        static int FindNumberAlone(string[] lines, int y, int x)
        {
            if (y < 0 || y >= lines.Length)
                return 0;
            if (x < 0 || x >= lines[y].Length)
                return 0;

            // are we immediately inside a number?
            int temp = FindNumberInside(lines, y, x);
            if (temp != 0)
                return temp;

            // not, check left and also right
            int right = FindNumber(lines,  1, y, x + 1);
            int left  = FindNumber(lines, -1, y, x - 1);

            return right + left;
        }

        static List<int> FindNumberAlone2(string[] lines, int y, int x)
        {
            List<int> found = new();

            if (y < 0 || y >= lines.Length)
                return found;
            if (x < 0 || x >= lines[y].Length)
                return found;

            // are we immediately inside a number?
            int temp = FindNumberInside(lines, y, x);
            if (temp != 0)
            {
                found.Add(temp);
                return found;
            }

            // not, check left and also right
            int right = FindNumber(lines, 1, y, x + 1);
            int left = FindNumber(lines, -1, y, x - 1);

            if (right != 0)
                found.Add(right);
            if (left != 0)
                found.Add(left);
            return found;
        }


        static int FindNumber(string[] lines, int direction, int y, int x)
        {
            if (y < 0 || y >= lines.Length)
                return 0;
            if (x <0 || x >= lines[y].Length)
                return 0;

            if (!Char.IsDigit(lines[y][x]))
                return 0;

            string total = "";

            while (x >= 0 && x < lines[y].Length && Char.IsDigit(lines[y][x]))
            {
                total += lines[y][x];
                x += direction;
            }

            if (direction == -1)
                total = new string(total.Reverse().ToArray());

            // Console.WriteLine(total);
            return int.Parse(total);
        }

        static void Part1(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            int sum = 0;

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (Char.IsDigit(lines[y][x]) || lines[y][x] == '.')
                        continue;
                    Console.WriteLine(lines[y][x]);

                    // to the left is easy
                    sum += FindNumber(lines, -1, y, x - 1);

                    // so is to the right
                    sum += FindNumber(lines,  1, y, x + 1);

                    // above and below are a bit different
                    sum += FindNumberAlone(lines, y - 1, x);
                    sum += FindNumberAlone(lines, y + 1, x);
                }
            }

            Console.WriteLine($"{fileName} sum is {sum}");
        }



        static void Part2(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            int sum = 0;

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] != '*')
                        continue;
                    // Console.WriteLine(lines[y][x]);

                    List<int> ints = new();

                    // to the left is easy
                    int temp = FindNumber(lines, -1, y, x - 1);
                    if (temp != 0)
                        ints.Add(temp);

                    // so is to the right
                    temp = FindNumber(lines, 1, y, x + 1);
                    if (temp != 0)
                        ints.Add(temp);

                    // above and below are a bit different
                    ints.AddRange(FindNumberAlone2(lines, y - 1, x));
                    ints.AddRange(FindNumberAlone2(lines, y + 1, x));

                    if (ints.Count == 2)
                    {
                        int product = ints[0] * ints[1];
                        Console.WriteLine($"{product} = {ints[0]} * {ints[1]}");
                        sum += product;
                    }
                }
            }

            Console.WriteLine($"{fileName} sum is {sum}");
        }



        static void Main(string[] args)
        {
            // Part1("sample1.txt");
            // Part1("part1.txt");
            // Part2("sample1.txt");
            // Part2("extra2.txt");
            Part2("part1.txt");
        }
    }
}
