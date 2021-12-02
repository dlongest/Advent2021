using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Advent2021.Runner.Extensions;

namespace Advent2021.Tests
{
    public class ArrayExtensionsTests
    {
        [Theory]
        [ClassData(typeof(PairTestCases))]
        public void Pair_CreatesCorrectPairs(int[] values, IEnumerable<Tuple<int, int>> expected)
        {
            var actual = values.Pair();

            Assert.Equal(expected, actual);
        }

        private class PairTestCases : IEnumerable<object[]>
        {
            private List<object[]> data = new List<object[]>();

            public PairTestCases()
            {
                this.data.Add(new object[] { new int[] { 1 }, new Tuple<int, int>[0] { } });
                this.data.Add(new object[] { new int[] { 1, 2 }, new Tuple<int, int>[] { Tuple.Create(1, 2) } });

                this.data.Add(new object[] { new int[] { 1, 2, 3, 4, 5 },
                                             new Tuple<int, int>[] { Tuple.Create(1, 2), Tuple.Create(2, 3),
                                                                     Tuple.Create(3, 4), Tuple.Create(4, 5) } });                             
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                return this.data.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        [Theory]
        [ClassData(typeof(WindowTestCases))]
        public void Window_CreatesCorrectGroups(int[] values, int windowSize, IEnumerable<int[]> expected)
        {
            var actual = values.Window(windowSize);

            Assert.Equal(expected, actual);
        }

        private class WindowTestCases : IEnumerable<object[]>
        {
            private List<object[]> data = new List<object[]>();

            public WindowTestCases()
            {
                this.data.Add(new object[] { new int[] { 1, 2 }, 1, new List<int[]> { new int[] { 1 }, new int[] { 2 } } });
                this.data.Add(new object[] { new int[] { 1, 2 }, 2, new List<int[]> { new int[] { 1, 2 } } });

                this.data.Add(new object[] { new int[] { 1, 2, 3, 4, 5, 6 }, 3,
                                              new List<int[]>
                                              { new int[] { 1, 2, 3 }, new int[] { 2, 3, 4 },
                                                new int[] { 3, 4, 5 }, new int[] { 4, 5, 6} }
                });
                                             
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                return this.data.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
