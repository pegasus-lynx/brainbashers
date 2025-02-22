using BridgeBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeBot.Rules
{
    public abstract class RuleBase
    {
        public string Name { get; set; } = "";

        public abstract bool CanApply(BridgePuzzle puzzle, Cell cell);

        public abstract void Apply(BridgePuzzle puzzle, Cell cell);
    }
}
