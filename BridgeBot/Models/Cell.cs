namespace BridgeBot.Models
{
    public class Cell : IEquatable<Cell>
    {
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Cell(Tuple<int, int> tuple)
        {
            X = tuple.Item1;
            Y = tuple.Item2;
        }

        public Cell(Cell cell)
        {
            X = cell.X;
            Y = cell.Y;
        }

        public Tuple<int, int> ToTuple()
        {
            return new Tuple<int, int>(X, Y);
        }

        public bool Equals(Cell? other)
        {
            if (other is null)
                return false;

            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(Cell c1, Cell c2) => c1.Equals(c2);
        public static bool operator !=(Cell c1, Cell c2) => !c1.Equals(c2);

        public int X { get; set; }
        public int Y { get; set; }

        public static Cell operator +(Cell c1, Cell c2)
        {
            return new Cell(c1.X + c2.X, c1.Y + c2.Y);
        }

        public static Cell operator -(Cell c1, Cell c2)
        {
            return new Cell(c1.X - c2.X, c1.Y - c2.Y);
        }

        public override string ToString()
        {
            return $"[ {X}, {Y} ]";
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            return Equals(obj as Cell);
        }
    }
}