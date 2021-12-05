using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Runner.Days
{
    public class Day5 : IAdventProblem
    {
        public void A()
        {

            var vents = FileSystem.MakeDataFilePath("day5")
                                   .Read(line => new { Valid = ThermalVent.IsDiagonal(line), Line = line })
                                   .Where(line => line.Valid)
                                   .Select(line => ThermalVent.From(line.Line))
                                   .ToArray();

            var overlaps = ThermalVent.PartialOverlap(vents);

            Console.WriteLine($"Overlaps = {overlaps.Count()}");
        }

        public void B()
        {
            var vents = FileSystem.MakeDataFilePath("day5")
                                  .Read(line => ThermalVent.From(line))
                                  .ToArray();

            var overlaps = ThermalVent.PartialOverlap(vents);

            Console.WriteLine($"Overlaps = {overlaps.Count()}");
        }
    }


    public class ThermalVent
    {
        public ThermalVent(XY start, XY end)
        {
            this.Start = start;
            this.End = end;
        }


        public static ThermalVent From(string position)
        {        
            var (start, end) = Parse(position);
            return new ThermalVent(start, end);
        }

        public XY Start { get; private set; }

        public XY End { get; private set; }


        public HashSet<XY> Range()
        {
            return new HashSet<XY>(XY.Range(this.Start, this.End));
        }


        /// <summary>
        /// Returns the points across the provided vent covered by every vent. 
        /// </summary>
        /// <param name="vents"></param>
        /// <returns></returns>
        public static HashSet<XY> CompleteOverlap(IEnumerable<ThermalVent> vents)
        {
            var overlapped = vents.First().Range();

            if (vents.Count() == 1)
            {
                return overlapped;
            }

            foreach (var vent in vents.Skip(1))
            {
                overlapped.IntersectWith(vent.Range());
            }

            return overlapped;
        }

        /// <summary>
        /// Returns the points across vents covered by at least 2 vents. 
        /// </summary>
        /// <param name="vents"></param>
        /// <returns></returns>
        public static HashSet<XY> PartialOverlap(IEnumerable<ThermalVent> vents)
        {
            if (vents.Count() == 1)
            {
                return vents.First().Range();
            }

            var overlapped = vents.First().Range().ToDictionary(k => k, v => 1);

            foreach (var vent in vents.Skip(1))
            {
                foreach (var xy in vent.Range())
                {
                    if (!overlapped.ContainsKey(xy))
                    {
                        overlapped.Add(xy, 0);
                    }

                    overlapped[xy]++;
                }               
            }

            return new HashSet<XY>(overlapped.Where(o => o.Value > 1).Select(o => o.Key));
        }


        public static bool IsDiagonal(string position)
        {
            var (start, end) = Parse(position);

            return !XY.IsDiagonal(start, end);
        }     

        private static Tuple<XY, XY> Parse(string position)
        {
            var split = position.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);

            return Tuple.Create(XY.From(split[0]), XY.From(split[1]));
        }
    }

    public class XY
    {

        public static XY From(string commaSeparatedXy)
        {
            var parts = commaSeparatedXy.Split(new string[] { "," }, StringSplitOptions.None)
                                        .Select(s => Int32.Parse(s)).ToArray();

            return XY.New(parts[0], parts[1]);
        }

        public static XY New(int x, int y)
        {
            return new XY(x, y);
        }

        public XY(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public double Slope(XY other)
        {
            return ((this.Y - other.Y) / (this.X - other.X));
        }

        public static IEnumerable<XY> Range(XY start, XY end)
        {
            if (IsDiagonal(start, end))
            {
                // We can only handle 45 degree diagonals. There are two types of those. One 
                // is positive slope (with respect to our coordinate grid) going from top left to bottom right. 
                if (start.Slope(end) == 1)
                {
                    var units = Math.Abs(end.X - start.X) + 1;

                    // Pick the smallest X
                    var startX = start.X <= end.X ? start.X : end.X;
                   
                    // The smallest X is paired with the smallest Y
                    var startY = start.Y <= end.Y ? start.Y : end.Y;
                   
                    // Start at our starting point and generate the points by adding 1 to X and Y position until 
                    // you've done that units times. 
                    return Enumerable.Range(0, units).Select(offset => XY.New(startX + offset, startY + offset));                   
                }

                // The second type of slope is "negative" slope (again, with respect to our coordinate plane), which
                // moves from lower left to upper right. 
                if (start.Slope(end) == -1)
                {
                    // How long will the line be?
                    var units = Math.Abs(end.X - start.X) + 1;

                    // Start at the smallest X
                    var startX = start.X <= end.X ? start.X : end.X;

                    // The smallest X will be paired with the greatest Y so choose that
                    var startY = start.Y >= end.Y ? start.Y : end.Y;

                    // Each of our points is (starting X + offset, starting Y - offset)
                    return Enumerable.Range(0, units).Select(offset => XY.New(startX + offset, startY - offset));
                }

                throw new InvalidOperationException("Cannot do a diagonal that isn't 45 degrees");
            }

            if (IsHorizontal(start, end))
            {
                // Y is the same for every point in the range
                var y = start.Y;

                // Start with the lower X, end on the higher X
                var startX = start.X <= end.X ? start.X : end.X;
                var endX = end.X >= start.X ? end.X : start.X;

                return Enumerable.Range(startX, endX - startX + 1).Select(x => XY.New(x, y));
            }

            if (IsVertical(start, end))
            {
                // X is the same for every point in the range
                var x = start.X;

                // Start with the lower Y, end on the higher Y
                var startY = start.Y <= end.Y ? start.Y : end.Y;
                var endY = end.Y >= start.Y ? end.Y : start.Y;

                return Enumerable.Range(startY, endY - startY + 1).Select(y => XY.New(x, y));
            }

            throw new InvalidOperationException("No idea how we got here");
        }

        public static bool IsDiagonal(XY one, XY two)
        {
            return !(one.X == two.X || one.Y == two.Y);
        }

        public static bool IsHorizontal(XY one, XY two)
        {
            return one.Y == two.Y;
        }

        public static bool IsVertical(XY one, XY two)
        {
            return one.X == two.X;
        }

        public IEnumerable<XY> HorizontalLine(int x, int startY, int endY)
        {
            foreach (var y in Enumerable.Range(startY, endY - startY))
            {
                yield return XY.New(x, y);
            }
        }

        public IEnumerable<XY> VerticalLine(int y, int startX, int endX)
        {
            foreach (var x in Enumerable.Range(startX, endX - startX))
            {
                yield return XY.New(x, y);
            }
        }

        public override bool Equals(object obj)
        {
            var o = obj as XY;

            if (o == null)
            {
                return false;
            }

            return o.X == this.X && o.Y == this.Y;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + 17 * this.Y.GetHashCode();
        }

        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }
    }
}
