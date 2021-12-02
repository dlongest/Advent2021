using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent2021.Runner.Extensions;

namespace Advent2021.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var days = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                             .Where(t => typeof(IAdventProblem).IsAssignableFrom(t) && t.IsClass);

            PrintDaysAvailable(days);

            Console.Write("What problem do you want to run?    ");
            var input = Console.ReadLine();

            Console.WriteLine($"Running Day {input}");

            new DaySelector().Select(days, input)();

            PrintFooter();
        }

        private static void PrintDaysAvailable(IEnumerable<Type> adventProblems)
        {
            Console.WriteLine("Days\n===============================");

            var dayNames = adventProblems.Select(ap => ap.Name).OrderBy(ap => ap.Length).ThenBy(ap => ap);

            dayNames.ToList().ForEach(d => Console.WriteLine(d));
        }
        

        private static void PrintFooter()
        {
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
                             
        private class DaySelector
        {
            public Action Select(IEnumerable<Type> days, string toRun)
            {               
                var day = GetDay(toRun);
                var part = GetPart(toRun).ToUpper();

                var targetTypeName = "Day" + day;

                var type = days.FirstOrDefault(d => d.Name.Equals(targetTypeName));

                if (type == null)
                {
                    throw new ArgumentException($"Cannot find an IAdventProblem type for input '{toRun}'");
                }

                var adventProblem = Activator.CreateInstance(type) as IAdventProblem;

                return CreateMethodCall(adventProblem, part);
            }

            private string GetDay(string toRun)
            {
                var regex = new System.Text.RegularExpressions.Regex(@"[1-2]?[0-9]");

                if (!regex.IsMatch(toRun))
                {
                    throw new ArgumentException($"Unable to find a day based on the input '${toRun}'");
                }

                var day = regex.Match(toRun).Value;

                return day;
            }

            private string GetPart(string toRun)
            {
                return toRun.Right(1);
            }

            private Action CreateMethodCall(IAdventProblem problem, string part)
            {
                if (part.Equals("A"))
                {
                    return () => problem.A();
                }

                if (part.Equals("B"))
                {
                    return () => problem.B();
                }

                throw new ArgumentException($"Could not create method call expression for part '{part}'");
            }
        }
    }
}
