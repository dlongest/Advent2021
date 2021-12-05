using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Runner
{
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
