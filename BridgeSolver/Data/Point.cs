using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeSolver.Data
{
    public record Point(int X, int Y)
    {
        public static Point operator +(Point p, Vector vec)
        {
            return new Point(p.X + vec.DeltaX, p.Y + vec.DeltaY);
        }

        public static Point operator -(Point p, Vector vec)
        {
            return new Point(p.X - vec.DeltaX, p.Y - vec.DeltaY);
        }

        public static Vector operator -(Point p1, Point p2)
        {
            return new Vector(p1, p2);
        }

        public int X { get; private set; } = X;
        public int Y { get; private set; } = Y;
    }
}