using BrainBashersSolver.Common;
using BridgeSolver.Data;

namespace BridgeSolver.Models
{
    public abstract class AbstractBridgeModel(int size)
    {
        public abstract List<Point> Cells { get; }

        public abstract int MaxCellEdgesCount(Point p);
        public abstract int CellEdgesCount(Point p);
        public abstract int EdgesCount(Point p1, Point p2);

        public abstract IDictionary<DirectionEnum, Point> CellNeighbors(Point p);
        public abstract IDictionary<DirectionEnum, Edge> CellEdges(Point p);

        public abstract void CreateEdge(Point p1, Point p2, int weight);
        public abstract void RemoveEdge(Point p1, Point p2, int weight);

        public abstract void Print();

        public int Size { get; private set; } = size;
    }
}