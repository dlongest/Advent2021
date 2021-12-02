using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent2021.Runner.Extensions;

namespace Advent2021.Runner.Days
{
    public class Day1 : IAdventProblem
    {
        public void A()            
        {
            var increases = FileSystem.MakeDataFilePath("day1")
                                      .Read(line => Int32.Parse(line))
                                      .Pair()
                                      .Where(t => t.Item1 < t.Item2)
                                      .Count();

            Console.WriteLine($"{increases} increases occurred between pair-wise values");
        }

        public void B()
        {
            var manager = new GroupManager();

            var increases = FileSystem.MakeDataFilePath("day1")
                                   .Read(line => Int32.Parse(line))
                                   .Window(3)
                                   .Select(w => w.Sum())
                                   .Pair()
                                   .Where(t => t.Item1 < t.Item2)
                                   .Count();

            Console.WriteLine($"{increases} increases occurred between pair-wise three measurement groups");
        }
    }


    public class GroupManager
    {
        private readonly GroupNameGenerator nameGenerator = new GroupNameGenerator();
        private List<Group> completeGroups = new List<Group>();

        private Queue<Group> workingGroups = new Queue<Group>();

        private const int maximumGroupSize = 3;
        private const int maximumActiveGroups = 3;

        public void Add(int item)
        {
            // If we don't have a full complement of working groups, add one
            if (this.workingGroups.Count < maximumActiveGroups)
            {
                this.workingGroups.Enqueue(Group.New(this.nameGenerator.Name()));            
            }

            // Add the new item to all of the working groups
            foreach (var group in this.workingGroups)
            {
                group.Add(item);
            }

            // If the first group is full, pop it and add it to the complete groups
            if (this.workingGroups.Peek().Count == maximumGroupSize)
            {
                var completed = workingGroups.Dequeue();

                this.completeGroups.Add(completed);
            }
        }                 
        public IEnumerable<Group> Groups { get { return this.completeGroups; } }        

        public void Print()
        {
            foreach (var group in this.completeGroups)
            {
                Console.WriteLine($"{group.Name}: {group.Sum()}");
            }
        }
    }


    public class GroupNameGenerator
    {
        private int seed = 1;
        private char startChar = 'A';
        private char endChar = 'Z';
        private char lastGeneratedChar = '\0';
        private string baseString = string.Empty;

        public string Name()
        {
            if (lastGeneratedChar > endChar)
            {
                baseString = string.Join("", new object[] { baseString, startChar });
                lastGeneratedChar = startChar;
            }

            return "";
        }
    }
   

    public class Group
    {
        private IList<int> items;

        public static Group New(string name)
        {
            return new Group(name);
        }

        public Group(string name)
        {
            this.Name = name;
            this.items = new List<int>();
        }

        public void Add(int item)
        {
            this.items.Add(item);
        }

        public int Sum()
        {
            return this.items.Sum();
        }

        public int Count {  get { return this.items.Count; } }

        public static bool operator <(Group first, Group second) => first.Sum() < second.Sum();

        public static bool operator >(Group first, Group second) => first.Sum() > second.Sum();

        public string Name { get; private set; }
    }
}
