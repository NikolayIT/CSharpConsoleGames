using System;

namespace TicTacToe
{
    public class Index
    {
        public Index(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public Index(string str)
        {
            var parts = str.Split(',');
            this.Row = int.Parse(parts[0]);
            this.Column = int.Parse(parts[1]);
        }

        public int Row { get; set; }

        public int Column { get; set; }

        public override bool Equals(object obj)
        {
            var otherIndex = obj as Index;

            return this.Row == otherIndex.Row &&
                this.Column == otherIndex.Column;
        }

        public override string ToString()
        {
            return $"{this.Row},{this.Column}";
        }
    }
}
