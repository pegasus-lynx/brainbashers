using BrainBashersSolver.Common;

namespace BridgeSolver.Data
{
    public class Edge
    {
        public Edge(Point start, Point end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
        }

        public Point Start { get; private set; }
        public Point End { get; private set; }
        public int Weight { get; set; }

    }
}