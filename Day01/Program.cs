namespace Day01
{
    internal class Program
    {

        static void Part1(string file)
        {
            string[] lines = File.ReadAllLines(file);

            int sum = 0;
            foreach (string line in lines)
            {
                char[] chars = new char[2];
                chars[0]  = line[line.IndexOfAny("0123456789".ToArray())];
                chars[1] = line[line.LastIndexOfAny("0123456789".ToArray())];

                // Console.WriteLine($"{line} {chars[0]} {chars[1]}");

                sum += Int32.Parse(new string(chars));
            }
            Console.WriteLine($"{file} sum is {sum}");
        }


        static int FirstWord(string s)
        {
            string[] words = "zero,one,two,three,four,five,six,seven,eight,nine".Split(',');

            for (int i = 0; i < s.Length; i++)
            {
                for (int w = 0; w < words.Length; w++)
                {
                    // check for a number here
                    if (s[i] >= '0' && s[i] <= '9')
                        return (s[i] - '0');

                     // check for a word
                    int c = String.CompareOrdinal(words[w], 0, s, i, words[w].Length);
                    if (c == 0)
                        return w;
                }
            }

            return 0;
        }


        static int LastWord(string s)
        {
            string[] words = "zero,one,two,three,four,five,six,seven,eight,nine".Split(',');

            for (int i = s.Length-1; i >= 0; i--)
            {
                for (int w = 0; w < words.Length; w++)
                {
                    // check for a number here
                    if (s[i] >= '0' && s[i] <= '9')
                        return (s[i] - '0');

                    // check for a word
                    int c = String.CompareOrdinal(words[w], 0, s, i, words[w].Length);
                    if (c == 0)
                        return w;
                }
            }

            return 0;
        }


        static void Part2(string file)
        {
            string[] lines = File.ReadAllLines(file);

            int sum = 0;
            foreach (string line in lines)
            {
                int first = FirstWord(line);
                int last = LastWord(line);
                // Console.WriteLine($"{line} {first} {last}");
                sum += first * 10 + last;
            }

            Console.WriteLine($"{file} sum is {sum}");
        }

        static void Main(string[] args)
        {
            Part1("Sample1.txt");
            Part1("Data1.txt");

            Part2("Sample2.txt");
            Part2("Data1.txt");
        }
    }
}
