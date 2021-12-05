using Advent2021.Runner.Days;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Advent2021.Tests
{
    public class Day5Tests
    {

        [Fact]
        public void XY_Range_IsCorrect_For_HorizontalRange()
        {
            var expected = new XY[] { XY.New(0, 0), XY.New(0, 1), XY.New(0, 2), XY.New(0, 3) };

            var actual = XY.Range(XY.New(0, 0), XY.New(0, 3));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void XY_Range_IsCorrect_For_VerticalRange()
        {
            var expected = new XY[] { XY.New(2, 2), XY.New(3, 2), XY.New(4, 2), XY.New(5, 2), XY.New(6, 2) };

            var actual = XY.Range(XY.New(2, 2), XY.New(6, 2));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThermalVent_Range_ProducesSetOfXYs()
        {
            var expected = new HashSet<XY>(new[] { XY.New(1,1), XY.New(1, 2), XY.New(1,3), XY.New(1, 4),
                                                    XY.New(1, 5), XY.New(1,6), XY.New(1, 7) });

            var sut = new ThermalVent(XY.New(1, 1), XY.New(1, 7));

            var actual = sut.Range();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThermalVent_Overlap_FindsOverlappingPositions()
        {
            var vent1 = new ThermalVent(XY.New(1, 1), XY.New(1, 5));
            var vent2 = new ThermalVent(XY.New(1, 4), XY.New(1, 7));
            var vent3 = new ThermalVent(XY.New(1, 2), XY.New(4, 2));
            var vent4 = new ThermalVent(XY.New(4, 1), XY.New(4, 3));
            var vent5 = new ThermalVent(XY.New(1, 1), XY.New(1, 2));

            var expected = new HashSet<XY>(new[] { XY.New(1, 4), XY.New(1, 5), XY.New(1, 2), XY.New(4, 2), XY.New(1, 1) });

            var actual = ThermalVent.PartialOverlap(new[] { vent1, vent2, vent3, vent4, vent5 });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThermalVent_Range_ReturnsDiagonalPositiveSlopeRange_LowerStart()
        {
            var expected = new[] { XY.New(3, 4), XY.New(4, 5), XY.New(5, 6), XY.New(6, 7), XY.New(7, 8) };

            var actual = XY.Range(XY.New(3, 4), XY.New(7, 8));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThermalVent_Range_ReturnsDiagonalPositiveSlopeRange_UpperStart()
        {
            var expected = new[] { XY.New(3, 4), XY.New(4, 5), XY.New(5, 6), XY.New(6, 7), XY.New(7, 8) };

            var actual = XY.Range(XY.New(7, 8), XY.New(3, 4));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThermalVent_Range_ReturnsDiagonalNegativeSlopeRange_LowerStart()
        {
            var expected = new[] { XY.New(0, 9), XY.New(1, 8), XY.New(2, 7), XY.New(3, 6), XY.New(4, 5) };

            var actual = XY.Range(XY.New(0, 9), XY.New(4, 5));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThermalVent_Range_ReturnsDiagonalNegativeSlopeRange_UpperStart()
        {
            var expected = new[] { XY.New(0, 9), XY.New(1, 8), XY.New(2, 7), XY.New(3, 6), XY.New(4, 5) };

            var actual = XY.Range(XY.New(4, 5), XY.New(0, 9));

            Assert.Equal(expected, actual);
        }
    }
}
