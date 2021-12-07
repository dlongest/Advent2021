using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Runner.Days
{
    public class Day6 : IAdventProblem
    {
        public void A()
        {
            var startingFishTimings = FileSystem.MakeDataFilePath("day6")
                                                .Read()
                                                .First()
                                                .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(s => Int32.Parse(s))
                                                .ToArray();

            var totalFish = FishSimulation.Run(startingFishTimings, 80);

            Console.WriteLine($"We've got {totalFish} fish");
        }

        public void B()
        {
            var startingFishTimings = FileSystem.MakeDataFilePath("day6")
                                                .Read()
                                                .First()
                                                .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(s => Int32.Parse(s))
                                                .ToArray();

            var totalFish = FishSimulation.Run(startingFishTimings, 256);

            Console.WriteLine($"We've got {totalFish} fish");
        }
    }

    public static class FishSimulation
    {
        public static long Run(int[] startingFish, int steps)
        {
            return Run(startingFish, steps, () => { });
        }

        public static long Run(int[] startingFish, int steps, Action afterIteration)
        {
            var fishCohorts = AsCohort(startingFish);

            foreach (var step in Enumerable.Range(0, steps))
            {
                fishCohorts = Iterate(fishCohorts);
                afterIteration();
            }

            return fishCohorts.Sum(kvp => kvp.Value);
        }

        public static Dictionary<int, long> AsCohort(int[] fish)
        {
            var startingCohorts = fish.GroupBy(f => f).ToDictionary(g => g.Key, g => g.Count());

            var fishCohorts = Enumerable.Range(0, 9)
                                      .ToDictionary(i => i,
                                                    i => startingCohorts.ContainsKey(i) ? startingCohorts[i] : 0L);

            return fishCohorts;
        }

        public static Dictionary<int, long> Iterate(int[] fish)
        {           
            return Iterate(AsCohort(fish));
        }
            
        public static Dictionary<int, long> Iterate(Dictionary<int, long> fishCohorts)
        {
            var next = Enumerable.Range(0, 9).ToDictionary(i => i, _ => 0L);

            foreach (var kvp in fishCohorts.OrderByDescending(kvp => kvp.Key))
            {
                if (kvp.Key > 0)
                {
                    next[kvp.Key - 1] = fishCohorts[kvp.Key];
                }
                else
                {
                    // First, the current fish cohort resets at 6 so add the # of fish in this cohort
                    // to whatever fish we've accumulated into the 6 cohort previously this iteration. 
                    next[6] = next[6] + fishCohorts[kvp.Key];

                    // Each fish in this cohort spaws a new fish, 1:1, into the 8 cohort, which is 
                    // otherwise empty so set its value. 
                    next[8] = fishCohorts[kvp.Key];
                }
            }

            return next;
        }
    }
}
