using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeBot.Models
{
    public enum DirectionUnit
    {
        Up,
        Down,
        Right,
        Left
    }

    public static class Direction
    {
        public static Cell Up
        {
            get
            {
                //if (_up.X != 0 || _up.Y != -1)
                //    _up = new Cell(0, -1);
                return _up;
            }
        }

        public static Cell Down
        {
            get
            {
                //if (_down.X != 0 || _down.Y != 1)
                //    _down = new Cell(0, 1);
                return _down;
            }
        }

        public static Cell Right
        {
            get
            {
                //if (_right.X != 0 || _right.Y != 1)
                //    _right = new Cell(1, 0);
                return _right;
            }
        }

        public static Cell Left
        {
            get
            {
                //if (_left.X != 0 || _left.Y != 1)
                //    _left = new Cell(0, 1);
                return _left;
            }
        }

        public static Cell FromUnit(DirectionUnit directionUnit)
        {
            return DirectionsList[(int)directionUnit];
        }

        public static DirectionUnit? UnitFromCells(Cell c1, Cell c2)
        {
            Cell c = c2 - c1;
            if (c.X != 0 && c.Y != 0)
                return null;

            if (c.X == 0 && c.Y > 0)
                return DirectionUnit.Right;
            if (c.X == 0 && c.Y < 0)
                return DirectionUnit.Left;
            if (c.X < 0 && c.Y == 0)
                return DirectionUnit.Up;
            if (c.X > 0 && c.Y == 0)
                return DirectionUnit.Down;

            return null;
        }

        public static List<Cell> DirectionsList => [Up, Down, Right, Left];

        private static readonly Cell _left = new(0, -1);
        private static readonly Cell _right = new(0, 1);
        private static readonly Cell _down = new(1, 0);
        private static readonly Cell _up = new(-1, 0);
    }
}