namespace Day14
{
    internal class Program
    {
        static void PaintRocksNorth(char [,] spots, int x, int start, int end, int count)
        {
            for (int y = end; y < end+count; y++)
                spots[y,x] = 'O';
            for (int y = end + count; y <= start; y++)
                spots[y,x] = '.';
        }

        static void MoveNorth(char[,] spots)
        {
            for (int x = 0; x < spots.GetLength(1); x++)
            {
                int startY = -1;
                int rocks = 0;
                for (int y = spots.GetLength(0) - 1; y >= -1; y--)
                {
                    if (y == -1 || spots[y, x] == '#')
                    {
                        if (startY != -1)
                        {
                            PaintRocksNorth(spots, x, startY, y + 1, rocks);
                            startY = -1;
                            rocks = 0;
                        }
                    }
                    else
                    {
                        if (startY == -1)
                            startY = y;
                        if (spots[y, x] == 'O')
                        {
                            rocks += 1;
                        }
                    }
                }
            }
        }


        static void Part1(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            char[,] spots = new char[lines.Length, lines[0].Length];
            for (int y = 0; y < lines.Length; y++)
                for (int x = 0; x < lines[y].Length; x++)
                    spots[y, x] = lines[y][x];

            MoveNorth(spots);

            int weight = 0;
            for (int y = 0; y < lines.Length; y++)
            {
                int rocks = 0;

                for (int x = 0; x < lines[y].Length; x++)
                {
                    Console.Write(spots[y, x]);
                    if (spots[y, x] == 'O')
                        rocks++;
                }
                Console.WriteLine($"  {lines.Length - y}");

                weight += rocks * (lines.Length - y);
            }

            Console.WriteLine($"{fileName} has weight {weight}");
        }


        static void Main(string[] args)
        {
            Part1("sample.txt");
            Part1("part1.txt");
        }
    }
}
