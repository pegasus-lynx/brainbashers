using CommandLine;
using BrainBashersSolver.Common;
using BrainBashersSolver.Common.Abstract;

namespace BridgeSolver
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Hello, Bridge Solver welcomes you !!");
            Console.WriteLine("Parsing arguments ....");
            bool success = ParseArguments(args, out BridgeOptions options);

            if (!success)
            {
                Console.WriteLine("Program execution stopped");
                return 0;
            }

            Console.WriteLine("Creating Bridge Puzzle ....");
            var puzzle = new BridgePuzzle(options);

            puzzle.Print();

            Console.WriteLine("Solving Puzzle ....");
            puzzle.Solve();

            Console.WriteLine("Printing Solved Bridge ....");
            puzzle.Print(solved: true);

            return 0;
        }


        private static bool ParseArguments(string[] args, out BridgeOptions options)
        {
            bool success = true;
            options = new BridgeOptions();

            var results = Parser.Default.ParseArguments<BridgeOptions>(args).WithParsed(options => {
                if(!string.IsNullOrEmpty(options.PuzzleFile))
                {
                    if(!File.Exists(options.PuzzleFile))
                    {
                        Console.WriteLine($"Provided file : ${options.PuzzleFile} does not exist");
                        Console.WriteLine("Since --puzzlefile is not empty , other args were not parsed");
                        success = false;
                        return;
                    }
                }
                else if (!Helpers.TryParseDateString(options.Date, out DateTime date))
                {
                    Console.WriteLine($"Provided date : {options.Date} is not in right format.");
                    Console.WriteLine($"Date format is mmdd. For 28th Feb, date string should be 0228");
                    success = false;
                    return;
                }


            });

            if (!success || results.Tag != ParserResultType.Parsed)
                return false;

            options = results.Value;
            return true;
        }
    }

    public class BridgeOptions
    {
        [Option]
        public string Date { get; set; } = "today";

        [Option]
        public int Size { get; set; }

        [Option]
        public Difficulty Difficulty { get; set; }

        [Option]
        public string PuzzleFile { get; set; } = "";
    }
}
