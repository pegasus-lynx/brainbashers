using BridgeSolver.Models;
using BridgeSolver.Data;
using BridgeSolver.Solvers.Rules;

namespace BridgeSolver.Solvers
{
    public class RuleBasedBridgeSolver : AbstractBridgeSolver
    {
        public RuleBasedBridgeSolver(List<RuleBase>? rules = null)
        {
            if (rules is null)
                rules = DefaultRules;

            Rules = rules;
        }

        public override void Solve(AbstractBridgeModel model)
        {
            bool newEdge = true;
            
            while(newEdge)
            {
                newEdge = false;

                foreach(Point p in model.Cells)
                {
                    foreach(RuleBase rule in Rules)
                    {
                        if (rule.CanApply(model, p))
                        {
                            Console.WriteLine($"Applying rule to point : ({p.X}, {p.Y})");
                            rule.Apply(model, p);
                            model.Print();
                            newEdge = true;
                        }
                    }
                }
            }
        }

        public static List<RuleBase> DefaultRules = [ new PHPRule() ];

        public List<RuleBase> Rules { get; private set; }
    }
}
