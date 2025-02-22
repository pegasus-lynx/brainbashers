using BridgeBot.Models;
using BridgeBot.Rules;
using System.Net.Http;
using System.Threading.Tasks;
using Bot = BridgeBot.Bots;

namespace BridgeBot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] testargs = new string[] { "C:\\personal\\repos\\brainbashers\\Data\\0221-10-Easy.txt" };
            
            Console.WriteLine("Hello World !");
            
            Console.WriteLine("Creating Bridge Puzzle...");
            BridgePuzzle puzzle = CreateBridgePuzzleFromArgs(testargs);
            
            Console.WriteLine("Printing Bridge Puzzle...");
            BridgePuzzle.Print(puzzle);

            Bot.BridgeBot bot = new Bot.BridgeBot();
            AddRules(bot);
            bot.Solve(puzzle);

            Console.WriteLine();
            Console.WriteLine("Printing Bridge Puzzle After Solving..");
            BridgePuzzle.PrintSolved(puzzle);
        }

        private static void AddRules(Bot.BridgeBot bot)
        {
            bot.AddRule(new ExNyRule(8, 4));
            bot.AddRule(new ExNyRule(7, 4));
            bot.AddRule(new ExNyRule(6, 3));
            bot.AddRule(new ExNyRule(5, 3));
            bot.AddRule(new ExNyRule(4, 2));
            bot.AddRule(new ExNyRule(3, 2));
            bot.AddRule(new ExNyRule(2, 1));
            bot.AddRule(new ExNyRule(1, 1));
        }

        private static BridgePuzzle CreateBridgePuzzleFromArgs(string[] args)
        {
            if(args.Length != 1 && args.Length != 3)
                throw new ArgumentException("The arguments list is wrong");


            if(args.Length == 1)
            {
                string filePath = args[0];
                
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File not found : {filePath}");

                return BridgePuzzle.LoadFromFile(filePath);
            }

            string dateString = args[0];
            string size = args[1];
            string difficulty = args[2];

            return new BridgePuzzle(dateString, int.Parse(size), (Difficulty)Enum.Parse(typeof(Difficulty), difficulty, true));
        }
    }
}
