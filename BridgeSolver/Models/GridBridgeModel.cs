using BrainBashersSolver.Common;
using BridgeSolver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeSolver.Models
{
    public class GridBridgeModel : AbstractBridgeModel
    {
        public GridBridgeModel(int[,] grid, int size) : base(size)
        {
            Grid = InitializeCellGrid(grid, size);
        }


        public int MaxCellEdgesCount(Cell c)
        {
            if (int.TryParse(c.Value, out int val))
                return val;

            return 0;
        }

        public int CellEdgesCount(Cell c)
        {
            var directionEdgesDict = CellEdges(c);
            return directionEdgesDict.Values.Sum(e => e.Weight);
        }

        public int EdgesCount(Cell c1, Cell c2)
        {
            // Edge case : if any c has Position null, no edges
            if (c1.Position is null || c2.Position is null)
                return 0;

            // Cell is out of bounds, no edges
            if (!VerifyBounds(c1.Position) || !VerifyBounds(c2.Position))
                return 0;

            Vector move = Vector.GetUnitVector(c1.Position, c2.Position);

            // Points don't form lines parallel to axis
            // Hence, they can't be neighbors, and no edges between them
            if (move.DeltaX == 0 && move.DeltaY == 0)
                return 0;

            // First and last edge points
            Point fp = c1.Position + move;
            Point ep = c2.Position - move;

            // Adjacent cells
            if (fp == c2.Position)
                return 0;

            Cell fc = Grid![fp.X, fp.Y];
            Cell ec = Grid![ep.X, ep.Y];

            if (fc.Value != ec.Value)
                return 0;

            string valString = fc.Value;

            if (valString[0] != 'h' || valString[0] != 'v')
                return 0;

            char val;
            if (valString[0] == 'h')
            {
                if (move.DeltaX != 0)
                    return 0;

                val = valString[1];
            }
            else
            {
                if (move.DeltaY != 0)
                    return 0;

                val = valString[1];
            }


            return val switch
            {
                '1' => 1,
                '2' => 2,
                _ => 0,
            };
        }

        public IDictionary<DirectionEnum, Cell> CellNeighbors(Cell c)
        {
            Dictionary<DirectionEnum, Cell> neighbors = [];

            if (c.Position is null)
                return neighbors;

            foreach (DirectionEnum direction in GridMoves)
            {
                Vector move = Vector.GetUnitVector(direction);
                Point p = c.Position + move;

                while (VerifyBounds(p))
                {
                    Cell curr = Grid![p.X, p.Y];
                    if (!IsEmpty(curr))
                    {
                        if (!IsEdge(curr))
                        {
                            neighbors.Add(direction, curr);
                        }

                        break;
                    }

                    p += move;
                }
            }

            return neighbors;
        }

        public IDictionary<DirectionEnum, Edge> CellEdges(Cell c)
        {
            Dictionary<DirectionEnum, Edge> cellEdges = [];

            if (c.Position is null || !VerifyBounds(c.Position))
                return cellEdges;

            var neighbors = CellNeighbors(c);


            foreach (DirectionEnum dir in neighbors.Keys)
            {
                cellEdges[dir] = new Edge(c.Position, neighbors[dir].Position!, EdgesCount(c, neighbors[dir]));
            }

            return cellEdges;
        }

        public void CreateEdge(Cell c1, Cell c2, int weight)
        {
            if (c1.Position is null || c2.Position is null)
                return;

            if (!VerifyBounds(c1.Position) || !VerifyBounds(c2.Position))
                return;

            Vector move = Vector.GetUnitVector(c1.Position, c2.Position);

            if (move.DeltaX == 0 && move.DeltaY == 0)
                return;

            bool canMakeEdge = true;
            for (Point p = c1.Position + move; p != c2.Position; p += move)
            {
                Cell c = Grid![p.X, p.Y];
                if (!IsEmpty(c))
                {
                    if ((move.DeltaX == 0 && c.Value[0] != 'h')
                        || (move.DeltaY == 0 && c.Value[0] != 'v'))
                    {
                        canMakeEdge = false;
                        break;
                    }
                }
            }

            if (canMakeEdge)
            {
                int edgeCount = EdgesCount(c1, c2);
                int newWeight = edgeCount + weight;

                if (newWeight > 2)
                    return;

                for (Point p = c1.Position + move; p != c2.Position; p += move)
                {
                    Cell c = Grid![p.X, p.Y];

                    char edgeType = move.DeltaX == 0 ? 'h' : 'v';
                    c.Value = $"{edgeType}{newWeight}";
                }
            }
        }

        public void RemoveEdge(Cell c1, Cell c2, int weight)
        {
            int edgeCount = EdgesCount(c1, c2);
            int newWeight = edgeCount - weight;

            if (newWeight < 0 || weight == 0)
                return;

            Vector move = Vector.GetUnitVector(c1.Position!, c2.Position!);
            for (Point p = c1.Position! + move; p != c2.Position!; p += move)
            {
                Cell c = Grid![p.X, p.Y];

                char edgeType = move.DeltaX == 0 ? 'h' : 'v';
                c.Value = $"{edgeType}{newWeight}";
            }
        }

        public override int MaxCellEdgesCount(Point p)
        {
            return MaxCellEdgesCount(Grid[p.X, p.Y]);
        }

        public override int CellEdgesCount(Point p)
        {
            return CellEdgesCount(Grid[p.X, p.Y]);
        }

        public override IDictionary<DirectionEnum, Point> CellNeighbors(Point p)
        {
            var cellNeighbors = CellNeighbors(Grid![p.X, p.Y]);
            Dictionary<DirectionEnum, Point> pointNeighbors = [];
            foreach (DirectionEnum key in cellNeighbors.Keys)
            {
                Point? position = cellNeighbors[key].Position;
                if (position is not null)
                    pointNeighbors.Add(key, position);
            }

            return pointNeighbors;
        }

        public override IDictionary<DirectionEnum, Edge> CellEdges(Point p)
        {
            throw new NotImplementedException();
        }

        public override int EdgesCount(Point p1, Point p2)
        {
            if (!VerifyBounds(p1) || !VerifyBounds(p2))
                return 0;

            return EdgesCount(Grid![p1.X, p1.Y], Grid![p2.X, p2.Y]);
        }


        public override void CreateEdge(Point p1, Point p2, int weight)
        {
            if (VerifyBounds(p1) && VerifyBounds(p2))
                CreateEdge(Grid![p1.X, p1.Y], Grid![p2.X, p2.Y], weight);
        }

        public override void RemoveEdge(Point p1, Point p2, int weight)
        {
            if (VerifyBounds(p1) && VerifyBounds(p2))
                RemoveEdge(Grid![p1.X, p1.Y], Grid![p2.X, p2.Y], weight);
        }


        private bool VerifyBounds(Point p)
        {
            return (p.X >= 0 && p.X < Size) && (p.Y >= 0 && p.Y < Size);
        }

        private static Cell[,] InitializeCellGrid(int[,] grid, int size)
        {
            Cell[,] _grid = new Cell[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int val = grid[i, j];
                    _grid[i, j] = new Cell(i, j, val != 0 ? $"{val:00}" : "  ");
                }
            }

            return _grid;
        }


        public bool IsEdge(Cell c)
        {
            if (c.Position is null)
                return false;

            if (VerifyBounds(c.Position))
                return c.Value[0] == 'h' || c.Value[0] == 'v';

            return false;
        }

        public bool IsEmpty(Cell c)
        {
            if (c.Position is null || !VerifyBounds(c.Position))
                return false;

            return c.Value == "  ";
        }

        public override void Print()
        {
            if (Grid is null)
            {
                Console.WriteLine("Grid is not initialized");
                return;
            }

            Console.WriteLine("*********************** Bridge Puzzle ****************************");
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    string val = Grid[i, j].Value;

                    if (val.StartsWith('h'))
                    {
                        Console.Write(val[1] == '1' ? "--" : "==");
                    }
                    else if (val.StartsWith('v'))
                    {
                        Console.Write(val[1] == '1' ? " |" : "||");
                    }
                    else
                    {
                        Console.Write(val);
                    }

                    Console.Write(" ");
                }

                Console.Write("\n");
            }
        }

        private static IList<DirectionEnum> GridMoves
        {
            get
            {
                return [
                        DirectionEnum.Up, DirectionEnum.Down,
                        DirectionEnum.Right, DirectionEnum.Left
                ];
            }
        }
        public Cell[,] Grid { get; set; }

    }
}