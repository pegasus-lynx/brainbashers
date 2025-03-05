using BridgeBot.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeBot.Bots
{
    public abstract class BotBase
    {
        public virtual void AddRule(RuleBase rule) { }

        public virtual void RemoveRule(RuleBase rule) { }

        public virtual void Solve() { }
    }
}