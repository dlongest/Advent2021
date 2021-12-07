using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Runner.Days
{
    public class Day7 : IAdventProblem
    {
        public void A()
        {
            var positions = FileSystem.MakeDataFilePath("day7")
                                      .Read()
                                      .First()
                                      .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => Int32.Parse(s))
                                      .ToArray();


            var solution = CrabSolver.Solve(positions, new DifferenceCostStrategy());
            Console.WriteLine($"Best solution is moving all crabs to {solution.Position} at fuel cost {solution.TotalFuelCost}");
        }

        public void B()
        {
            var positions = FileSystem.MakeDataFilePath("day7")
                                     .Read()
                                     .First()
                                     .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(s => Int32.Parse(s))
                                     .ToArray();


            var solution = CrabSolver.Solve(positions, new WeightedCostStrategy());
            Console.WriteLine($"Best solution is moving all crabs to {solution.Position} at fuel cost {solution.TotalFuelCost}");
        }
    }

    public static class CrabSolver
    {
        public static Solution Solve(int[] startingPositions, ICrabCostStrategy strategy)
        {            
            var min = startingPositions.Min();
            var max = startingPositions.Max();

            var median = startingPositions.Median();

            var startRange = (int)Math.Max(min, median - min);
            var endRange = (int)Math.Min(max, max - median);

            var bestSolution = new Solution { Position = Int32.MaxValue, TotalFuelCost = Int32.MaxValue };

            foreach (var position in Enumerable.Range(0, endRange - startRange + 1))
            {
                var possibleSolution = strategy.ComputeCost(startingPositions, position);

                if (bestSolution.TotalFuelCost > possibleSolution.TotalFuelCost)
                {
                    bestSolution = possibleSolution;
                }
            }

            return bestSolution;
        }
    }

    public class Solution
    {
      
        public int Position { get; set; }

        public int TotalFuelCost { get; set; }

    }

    public interface ICrabCostStrategy
    {
        Solution ComputeCost(int[] startingPositions, int targetPosition);
    }

    public class DifferenceCostStrategy : ICrabCostStrategy
    {
        public Solution ComputeCost(int[] startingPositions, int targetPosition)
        {
            // This strategy computes the fuel cost as the difference between the crab's current position
            // and the target position where equal weight is applied to each step regardless of its
            // distance from the target. 
            return new Solution()
            {
                Position = targetPosition,
                TotalFuelCost = startingPositions.Select(s => Math.Abs(s - targetPosition)).Sum()
            };
        }
    }

    public class WeightedCostStrategy : ICrabCostStrategy
    {
        public Solution ComputeCost(int[] startingPositions, int targetPosition)
        {
            // This strategy uses a higher cost for positions that are further from the target position
            // than those that are closer. The compute the cost for a given crab:
            // - Compute the distance from the crab to the target
            // - Determine the cost basis through the formula (distance + 1) / 2 as float
            // - Multiply the distance by the cost basis to equal the cost to move that crab to the target position. 
            // Now sum all those up across all the crabs to find the total cost. 

            int sum = 0;

            foreach (var position in startingPositions)
            {
                var distance = Math.Abs(position - targetPosition);
                var costBasis = (distance + 1) / 2.0;

                var cost = (int)(costBasis * distance);

                sum += cost;
            }

            return new Solution()
            {
                Position = targetPosition,
                TotalFuelCost = sum
            };
        }
    }

    public static class MathEx
    {
        public static double Median(this int[] values)
        {
            var size = values.Length;

            var sorted = values.OrderBy(s => s).ToArray();

            if (size % 2 == 0)
            {
                var midpoint = size / 2;

                return (values[midpoint] + values[midpoint - 1]) / 2.0;
            }
            else
            {
                var midpoint = (int)(size / 2);

                return values[midpoint];
            }
        }
    }
}
