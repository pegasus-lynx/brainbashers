using BridgeSolver.Data;
using BridgeSolver.Models;

namespace BridgeSolver.Solvers.Rules
{
    public abstract class RuleBase
    {
        public string Name { get; set; } = "";

        public abstract bool CanApply(AbstractBridgeModel model, Point p);

        public abstract void Apply(AbstractBridgeModel model, Point p);
    }
}
