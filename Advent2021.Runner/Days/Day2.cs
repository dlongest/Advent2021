using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Runner.Days
{
    public class Day2 : IAdventProblem
    {
        public void A()
        {
            var sub = new Submarine();

            FileSystem.MakeDataFilePath("day2")
                      .Read(line => sub = sub.ApplyCommand(line))
                      .ToArray();

            var depth = sub.Depth;
            var h = sub.HorizontalPosition;

            Console.WriteLine($"\nDepth {depth} x H-Position {h} = {depth * h}");
        }

        public void B()
        {
            var sub = new Submarine();

            FileSystem.MakeDataFilePath("day2")
                      .Read(line => sub = sub.ApplyCommand(line))
                      .ToArray();

            var depth = sub.Depth;
            var h = sub.HorizontalPosition;

            Console.WriteLine($"\nDepth {depth} x H-Position {h} = {depth * h}");
        }
    }

    public class Submarine
    {
        private int horizontalPosition = 0;
        private int depth = 0;
        private int aim;

        public void Forward(int distance)
        {
            this.horizontalPosition += distance;

            var depthChange = this.aim * distance;

            this.depth += depthChange;
        }

        public void Down(int distance)
        {
            distance = Math.Abs(distance);

            this.aim += distance;
        }

        public void Up(int distance)
        {
            distance = Math.Abs(distance);

            this.aim -= distance;
        }

        public int Depth { get { return this.depth;  } }

        public int HorizontalPosition {  get { return this.horizontalPosition; } }

        public Submarine ApplyCommand(string command)
        {
            var commandParts = command.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var instruction = commandParts[0];
            var magnitude = Int32.Parse(commandParts[1]);
            
            if (instruction == "forward")
            {
                this.Forward(magnitude);
            }
            else if (instruction == "up")
            {
                this.Up(magnitude);
            }
            else if (instruction == "down")
            {
                this.Down(magnitude);
            }
            else
            {
                throw new ArgumentException($"Unrecognized command: {command}");
            }

            return this;
        }
    }
}
