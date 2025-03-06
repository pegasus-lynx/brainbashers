using BridgeSolver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeSolver.Solvers
{
    public abstract class AbstractBridgeSolver
    {
        public abstract void Solve(AbstractBridgeModel model);
    }
}
