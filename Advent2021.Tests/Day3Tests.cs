using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Advent2021.Runner.Days;
using System.Collections;

namespace Advent2021.Tests
{
    public class Day3Tests
    {
        [Fact]
        public void FromBigEndian_CreatesCorrectArray()
        {
            var bigEndian = "10110";

            var expected = new int[] { 0, 1, 1, 0, 1 };

            var actual = Bits.FromBigEndian(bigEndian);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FromLittleEndian_CreatesCorrectArray()
        {
            var littleEndian = "01101";

            var expected = new int[] { 0, 1, 1, 0, 1 };

            var actual = Bits.FromLittleEndian(littleEndian);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("10110", 22)]
        [InlineData("01001", 9)]
        public void ToInt_ConvertsBitsToCorrectNumber(string bigEndian, int expected)
        {
            var bits = Bits.FromBigEndian(bigEndian);

            var actual = Bits.ToInt(bits);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("10110", 9)]
        [InlineData("01001", 22)]
        public void Not_InvertsBitsCorrectly(string bigEndian, int expected)
        {
            var bits = Bits.FromBigEndian(bigEndian);

            var inverted = Bits.Not(bits);

            var actual = Bits.ToInt(inverted);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [ClassData(typeof(GammaTestCases))]
        public void FindGammaRate_IsCorrect(IEnumerable<string> bigEndianReadings, int[] expected)
        {
            var littleEndianBits = bigEndianReadings.Select(b => Bits.FromBigEndian(b));

            var actual = PowerConsumption.FindGammaRate(littleEndianBits);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Compute_CalculatesCorrectPowerConsumption_FromGamma()
        {
            var expected = 198;

            var gammaRate = Bits.FromBigEndian("10110");

            var actual = PowerConsumption.Compute(gammaRate);

            Assert.Equal(expected, actual);
        }

        private class GammaTestCases : IEnumerable<object[]>
        {
            private List<object[]> data = new List<object[]>();

            public GammaTestCases()
            {
                this.data.Add(new object[]
                    {
                        new [] { "00100", "11110", "10110", "10111", "10101", "01111","00111","11100","10000","11001","00010","01010" },
                        new int[] { 0, 1, 1, 0, 1 }
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
