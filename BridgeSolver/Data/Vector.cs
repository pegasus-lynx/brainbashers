using BrainBashersSolver.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeSolver.Data
{
    public class Vector
    {
        public Vector(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public Vector(Point end) : this(new Point(0, 0), end) { }

        public DirectionEnum Direction
        {
            get
            {
                int dx = DeltaX;
                int dy = DeltaY;

                if (dx == 0 && dy == 0)
                    return DirectionEnum.None;

                if (dx == 0 || dy == 0)
                {
                    if (dx == 0 && dy > 0)
                        return DirectionEnum.Right;
                    if (dx == 0 && dy < 0)
                        return DirectionEnum.Left;
                    if (dx > 0 && dy == 0)
                        return DirectionEnum.Down;
                    if (dx < 0 && dy == 0)
                        return DirectionEnum.Up;
                }

                return DirectionEnum.None;
            }
        }

        public int DeltaX => End.X - Start.X;
        public int DeltaY => End.Y - Start.Y;

        public static Vector GetUnitVector(Point p1, Point p2)
        {
            Vector vec = new Vector(p1, p2);
            return GetUnitVector(vec.Direction);
        }

        public static Vector GetUnitVector(DirectionEnum direction)
        {
            return direction switch
            {
                DirectionEnum.Up => Up,
                DirectionEnum.Down => Down,
                DirectionEnum.Right => Right,
                DirectionEnum.Left => Left,
                _ => None,
            };
        }

        public static Vector Up => new Vector(new Point(-1, 0));
        public static Vector Down => new Vector(new Point(1, 0));
        public static Vector Right => new Vector(new Point(0, 1));
        public static Vector Left => new Vector(new Point(0, -1));
        public static Vector None => new Vector(new Point(0, 0));

        public Point Start { get; private set; }
        public Point End { get; private set; }
    }
}