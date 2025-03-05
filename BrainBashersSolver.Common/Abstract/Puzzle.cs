using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBashersSolver.Common.Abstract
{
    public abstract class Puzzle
    {
        public string Date { get; set; } = "";
        public Difficulty Difficulty { get; set; }
        public int Size { get; set; }

        public abstract void Solve();
        public abstract void Print(bool solved = false);
    }
}