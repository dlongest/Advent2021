using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent2021.Runner.Extensions;

namespace Advent2021.Runner.Days
{
    public class Day3 : IAdventProblem
    {
        public void A()
        {
            var readings = FileSystem.MakeDataFilePath("day3")
                                     .Read(line => Bits.FromBigEndian(line));

            var gammaRate = PowerConsumption.FindGammaRate(readings);

            var powerConsumption = PowerConsumption.Compute(gammaRate);

            Console.WriteLine($"Power Consumption = {powerConsumption}");
        }

        public void B()
        {
            var readings = FileSystem.MakeDataFilePath("day3")
                                    .Read(line => Bits.FromBigEndian(line));

            var oxygenRating = PowerConsumption.FindOxygenGeneratorRating(readings).ToInt();
            var co2Rating = PowerConsumption.FindCO2ScrubberRating(readings).ToInt();

            Console.WriteLine($"Oxygen Generator Rating = {oxygenRating}");
            Console.WriteLine($"CO2 Scrubber Rating     = {co2Rating}");
            Console.WriteLine($"Life Support Rating     = {oxygenRating * co2Rating}");
        }
    }


    public static class Bits
    {
        public static int[] FromBigEndian(string bigEndian)
        {
            var reversed = bigEndian.Reverse();

            var s = string.Join(string.Empty, reversed);

            return FromLittleEndian(s);
        }

        public static int[] FromLittleEndian(string littleEndian)
        {
            var bits = littleEndian.Select(ch => ch == '1' ? 1 : 0);

            return bits.ToArray();
        }

        public static int ToInt(this int[] bits)
        {
            var powers = Enumerable.Range(0, 16).Select(n => new { Rank = n, Value = Math.Pow(2, n) })
                                                .ToDictionary(a => a.Rank, a => (int)a.Value);

            return Enumerable.Range(0, bits.Length).Select(index => powers[index] * bits[index]).Sum();
        }

        public static int[] Not(int[] bits)
        {
            return bits.Select(b => b == 1 ? 0 : 1).ToArray();
        }
    }


    public static class PowerConsumption
    {
        public static int[] FindGammaRate(IEnumerable<int[]> readings)
        {
            var bitSize = readings.First().Length;

            var gammaBits = new int[bitSize];

            foreach (var index in Enumerable.Range(0, bitSize))
            {
                gammaBits[index] = readings.MostCommonValue(index, Int32.MaxValue);
            }

            return gammaBits;
        }

        public static int[] FindOxygenGeneratorRating(IEnumerable<int[]> readings)
        {
            return readings.FindAdvancedRating((values, index, tieValue) => values.MostCommonValue(index, tieValue), 1);
        }

        public static int[] FindCO2ScrubberRating(IEnumerable<int[]> readings)
        {
            return readings.FindAdvancedRating((values, index, tieValue) => values.LeastCommonValue(index, tieValue), 0);
        }

        private static int[] FindAdvancedRating(this IEnumerable<int[]> readings, 
                                                Func<IEnumerable<int[]>, int, int, int> valueSelector, 
                                                int inCaseOfTie)
        {
            var bitSize = readings.First().Length;

            var oxygenBits = new int[bitSize];

            var remaining = readings.Skip(0);

            foreach (var index in Enumerable.Range(0, bitSize).Reverse())
            {
                // var mostCommon = remaining.MostCommonValue(index, inCaseOfTie);
                var mostCommon = valueSelector(remaining, index, inCaseOfTie);

                remaining = remaining.Where(a => a.ElementAt(index) == mostCommon);

                if (remaining.Count() == 1)
                {
                    return remaining.First();
                }
            }

            if (remaining.Count() > 1)
            {
                throw new InvalidOperationException("We have too many numbers remaining.");
            }

            return remaining.First();
        }


        public static int Compute(int[] gammaRate)
        {
            var epsilonRate = Bits.Not(gammaRate);

            return Compute(gammaRate, epsilonRate);
        }

        public static int Compute(int[] gammaRate, int[] epsilonRate)
        {
            return Bits.ToInt(gammaRate) * Bits.ToInt(epsilonRate);
        }
    }
}

namespace Advent2021.Runner.Extensions
{ 
    public static partial class ArrayExtensions
    {
        public static int MostCommonValue(this IEnumerable<int[]> values, int index, int inCaseOfTie)
        {
            var column = values.Select(v => v[index]);

            return MostCommonValue(column, inCaseOfTie);
        }

        public static int MostCommonValue(this IEnumerable<int> values, int inCaseOfTie)
        {
            return values.ToArray().MostCommonValue(inCaseOfTie);
        }

        public static int MostCommonValue(this int[] values, int inCaseOfTie)
        {
            var grouped = values.GroupBy(v => v);

            if (grouped.Count() == 1)
            {
                return grouped.First().Key;
            }

            var summed = grouped.Select(g => new { Value = g.Key, Count = g.Count() })
                                 .OrderByDescending(a => a.Count);

            if (summed.ElementAt(0).Count == summed.ElementAt(1).Count)
            {
                return inCaseOfTie;
            }

            return summed.First().Value;
        }

        public static int LeastCommonValue(this IEnumerable<int[]> values, int index, int inCaseOfTie)
        {
            var column = values.Select(v => v[index]).ToArray();

            return LeastCommonValue(column, inCaseOfTie);
        }

        public static int LeastCommonValue(this int[] values, int inCaseOfTie)
        {
            var grouped = values.GroupBy(v => v);

            if (grouped.Count() == 1)
            {
                return grouped.First().Key;
            }

            var summed = grouped.Select(g => new { Value = g.Key, Count = g.Count() })
                                .OrderBy(a => a.Count);

            if (summed.ElementAt(0).Count == summed.ElementAt(1).Count)
            {
                return inCaseOfTie;
            }

            return summed.First().Value;
        }
    }
}
