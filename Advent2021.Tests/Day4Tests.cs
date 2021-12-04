using Advent2021.Runner.Days;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Advent2021.Tests
{
    public class Day4Tests
    {
        [Fact]
        public void BingoCard_ConstructedCorrectly()
        {
            var card = new List<int[]>()
            {
                new int[] { 22, 13, 17, 11, 0 }, new int[] { 8, 2, 23, 4, 24 }, new int[] { 21, 9, 14, 16, 7 },
                new int[] { 6, 10, 3, 18, 5 }, new int[] { 1, 12, 20, 15, 19 }
            };

            var sut = new BingoCard(1, card);

            Assert.Equal(RowColumnIndex.New(0, 0), sut.WhereIs(22));
            Assert.Equal(RowColumnIndex.New(1, 1), sut.WhereIs(2));
            Assert.Equal(RowColumnIndex.New(2, 2), sut.WhereIs(14));
            Assert.Equal(RowColumnIndex.New(3, 3), sut.WhereIs(18));
            Assert.Equal(RowColumnIndex.New(4, 4), sut.WhereIs(19));

            Assert.Equal(RowColumnIndex.New(0, 1), sut.WhereIs(13));
            Assert.Equal(RowColumnIndex.New(0, 2), sut.WhereIs(17));
            Assert.Equal(RowColumnIndex.New(0, 3), sut.WhereIs(11));
            Assert.Equal(RowColumnIndex.New(0, 4), sut.WhereIs(0));

            Assert.Equal(RowColumnIndex.New(1, 0), sut.WhereIs(8));
            Assert.Equal(RowColumnIndex.New(1, 2), sut.WhereIs(23));
            Assert.Equal(RowColumnIndex.New(1, 3), sut.WhereIs(4));
            Assert.Equal(RowColumnIndex.New(1, 4), sut.WhereIs(24));

            Assert.Equal(RowColumnIndex.New(2, 0), sut.WhereIs(21));
            Assert.Equal(RowColumnIndex.New(2, 1), sut.WhereIs(9));
            Assert.Equal(RowColumnIndex.New(2, 3), sut.WhereIs(16));
            Assert.Equal(RowColumnIndex.New(2, 4), sut.WhereIs(7));

            Assert.Equal(RowColumnIndex.New(3, 0), sut.WhereIs(6));
            Assert.Equal(RowColumnIndex.New(3, 1), sut.WhereIs(10));
            Assert.Equal(RowColumnIndex.New(3, 2), sut.WhereIs(3));
            Assert.Equal(RowColumnIndex.New(3, 4), sut.WhereIs(5));

            Assert.Equal(RowColumnIndex.New(4, 0), sut.WhereIs(1));
            Assert.Equal(RowColumnIndex.New(4, 1), sut.WhereIs(12));
            Assert.Equal(RowColumnIndex.New(4, 2), sut.WhereIs(20));
            Assert.Equal(RowColumnIndex.New(4, 3), sut.WhereIs(15));
        }

        [Fact]
        public void BingoCard_Mark_FindsBingoCorrectlyOnRow()
        {
            var expected = true;

            var card = new List<int[]>()
            {
                new int[] { 22, 13, 17, 11, 0 }, new int[] { 8, 2, 23, 4, 24 }, new int[] { 21, 9, 14, 16, 7 },
                new int[] { 6, 10, 3, 18, 5 }, new int[] { 1, 12, 20, 15, 19 }
            };

            var sut = new BingoCard(1, card);

            sut.Mark(0); sut.Mark(11); sut.Mark(17); sut.Mark(13);
            var actual = sut.Mark(22);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BingoCard_Mark_FindsBingoCorrectlyOnColumn()
        {
            var expected = true;

            var card = new List<int[]>()
            {
                new int[] { 22, 13, 17, 11, 0 }, new int[] { 8, 2, 23, 4, 24 }, new int[] { 21, 9, 14, 16, 7 },
                new int[] { 6, 10, 3, 18, 5 }, new int[] { 1, 12, 20, 15, 19 }
            };

            var sut = new BingoCard(1, card);

            sut.Mark(17); sut.Mark(23); sut.Mark(3); sut.Mark(14);
            var actual = sut.Mark(20);

            Assert.Equal(expected, actual);
        }
    }
}
