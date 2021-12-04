using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Runner.Extensions
{
    public static partial class ArrayExtensions
    {

        public static IEnumerable<Tuple<T, T>> Pair<T>(this IEnumerable<T> values)
        {
            var count = values.Count();

            return Enumerable.Range(0, count - 1)
                             .Select(i => Tuple.Create<T, T>(values.ElementAt(i), values.ElementAt(i + 1)));

        }

        public static IEnumerable<T[]> Window<T>(this IEnumerable<T> values, int windowSize)
        {
            var ar = values.ToArray();

            return Enumerable.Range(0, ar.Length - windowSize + 1)
                             .Select(i => ar.TakeFrom(i, windowSize));
        }

        /// <summary>
        /// For the given array ar, takes count items beginning at startIndex and returns that subset
        /// as a new array. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] TakeFrom<T>(this T[] ar, int startIndex, int count)
        {
            var subset = new List<T>();

            for (int offset = 0; offset < count; ++offset)
            {
                subset.Add(ar[startIndex + offset]);
            }
            
            return subset.ToArray();
        }

        public static T[] Slice<T>(this IEnumerable<T[]> ar, int columnIndex)
        {
            return ar.Select(v => v[columnIndex]).ToArray();
        }
    }
}
