using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeSolver.Data
{
    public class Cell
    {
        public Cell(Point? position = null, string value = " ")
        {
            Position = position;
            Value = value;
        }

        public Cell(int x, int y, string value = "  ")
        {
            Position = new Point(x, y);
            Value = value;
        }

        public int X => Position?.X ?? -1;
        public int Y => Position?.Y ?? -1;

        public string Value { get; set; }
        public Point? Position { get; set; }
    }
}