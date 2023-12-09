using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace Day05
{

    class Translation
    {
        long destStart;
        long sourceStart;
        internal long rangeLength;

        internal Translation(long destStart, long sourceStart, long rangeLength)
        {
            this.destStart = destStart;
            this.sourceStart = sourceStart;
            this.rangeLength = rangeLength;
        }

        internal long SourceStartInclusive { get { return sourceStart; } }

        internal long SourceEndInclusive { get { return sourceStart + rangeLength - 1; } } 

        internal long Offset {  get { return destStart - sourceStart; } }       

        public override string ToString()
        {
            return $"{SourceStartInclusive} -> {SourceEndInclusive} ({rangeLength}), {Offset}";
        }
    }

    class TranslationTable
    {
        internal List<Translation> table;

        internal TranslationTable()
        {
            table = new();
        }

        internal void Add(Translation translation)
        {
            table.Add(translation);
        }

        internal long Translate(long n)
        {
            long n2 = n;

            foreach(Translation translation in table)
            {
                if (n2 >= translation.SourceStartInclusive && translation.SourceEndInclusive >= n2)
                {
                    n2 += translation.Offset;
                    break;
                }
            }

            // Console.WriteLine($"   {n} --> {n2}");
            return n2;
        }

        internal SplittableRangedSeed TranslateRangedSeed(SplittableRangedSeed srs)
        {
            List<RangedSeed> worker = srs.ToList();

            // for each seed ...
            //      apply each translation
            //      if a translation hits, move the translated seed out of the worklist
            //      if a translation splits, only the new seeds are added to the worklist

            List<RangedSeed> toAdd = new();
            for (int i = 0; i < worker.Count; i++)
            {
                foreach (Translation translation in table)
                {
                    // if translation is outside this seed, continue
                    // starts too high?
                    // t = 12345
                    // w =      678910
                    if (worker[i].SeedStartInclusive > translation.SourceEndInclusive)
                        continue;

                    // ends too low?
                    // t =      678910
                    // w = 12345
                    if (worker[i].SeedEndInclusive < translation.SourceStartInclusive)
                        continue;

                    // split: adjust this range
                    // if new ranges are created, add to toAdd

                    // Console.WriteLine("translating:");
                    // Console.WriteLine($"   seedling    {worker[i]}");
                    // Console.WriteLine($"   translation {translation}");

                    if (worker[i].SeedStartInclusive >= translation.SourceStartInclusive && translation.SourceEndInclusive >= worker[i].SeedEndInclusive)
                    {
                        // t = 123456789
                        // s =    456
                        // this translation overlaps this seed completely
                        // create a new seed translated
                        long newSeedStart = worker[i].SeedStartInclusive + translation.Offset;
                        long newSeedLength = worker[i].RangeLength;

                        toAdd.Add(new RangedSeed(newSeedStart, newSeedLength));

                        // and this one is gone
                        worker[i].RangeLength = 0;
                    }
                    else if (translation.SourceStartInclusive >= worker[i].SeedStartInclusive && worker[i].SeedEndInclusive >= translation.SourceEndInclusive)
                    {
                        // t =    456
                        // s = 123456789
                        // this seed overlaps the translation completely
                        // we get a translated seed

                        long newSeedStart = translation.SourceStartInclusive + translation.Offset;
                        long newSeedLength = translation.rangeLength;
                        toAdd.Add(new RangedSeed(newSeedStart, newSeedLength));

                        // the back is a new seed to work on
                        long lowerSeedStart = translation.SourceEndInclusive + 1;
                        long lowerSeedLength = worker[i].SeedEndInclusive - translation.SourceEndInclusive;
                        worker.Add(new RangedSeed(lowerSeedStart, lowerSeedLength));

                        // and we modify the front in place
                        worker[i].RangeLength = translation.SourceStartInclusive - worker[i].SeedStartInclusive;
                    }
                    else if (worker[i].SeedEndInclusive >= translation.SourceStartInclusive && worker[i].SeedStartInclusive < translation.SourceStartInclusive)
                    {
                        // t =   34567
                        // w = 12345
                        // this translation overlaps the end of this seed
                        long changedSeedStart = worker[i].SeedStartInclusive;
                        long changedSeedLength = translation.SourceStartInclusive - worker[i].SeedStartInclusive;

                        // the new seed is translated and added
                        long newSeedStart = translation.SourceStartInclusive + translation.Offset;
                        long newSeedLength = worker[i].SeedEndInclusive - translation.SourceStartInclusive + 1;
                        toAdd.Add(new RangedSeed(newSeedStart, newSeedLength));

                        // the existing seed is changed to be shorter
                        worker[i].RangeLength = changedSeedLength;
                        worker[i].SeedStartInclusive = changedSeedStart;

                    }
                    else if (worker[i].SeedStartInclusive >= translation.SourceStartInclusive && worker[i].SeedEndInclusive > translation.SourceEndInclusive)
                    {
                        // t = 123456
                        // w =    456789
                        // this translation overlaps the beginning of this seed
                        long changedSeedStart = translation.SourceEndInclusive + 1;
                        long changedSeedLength = worker[i].SeedEndInclusive - translation.SourceEndInclusive;

                        // the new seed is translated and added
                        long newSeedStart = worker[i].SeedStartInclusive + translation.Offset;
                        long newSeedLength = translation.SourceEndInclusive - worker[i].SeedStartInclusive + 1;
                        toAdd.Add(new RangedSeed(newSeedStart, newSeedLength));

                        // the existing seed is changed to start at a later and shorter range
                        worker[i].SeedStartInclusive = changedSeedStart;
                        worker[i].RangeLength = changedSeedLength;
                    }
                    else
                    {
                        throw new Exception($"Internal error: ");
                    }
                }
            }

            worker.AddRange(toAdd);
            return new SplittableRangedSeed(worker);
        }

    }

    class RangedSeed
    {
        long seedStart;
        long rangeLength;

        internal RangedSeed(long seedStart, long rangeLength)
        {
            this.seedStart = seedStart;
            this.rangeLength = rangeLength;
        }

        internal long SeedStartInclusive { get { return seedStart; } set { seedStart = value; } }

        internal long SeedEndInclusive { get { return seedStart + rangeLength - 1; } set { rangeLength = value; } }

        internal long RangeLength
        {
            get => rangeLength;
            set
            {
                if (value < 0)
                    throw new Exception($"RangeLength can't be negative: {value}");
                rangeLength = value;
            }
        }

        public override string ToString()
        {
            return $"{SeedStartInclusive} -> {SeedEndInclusive} ({rangeLength})";
        }
    }

    class SplittableRangedSeed
    {
        internal List<RangedSeed> list;

        internal SplittableRangedSeed(RangedSeed rs)
        {
            list = new();
            list.Add(rs);
        }

        internal SplittableRangedSeed(List<RangedSeed> rsl)
        {
            list = new();
            foreach (RangedSeed r in rsl)
            {
                if (r.RangeLength > 0)
                    list.Add(r);

                if (r.RangeLength < 0)
                    throw new Exception($"Range in RangedSeed is negative {r.RangeLength}");
            }
        }

        internal RangedSeed this[Index i] { get { return list[i]; } }

        internal int Ranges
        {
            get { return list.Count; }
        }

        internal List<RangedSeed> ToList()
        {
            return new(list);
        }

        internal long MinLocation()
        {
            long minLocation = long.MaxValue;
            foreach (RangedSeed rs in list)
            {
                if (rs.RangeLength > 0 && rs.SeedStartInclusive < minLocation)
                    minLocation = rs.SeedStartInclusive;
            }

            return minLocation;
        }

        internal long SeedCount
        {
            get
            {
                long c = 0;
                foreach (RangedSeed rs in list)
                    c += rs.RangeLength;

                return c;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (var s in list)
            {
                sb.Append($"   {s}");
            }

            return sb.ToString();
        }
    }

    internal class Program
    {
        static void Part1(string fileName)
        {
            List<TranslationTable> tables = new();
            tables.Add(new TranslationTable());
            List<long> seeds = new();

            string[] lines = File.ReadAllLines(fileName);
            bool skip = false;

            foreach (string line in lines)
            {
                if (line.StartsWith("seeds:"))
                {
                    skip = true;
                    string[] splitSeeds = line.Substring(line.IndexOf(':')+1).Trim().Split(' ');

                    foreach (string s in splitSeeds)
                    {
                        long sn = long.Parse(s);
                        seeds.Add(sn);
                        // Console.WriteLine($"seed {sn}");
                    }
                }
                else if (line.Length == 0)
                {
                    if (skip == true)
                        skip = false;
                    else
                        tables.Add(new TranslationTable());
                }
                else if (line.EndsWith(':'))
                {
                    // Console.WriteLine(line);
                }
                else
                {
                    string[] splitStrings = line.Split(' ');

                    long dest = long.Parse(splitStrings[0]);
                    long source = long.Parse(splitStrings[1]);
                    long range = long.Parse(splitStrings[2]);

                    // Console.WriteLine($"{dest}, {source}, {range}");

                    Translation t = new(dest, source, range);
                    tables.Last().Add(t);
                }
            }

            // now, do the translations
            long minLocation = long.MaxValue;
            foreach(long seed in  seeds)
            {
                long s2 = seed;
                foreach(TranslationTable t in tables)
                {
                    s2 = t.Translate(s2);
                }

                // Console.WriteLine($"{seed} ==> {s2}");

                if (s2 < minLocation)
                    minLocation = s2;
            }
            // Console.WriteLine($"{fileName} min location is {minLocation}");
        }


        static void Part2(string fileName)
        {
            List<TranslationTable> tables = new();
            tables.Add(new TranslationTable());
            List<RangedSeed> seeds = new();

            string[] lines = File.ReadAllLines(fileName);
            bool skip = false;

            foreach (string line in lines)
            {
                if (line.StartsWith("seeds:"))
                {
                    skip = true;
                    string[] splitSeeds = line.Substring(line.IndexOf(':') + 1).Trim().Split(' ');

                    for (int i = 0; i < splitSeeds.Length; i+=2)
                    {
                        long start = long.Parse(splitSeeds[i]);
                        long len = long.Parse(splitSeeds[i + 1]);
                        seeds.Add(new RangedSeed(start, len));
                    }
                }
                else if (line.Length == 0)
                {
                    if (skip == true)
                        skip = false;
                    else
                        tables.Add(new TranslationTable());
                }
                else if (line.EndsWith(':'))
                {
                    // Console.WriteLine(line);
                }
                else
                {
                    string[] splitStrings = line.Split(' ');

                    long dest = long.Parse(splitStrings[0]);
                    long source = long.Parse(splitStrings[1]);
                    long range = long.Parse(splitStrings[2]);

                    // Console.WriteLine($"{dest}, {source}, {range}");

                    Translation t = new(dest, source, range);
                    // Console.WriteLine(t);
                    tables.Last().Add(t);
                }
            }

            // now, do the translations
            long minLocation = long.MaxValue;
            foreach (RangedSeed rs in seeds)
            {
                SplittableRangedSeed s2 = new(rs);
                long startCount = s2.SeedCount;

                // Console.WriteLine($"working {s2}");
                foreach (TranslationTable t in tables)
                {
                    s2 = t.TranslateRangedSeed(s2);
                    // Console.WriteLine($"result is {s2}");
                }


                long ml = s2.MinLocation();
                if (ml < minLocation)
                    minLocation = ml;

                long endCount = s2.SeedCount;

                // Console.WriteLine($"===== minimum is now {minLocation}; {startCount}, {endCount}");
            }
            Console.WriteLine($"{fileName} min location is {minLocation}");
        }


        static void Main(string[] args)
        {
            // Part1("sample1.txt");
            // Part1("part1.txt");
            // Part2("sample1.txt");

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Part2("part1.txt");
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
