using BridgeBot.Models;
using BridgeBot.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeBot.Bots
{
    public class BridgeBot
    {
        public BridgeBot() { }

        public BridgeBot(List<RuleBase> rules)
        {
            Rules = rules is null ? [] : rules;
        }


        public void AddRule(RuleBase rule)
        {
            Rules.Add(rule);
        }

        public void RemoveRule(RuleBase rule)
        {
            Rules.Remove(rule);
        }


        public void Solve(BridgePuzzle puzzle)
        {
            Console.WriteLine("Solving Puzzle Now ....");

            puzzle.VerifyPuzzleState();

            int iteration = 0;
            int size = puzzle.Size;
            bool newEdges = true;

            while (newEdges)
            {
                Console.WriteLine($"Running iteration {iteration++}");
                Console.WriteLine("----------------------------");
                newEdges = false;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (puzzle.MaxCellEdges(new Cell(i, j)) > 0)
                        {
                            Console.WriteLine($"Checking Cell : {i} , {j}");
                            foreach (RuleBase rule in Rules)
                            {
                                if (rule.CanApply(puzzle, new Cell(i, j)))
                                {
                                    Console.WriteLine($"Applying Rule : {rule.Name}");
                                    rule.Apply(puzzle, new Cell(i, j));
                                    newEdges = true;
                                }
                            }
                        }
                    }
                }

                BridgePuzzle.Print(puzzle);
                Console.WriteLine("----------------------------");
            }
        }

        public List<RuleBase> Rules { get; } = [];


    }
}