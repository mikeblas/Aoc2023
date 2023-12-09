namespace Day08
{
    internal class MapNode
    {
        internal readonly string nodeName;
        internal readonly string leftTurn;
        internal readonly string rightTurn;

        internal MapNode(string nodeName, string leftTurn, string rightTurn)
        {
            this.nodeName = nodeName;
            this.leftTurn = leftTurn;
            this.rightTurn = rightTurn;
        }

        internal string NodeName { get { return nodeName; } }
        internal string LeftTurn { get {  return leftTurn; } }
        internal string RightTurn { get { return rightTurn; } } 
    }

    internal class Program
    {
        static void Part1(string fileName)
        {
            string [] lines = File.ReadAllLines(fileName);

            string turns = lines[0];

            // key is map node name; value is map node
            Dictionary<string, MapNode> theMap = new();

            for (int i = 2; i < lines.Length; i++)
            {
                string name = lines[i].Substring(0, 3);
                string left = lines[i].Substring(7, 3);
                string right = lines[i].Substring(12, 3);

                theMap[name] = new MapNode(name, left, right);
            }

            int steps = 0;
            string current = "AAA";
            int turn = 0;
            while (current != "ZZZ")
            {
                MapNode here = theMap[current];
                if (here.NodeName == "ZZZ")
                    break;

                if (turns[turn] == 'L')
                    current = here.LeftTurn;
                else
                    current = here.RightTurn;

                turn++;
                if (turn >= turns.Length)
                    turn = 0;
                steps++;
            }

            Console.WriteLine($"{fileName} took {steps}");
        }

        private static long CountSteps(Dictionary<string, MapNode> theMap, string turns, string startPlace)
        {
            long steps = 0;
            string current = startPlace;
            int turn = 0;
            while (!current.EndsWith('Z'))
            {
                MapNode here = theMap[current];
                if (here.NodeName == "ZZZ")
                    break;

                if (turns[turn] == 'L')
                    current = here.LeftTurn;
                else
                    current = here.RightTurn;

                turn++;
                if (turn >= turns.Length)
                    turn = 0;
                steps++;
            }

            return steps;
        }


        static long gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static long lcm(long a, long b)
        {
            return (a / gcf(a, b)) * b;
        }

        static long LcmOfArray(long[] arr, int idx)
        {
            // lcm(a,b) = (a*b/gcd(a,b))
            if (idx == arr.Length - 1)
            {
                return arr[idx];
            }
            long a = arr[idx];
            long b = LcmOfArray(arr, idx + 1);
            return (a * b / gcf(a, b)); // __gcd(a,b) is inbuilt library function
        }


        public static long lcm_of_array_elements(long[] element_array)
        {
            long lcm_of_array_elements = 1;
            long divisor = 2;

            while (true)
            {
                int counter = 0;
                bool divisible = false;
                for (int i = 0; i < element_array.Length; i++)
                {

                    // lcm_of_array_elements (n1, n2, ... 0) = 0.
                    // For negative number we convert into
                    // positive and calculate lcm_of_array_elements.
                    if (element_array[i] == 0)
                    {
                        return 0;
                    }
                    else if (element_array[i] < 0)
                    {
                        element_array[i] = element_array[i] * (-1);
                    }
                    if (element_array[i] == 1)
                    {
                        counter++;
                    }

                    // Divide element_array by divisor if complete
                    // division i.e. without remainder then replace
                    // number with quotient; used for find next factor
                    if (element_array[i] % divisor == 0)
                    {
                        divisible = true;
                        element_array[i] = element_array[i] / divisor;
                    }
                }

                // If divisor able to completely divide any number
                // from array multiply with lcm_of_array_elements
                // and store into lcm_of_array_elements and continue
                // to same divisor for next factor finding.
                // else increment divisor
                if (divisible)
                {
                    lcm_of_array_elements = lcm_of_array_elements * divisor;
                }
                else
                {
                    divisor++;
                }

                // Check if all element_array is 1 indicate 
                // we found all factors and terminate while loop.
                if (counter == element_array.Length)
                {
                    return lcm_of_array_elements;
                }
            }
        }



        static void Part2(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            string turns = lines[0];

            // key is map node name; value is map node
            Dictionary<string, MapNode> theMap = new();

            for (int i = 2; i < lines.Length; i++)
            {
                string name = lines[i].Substring(0, 3);
                string left = lines[i].Substring(7, 3);
                string right = lines[i].Substring(12, 3);

                theMap[name] = new MapNode(name, left, right);
            }


            // find all the places we're going to start
            List<string> currentPlaces = new();
            foreach (string place in theMap.Keys)
            {
                if (place.EndsWith('A'))
                    currentPlaces.Add(place);
            }

            // make a list of step counts
            List<long> stepsRequired = new();

            // for each place, find one of the eligible starting places and measure it
            foreach (string place in theMap.Keys)
            {
                if (place.EndsWith('A'))
                {
                    long steps = CountSteps(theMap, turns, place);
                    stepsRequired.Add(steps);
                    Console.WriteLine($"starting at {place} took {steps} steps");
                }
            }


            long result = lcm_of_array_elements(stepsRequired.ToArray());
            Console.WriteLine($"{fileName} needs {result} steps");
            long result2 = LcmOfArray(stepsRequired.ToArray(), 0);
            Console.WriteLine($"{fileName} needs {result2} steps");

            // Console.WriteLine($"{fileName} took {steps}");
        }


        static void Main(string[] args)
        {
            // Part1("sample2.txt");
            // Part1("part1.txt");

            // Part2("sample3.txt");
            Part2("part1.txt");
        }
    }
}
 