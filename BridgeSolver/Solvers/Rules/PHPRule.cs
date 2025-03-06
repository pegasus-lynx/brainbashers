using BrainBashersSolver.Common;
using BridgeSolver.Data;
using BridgeSolver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeSolver.Solvers.Rules
{
    /// <summary>
    /// PHP - Pigeon Hole Principle Rule
    /// </summary>
    public class PHPRule : RuleBase
    {
        public override void Apply(AbstractBridgeModel model, Point p)
        {
            IDictionary<DirectionEnum, Point> neighbors = model.CellNeighbors(p);

            int totalFreeNbrEdges = 0;
            int freeEdges = FreeCellEdgesCount(model, p);

            Dictionary<DirectionEnum, int> freeNbrEdges = new();
            foreach (DirectionEnum direction in neighbors.Keys)
            {
                Point nbr = neighbors[direction];
                int _freeEdges = FreeCellEdgesCount(model, nbr);

                freeNbrEdges[direction] = int.Min(_freeEdges, 2 - model.EdgesCount(p, nbr));
                totalFreeNbrEdges += freeNbrEdges[direction];
            }

            Dictionary<DirectionEnum, int> minNeighborEdges = new();
            foreach (DirectionEnum direction in freeNbrEdges.Keys)
            {
                int otherFreeNbrEdges = totalFreeNbrEdges - freeNbrEdges[direction];
                if (otherFreeNbrEdges < freeEdges)
                    model.CreateEdge(p, neighbors[direction], freeEdges - otherFreeNbrEdges);
            }
        }

        public override bool CanApply(AbstractBridgeModel model, Point p)
        {
            IDictionary<DirectionEnum, Point> neighbors = model.CellNeighbors(p);

            int totalFreeNbrEdges = 0;
            int freeEdges = FreeCellEdgesCount(model, p);

            Dictionary<DirectionEnum, int> freeNbrEdges = new();
            foreach(DirectionEnum direction in neighbors.Keys)
            {
                Point nbr = neighbors[direction];
                int _freeEdges = FreeCellEdgesCount(model, nbr);
                
                freeNbrEdges[direction] = int.Min(_freeEdges, 2 - model.EdgesCount(p, nbr));
                totalFreeNbrEdges += freeNbrEdges[direction];
            }

            Dictionary<DirectionEnum, int> minNeighborEdges = new();
            foreach(DirectionEnum direction in freeNbrEdges.Keys)
            {
                if (totalFreeNbrEdges - freeNbrEdges[direction] < freeEdges)
                    return true;
            }

            return false;
        }

        private static int FreeCellEdgesCount(AbstractBridgeModel model, Point p)
        {
            return model.MaxCellEdgesCount(p) - model.CellEdgesCount(p);
        }
    }
}
