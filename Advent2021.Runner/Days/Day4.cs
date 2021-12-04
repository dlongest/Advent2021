using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent2021.Runner.Extensions;

namespace Advent2021.Runner.Days
{
    public class Day4 : IAdventProblem
    {
        public void A()
        {
            var input = FileSystem.MakeDataFilePath("day4")
                                  .ReadGroups(s => s.Length == 0);

            var numbers = input.First()[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(n => Int32.Parse(n));

            var cards = Enumerable.Range(0, input.Count() - 1)
                                  .Select(i => new BingoCard(i + 1, ParseCard(input.ElementAt(i + 1)))).ToArray();
            
            foreach (var number in numbers)
            {
                foreach (var card in cards)
                {
                    var bingo = card.Mark(number);

                    if (bingo)
                    {
                        Console.WriteLine($"Bingo in card {card.Id} with final value {number}");
                        Console.WriteLine($"Score = {card.Score(number)}");
                        return;
                    }                       
                }
            }

            Console.WriteLine("No bingo :-(");
        }

        public void B()
        {
            var input = FileSystem.MakeDataFilePath("day4")
                                  .ReadGroups(s => s.Length == 0);

            var numbers = input.First()[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(n => Int32.Parse(n));

            var cards = Enumerable.Range(0, input.Count() - 1)
                                  .Select(i => new BingoCard(i + 1, ParseCard(input.ElementAt(i + 1)))).ToArray();

            BingoCard lastCard = null;

            foreach (var number in numbers)
            {
                var cardsToRemove = new List<int>();

                foreach (var card in cards)
                {
                    var bingo = card.Mark(number);

                    if (bingo)
                    {
                        cardsToRemove.Add(card.Id);
                    }
                }

                cards = cards.Where(c => !cardsToRemove.Contains(c.Id)).ToArray();

                if (cards.Count() == 1)
                {
                    lastCard = cards.First();
                }

                if (!cards.Any())
                {
                    Console.WriteLine($"Last Card {lastCard.Id} on winning number {number} with score = {lastCard.Score(number)}");
                    return;
                }
            }

            Console.WriteLine("No bingo :-(");
        }

        private IEnumerable<int[]> ParseCard(string[] cardValues)
        {
            return cardValues.Select(c => c.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(ca => Int32.Parse(ca)).ToArray());

        }

    }

    public class BingoCard
    {
        private HashSet<int>[] openRows = new HashSet<int>[5];
        private HashSet<int>[] openColumns = new HashSet<int>[5];
        private IDictionary<int, RowColumnIndex> valueToPosition = new Dictionary<int, RowColumnIndex>();

        private HashSet<int>[] markedRows = Enumerable.Range(0, 5).Select(_ => new HashSet<int>()).ToArray();
        private HashSet<int>[] markedColumns = Enumerable.Range(0, 5).Select(_ => new HashSet<int>()).ToArray();

        public BingoCard(int id, IEnumerable<int[]> card)
        {
            this.Id = id;

            foreach (var index in Enumerable.Range(0, 5))
            {
                openRows[index] = new HashSet<int>(card.ElementAt(index));
                openColumns[index] = new HashSet<int>(card.Slice(index));
            }

            foreach (var rowIndex in Enumerable.Range(0, 5))
            {
                foreach (var columnIndex in Enumerable.Range(0, 5))
                {
                    var value = card.ElementAt(rowIndex)[columnIndex];
                    var pair = RowColumnIndex.New(rowIndex, columnIndex);

                    valueToPosition.Add(value, pair);
                }
            }
        }
        
        public bool Mark(int value)
        {
            // If this card doesn't have the value, we're done. 
            if (!valueToPosition.ContainsKey(value))
            {
                return false;
            }

            // Find where on the card this value is. 
            var index = valueToPosition[value];

            // Remove the value from the row and column sets
            this.openRows[index.Row].Remove(value);
            this.openColumns[index.Column].Remove(value);
            this.markedRows[index.Row].Add(value);
            this.markedColumns[index.Column].Add(value);

            // If either of the affected row and column sets are now empty, we have bingo. 
            return this.openRows[index.Row].Count == 0 || this.openColumns[index.Column].Count == 0;
        }       

        public int Score(int winningNumber)
        {
            var sum = this.openRows.Sum(o => o.Select(h => h).Sum());

            return sum * winningNumber;
        }

        public RowColumnIndex WhereIs(int value)
        {
            return this.valueToPosition.ContainsKey(value) ? this.valueToPosition[value] : RowColumnIndex.None;
        }

        public int Id { get; private set; }
    }

    public class RowColumnIndex
    {
        public RowColumnIndex(int rowIndex, int columnIndex)
        {
            this.Row = rowIndex;
            this.Column = columnIndex;
        }

        public static RowColumnIndex None = new RowColumnIndex(Int32.MinValue, Int32.MinValue);

        public static RowColumnIndex New(int rowIndex, int columnIndex)
        {
            return new RowColumnIndex(rowIndex, columnIndex);
        }

        public int Row { get; private set; }

        public int Column { get; private set; }

        public override bool Equals(object obj)
        {
            var o = obj as RowColumnIndex;

            if (o == null)
            {
                return false;
            }

            return this.Row == o.Row && this.Column == o.Column;
        }

        public override int GetHashCode()
        {
            return this.Row.GetHashCode() + 17 * this.Column.GetHashCode();
        }

        public override string ToString()
        {
            return $"<{this.Row}, {this.Column}>";
        }
    }
}
