using BrainBashersSolver.Common;
using BrainBashersSolver.Common.Abstract;
using BridgeSolver.Models;
using System.Diagnostics;

namespace BridgeSolver
{
    public class BridgePuzzle : Puzzle
    {
        public BridgePuzzle(BridgeOptions options)
        {
            if (options.PuzzleFile is not null)
            {
                Load(options.PuzzleFile);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void Print(bool solved = false)
        {
            Model!.Print();
        }

        public override void Solve()
        {
        }


        private void Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File : {filePath} does not exist");

            string content = File.ReadAllText(filePath);
            List<int> cells = ProcessFileContent(content);

            int size = (int)Math.Sqrt((double)(cells.Count));

            Debug.Assert(cells.Count == (size * size), $"Not a perfect square. Cells = {cells.Count}, Size = {size}");
            int[,] grid = new int[size, size];
            for (int k = 0; k < cells.Count; k++)
            {
                grid[k / size, k % size] = cells[k];
            }

            Model = new GridBridgeModel(grid, size);
            Size = size;
        }

        private static List<int> ProcessFileContent(string content)
        {
            List<int> cells = [];
            foreach (string part in content.Split('<'))
            {
                if (part.StartsWith("img"))
                {
                    foreach (string attr in part.Split(" "))
                    {
                        if (attr.Contains("src"))
                        {
                            string value = attr.Split("=")[1];
                            if (value.Contains("x.png"))
                            {
                                cells.Add(0);
                            }
                            else if (value.Contains("1w.png"))
                            {
                                cells.Add(1);
                            }
                            else if (value.Contains("2w.png"))
                            {
                                cells.Add(2);
                            }
                            else if (value.Contains("3w.png"))
                            {
                                cells.Add(3);
                            }
                            else if (value.Contains("4w.png"))
                            {
                                cells.Add(4);
                            }
                            else if (value.Contains("5w.png"))
                            {
                                cells.Add(5);
                            }
                            else if (value.Contains("6w.png"))
                            {
                                cells.Add(6);
                            }
                            else if (value.Contains("7w.png"))
                            {
                                cells.Add(7);
                            }
                            else
                            {
                                cells.Add(8);
                            }

                            break;
                        }
                    }
                }
            }

            return cells;
        }

        private AbstractBridgeModel? Model { get; set; }

    }
}