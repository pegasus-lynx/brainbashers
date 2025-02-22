using System;
using BridgeBot.Models;


namespace BridgeBot.Rules
{
    public class ExNyRule : RuleBase
    {
        public ExNyRule(int edgesCount, int neighborsCount)
        {
            if(2*neighborsCount <= edgesCount - 2)
            {
                throw new ArgumentException($"Rule not possible for - Edges : {edgesCount} , Neighbors : {neighborsCount}");
            }

            Name = $"{edgesCount} Edges - {neighborsCount} NeighborsCount";
            EdgesCount = edgesCount;
            NeighborsCount = neighborsCount;
        }

        public override void Apply(BridgePuzzle puzzle, Cell cell)
        {
            List<Cell?> neighbors = puzzle.GetCurrentCellNeighbors(cell);

            bool makeDualEdges = EdgesCount == 2 * NeighborsCount;

            foreach(Cell? neighbor in neighbors)
            {
                if (neighbor is null)
                    continue;

                int remainingEdges = puzzle.MaxCellEdges(neighbor) - puzzle.CellEdgesCount(neighbor);

                if (remainingEdges == 0)
                    continue;

                puzzle.MakeEdges(cell, neighbor, makeDualEdges ? 2 : 1);
            }
        }

        public override bool CanApply(BridgePuzzle puzzle, Cell cell)
        {
            List<Tuple<Cell?,int>> remainingEdges = puzzle.RemainingCellEdges(cell);

            int neighborCount = 0;
            int edgesToCreate = 0;
            for(int i=0;i<4;i++)
            {
                Tuple<Cell?, int> edgeCellPair = remainingEdges[i];
                Cell? neighbor = edgeCellPair.Item1;
                int edge = edgeCellPair.Item2;

                if (neighbor is null)
                    continue;

                if (edge == 0)
                    continue;

                neighborCount++;
                edgesToCreate += edge;
            }

            edgesToCreate = Math.Min(edgesToCreate, puzzle.MaxCellEdges(cell)-puzzle.CellEdgesCount(cell));

            if (edgesToCreate == EdgesCount && neighborCount == NeighborsCount)
                return true;
            return false;
        }

        public int EdgesCount { get; }
        public int NeighborsCount { get; }

    }

    public class PigeonHoleRule : RuleBase
    {
        public override void Apply(BridgePuzzle puzzle, Cell cell)
        {
            throw new NotImplementedException();
        }

        public override bool CanApply(BridgePuzzle puzzle, Cell cell)
        {
            throw new NotImplementedException();
        }
    }
}