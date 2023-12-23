using System.Diagnostics;
using System.Transactions;

namespace Day15
{
    internal class Lens
    {

        internal Lens(int focalLength, string lensName)
        {
            this.FocalLength = focalLength;
            this.LensName = lensName;
        }

        internal string LensName { get; set; }
        internal int FocalLength { get; set; }
    }


    internal class Program
    {

        static void Part2Multi(string str)
        {
            List<List<Lens>> boxes = [];
            for (int i = 0; i < 256; i++)
                boxes.Add([]);

            string[] parts = str.Split(',');
            foreach (string part in parts)
            {
                int eq = part.IndexOf('=');
                int minus = part.IndexOf("-");
                if (eq != -1)
                {
                    // equals
                    string box = part.Substring(0, eq);
                    string right = part.Substring(eq + 1);
                    int lensLength = int.Parse(right);
                    int n = Part1(box);

                    int found = -1;
                    for (int i = 0; i < boxes[n].Count; i++)
                    {
                        if (boxes[n][i].LensName.Equals(box))
                        {
                            found = i;
                            break;
                        }
                    }

                    if (found == -1)
                        boxes[n].Add(new Lens(lensLength, box));
                    else
                    {
                        boxes[n][found].FocalLength = lensLength;
                    }
                }
                else if (minus != -1)
                {
                    string box = part.Substring(0, minus);
                    int n = Part1(box);

                    int found = -1;
                    for (int i = 0; i < boxes[n].Count; i++)
                    {
                        if (boxes[n][i].LensName.Equals(box))
                        {
                            found = i;
                            break;
                        }
                    }

                    if (found != -1)
                        boxes[n].RemoveAt(found);

                }
                else
                    Debug.Assert(false);
            }

            int sum = 0;
            for (int i = 0; i < boxes.Count; i++)
            {
                Console.Write($"Box {i}: ");
                for (int slot = 0; slot < boxes[i].Count; slot++)
                {
                    sum += (i + 1) * (slot + 1) * boxes[i][slot].FocalLength;
                    Console.Write($"[{boxes[i][slot].LensName} {boxes[i][slot].FocalLength}] ");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"sum is {sum}");
        }

        static void Part2File(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            Part2Multi(lines[0]);
        }


        static void Part1File(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            Part1Multi(lines[0]);
        }

        static void Part1Multi(string str)
        {
            int sum = 0;
            string[] parts = str.Split(',');
            foreach (string part in parts)
            {
                sum += Part1(part);
            }

            Console.WriteLine(sum);
        }


        static int Part1(string str)
        {
            int current = 0;
            for (int i = 0; i < str.Length; i++)
            {
                current += str[i];
                current *= 17;
                current = current % 256;
            }

            // Console.WriteLine($"result is {current}");
            return current;
        }


        static void Main(string[] args)
        {
            Console.WriteLine($"Hash test is {Part1("HASH")}");
            Part1Multi("rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7");
            Part1File("part1.txt");

            Part2Multi("rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7");
            Part2File("part1.txt");
        }
    }
}
