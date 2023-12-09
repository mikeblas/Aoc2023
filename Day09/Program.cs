namespace Day09
{
    internal class Program
    {
        static long[] GetNumbers(string line)
        {
            string[] values = line.Split(' ');
            long[] numbers = new long[values.Length];
            int i = 0;
            foreach (string value in values)
                numbers[i++] = long.Parse(value);

            return numbers;
        }

        static void Part1(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            long grandSum = 0;
            long grandDifference = 0;
            foreach (string line in lines)
            {
                long[] numbers = GetNumbers(line);

                foreach (long n in numbers)
                    Console.Write($"{n} ");
                Console.WriteLine();

                long[,] triangle = new long[numbers.Length, numbers.Length];
                for (int i = 0; i < numbers.Length; i++)
                    triangle[0,i] = numbers[i];

                int d = 1;
                bool allZeroes = false;
                do
                {
                    allZeroes = true;
                    for (int i = 0; i < numbers.Length - d; i++)
                    {
                        long diff = triangle[d - 1, i+1] - triangle[d - 1, i];
                        Console.Write($"{diff} ");
                        triangle[d,i] = diff;

                        if (diff != 0)
                            allZeroes = false;
                    }
                    Console.WriteLine();

                    d++;

                }
                while (!allZeroes && d < numbers.Length);


                long sum = 0;
                for (int i = d-2; i >=0; i--)
                {
                    sum += triangle[i, numbers.Length -i-1];
                }
                Console.WriteLine($"sum is {sum}");
                grandSum += sum;

                long[] diffColumn = new long[d]; 
                for (int i = d-2; i >=0; i--)
                {
                    diffColumn[i] = triangle[i, 0] - diffColumn[i+1];
                    // Console.WriteLine($"{triangle[i, 0]},   {diffColumn[i]}  ");
                }

                grandDifference += diffColumn[0];
                Console.WriteLine($"diff-sum is {diffColumn[0]}");

                Console.WriteLine();
            }

            Console.WriteLine($"{fileName} grand sum is {grandSum}");
            Console.WriteLine($"{fileName} grand diff is {grandDifference}");
        }


        static void Main(string[] args)
        {
            // Part1("sample1.txt");
            Part1("part1.txt");
        }
    }
}



