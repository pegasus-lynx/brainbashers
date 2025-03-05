using BridgeBot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BridgeBot
{
    public class BridgePuzzle
    {
        public BridgePuzzle(string dateString, int size, Difficulty difficulty)
        {
            DateString = dateString;
            Size = size;
            Difficulty = difficulty;
        }

        public BridgePuzzle(int[,] grid, int size)
        {
            Size = size;
            Grid = grid;
            OriginalGrid = grid.Clone() as int[,];
        }


        public List<Tuple<Cell?, int>> CurrentCellEdges(Cell cell)
        {
            ThrowIfVerifyBoundsFails(cell);
            VerifyPuzzleState();
            
            List<Tuple<Cell?, int>> cellEdges = [];
            List<Cell?> neighbors = GetCurrentCellNeighbors(cell);
        

            foreach(DirectionUnit unit in Enum.GetValues(typeof(DirectionUnit)))
            {
                Cell? neighbor = neighbors[(int)unit];

                if(neighbor is null)
                {
                    cellEdges.Add(new Tuple<Cell?, int>(null, 0));
                    continue;
                }

                Cell move = Direction.FromUnit(unit);
                Cell nc = cell + move;

                // Need to figure out a way to identify negative values with the type of edge.
                // One way can be( -1, -2 for vertical and -3, -4 for horizontal )
                int val = Grid![nc.X, nc.Y];
                if (val > 0)
                    throw new Exception("Not prepared for adjacent cells");

                if(val == 0)
                {
                    // No edges
                    cellEdges.Add(new Tuple<Cell?, int>(neighbor, 0));
                    continue;
                }
                else
                {
                    // There are edges
                    if(unit == DirectionUnit.Up || unit == DirectionUnit.Down)
                    {
                        cellEdges.Add(new Tuple<Cell?, int>(neighbor, Math.Abs(val)));
                    }
                    else
                    {
                        cellEdges.Add(new Tuple<Cell?, int>(neighbor, Math.Abs(val+2)));
                    }
                }
            }

            return cellEdges;
        }

        public int MaxCellEdges(Cell cell)
        {
            ThrowIfVerifyBoundsFails(cell);
            VerifyPuzzleState();
            return Math.Max(Grid![cell.X, cell.Y], 0);
        }

        public int CellEdgesCount(Cell cell)
        {
            List<Tuple<Cell?, int>> cellEdges = CurrentCellEdges(cell);
            int totalEdges = cellEdges.Sum(t => t.Item2);
            return totalEdges;
        }

        public List<Tuple<Cell?, int>> RemainingCellEdges(Cell cell)
        {
            ThrowIfVerifyBoundsFails(cell);
            VerifyPuzzleState();

            List<Tuple<Cell?, int>> cellEdges = CurrentCellEdges(cell);
            List<Tuple<Cell?, int>> remainingEdges = new();

            foreach(var cellEdgePair in cellEdges)
            {
                Cell? neighbor = cellEdgePair.Item1;
                if (neighbor is null)
                {
                    remainingEdges.Add(cellEdgePair);
                }
                else
                {
                    int maxRemainingEdges = MaxCellEdges(neighbor) - CellEdgesCount(neighbor);
                    remainingEdges.Add(new Tuple<Cell?, int>(cellEdgePair.Item1, Math.Min(2, maxRemainingEdges)));

                }
            }

            return remainingEdges;
        }

        public List<Cell?> GetAllCellNeighbors(Cell cell)
        {
            ThrowIfVerifyBoundsFails(cell);
            VerifyPuzzleState();

            List<Cell?> allNeighbors = new();
            foreach(Cell move in Direction.DirectionsList)
            {
                Cell tc = new Cell(cell);
                tc = tc + move;
                while(VerifyBounds(tc))
                {
                    if (Grid![tc.X, tc.Y] > 0)
                        break;

                    tc = tc + move;
                }

                allNeighbors.Add(VerifyBounds(tc) ? tc : null);
            }

            return allNeighbors;
        }

        public List<Cell?> GetCurrentCellNeighbors(Cell cell)
        {
            ThrowIfVerifyBoundsFails(cell);
            VerifyPuzzleState();

            List<Cell?> currentNeighbors = new();
            foreach (DirectionUnit unit in Enum.GetValues(typeof(DirectionUnit)))
            {
                Cell move = Direction.FromUnit(unit);

                Cell tc = cell + move;

                bool addNeighbor = false;
                while (VerifyBounds(tc))
                {
                    int val = Grid![tc.X, tc.Y];

                    if(val < 0)
                    {
                        if(unit == DirectionUnit.Up || unit == DirectionUnit.Down)
                        {
                            if (val < -2)
                                break;
                        }
                        else
                        {
                            if (val > -3)
                                break;
                        }
                    }

                    if (val > 0)
                    {
                        addNeighbor = true;
                        break;
                    }

                    tc = tc + move;
                }

                currentNeighbors.Add(addNeighbor ? tc : null);
            }

            return currentNeighbors;
        }

        public int CountEdges(Cell c1, Cell c2)
        {
            ThrowIfVerifyBoundsFails(c1);
            ThrowIfVerifyBoundsFails(c2);
            VerifyPuzzleState();

            DirectionUnit? unit = Direction.UnitFromCells(c1, c2);
            if (unit is null)
                return 0;

            Cell move = Direction.FromUnit((DirectionUnit)unit);
            Cell tc = c1 + move;

            int val = Grid![tc.X, tc.Y];
            if (val == 0)
                return 0;

            if (val > 0)
                throw new Exception("This should not have happened. Oopsies !!");

            if(unit == DirectionUnit.Up || unit == DirectionUnit.Down)
            {
                if (val < -2)
                    return 0;

                return val == -1 ? 1 : 2;
            }
            else
            {
                if (val > -3)
                    return 0;

                return val == -3 ? 1 : 2;
            }
        }

        public void MakeEdges(Cell c1, Cell c2, int count)
        {
            ThrowIfVerifyBoundsFails(c1);
            ThrowIfVerifyBoundsFails(c2);
            VerifyPuzzleState();

            DirectionUnit? unit = Direction.UnitFromCells(c1, c2);
            if (unit is null)
                return;

            int edges = CountEdges(c1, c2);

            if (edges + count > 2)
                throw new Exception("Cannot create more than two edges between two cells");

            int val = 0;
            if (unit == DirectionUnit.Down || unit == DirectionUnit.Up)
            {
                val = edges+count == 1 ? -1 : -2;
            }
            else
            {
                val = edges+count == 1 ? -3 : -4;
            }

            Cell move = Direction.FromUnit((DirectionUnit)unit);

            Cell tc = c1 + move;
            while(tc != c2)
            {
                Grid![tc.X, tc.Y] = val;
                tc = tc + move;
            }
        }


        private bool VerifyBounds(Cell cell)
        {
            return cell.X >= 0 && cell.Y >= 0 && cell.X < Size && cell.Y < Size;
        }

        public void VerifyPuzzleState()
        {
            if (Grid is null)
                throw new Exception("Cannot perform operation as Grid is null");
        }

        private void ThrowIfVerifyBoundsFails(Cell cell)
        {
            if (!VerifyBounds(cell))
                throw new Exception($"Cell {cell.X}, {cell.Y} is out out bounds. Grid.Size is {Size}");
        }

        #region Load Helper Methods

        public async void Load()
        {
            string url = GetPuzzleUrl();
            string content = await GetPuzzlePageContent(url);
            CreatePuzzle(content);
        }
        
        public static BridgePuzzle LoadFromFile(string filePath)
        {
            string content = File.ReadAllText(filePath);
            List<int> cells = ProcessFileContent(content);

            int size = (int)Math.Sqrt((double)(cells.Count));

            Debug.Assert(cells.Count == (size * size), $"Not a perfect square. Cells = {cells.Count}, Size = {size}");
            int[,] grid = new int[size, size];
            for(int k=0;k<cells.Count;k++)
            {
                grid[k / size, k % size] = cells[k];
            }

            return new BridgePuzzle(grid, size);
        }

        private static List<int> ProcessFileContent(string content)
        {
            List<int> cells = new();
            foreach(string part in content.Split('<'))
            {
                if(part.StartsWith("img"))
                {
                    foreach(string attr in part.Split(" "))
                    {
                        if(attr.Contains("src"))
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
                            else if(value.Contains("5w.png"))
                            {
                                cells.Add(5);
                            }
                            else if(value.Contains("6w.png"))
                            {
                                cells.Add(6);
                            }
                            else if(value.Contains("7w.png"))
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

        private static async Task<string> GetPuzzlePageContent(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }
        
        private string GetPuzzleUrl()
        {
            return $"https://www.brainbashers.com/showbridges.asp?" +
                        $"date={DateString}&size={Size}&diff={Difficulty}";
        }
        
        private void CreatePuzzle(string content)
        {
            throw new NotImplementedException();
        }

        #endregion
        
        public static void Print(BridgePuzzle puzzle)
        {
            if(puzzle.Grid is null)
            {
                Console.WriteLine("No Grid in puzzle");
                return;
            }

            Console.WriteLine($"Date : {puzzle.DateString?.Substring(2)}/{puzzle.DateString?.Substring(0,2)} , Size : {puzzle.Size} , Difficulty : {puzzle.Difficulty}");
            Console.WriteLine("------------------------------------------------------------------------");
            for(int i=0;i<puzzle.Size;i++)
            {
                for(int j=0;j<puzzle.Size;j++)
                {
                    Console.Write(puzzle.Grid[i, j]);
                    Console.Write(" ");
                }

                Console.Write("\n");
            }
        }

        public static void PrintSolved(BridgePuzzle puzzle)
        {
            if (puzzle.Grid is null)
            {
                Console.WriteLine("No Grid in puzzle");
                return;
            }

            Console.WriteLine($"Date : {puzzle.DateString?.Substring(2)}/{puzzle.DateString?.Substring(0, 2)} , Size : {puzzle.Size} , Difficulty : {puzzle.Difficulty}");
            Console.WriteLine("------------------------------------------------------------------------");
            for (int i = 0; i < puzzle.Size; i++)
            {
                for (int j = 0; j < puzzle.Size; j++)
                {
                    int val = puzzle.Grid[i, j];

                    if (val > 0)
                        Console.Write($" {val}");
                    else if (val == 0)
                        Console.Write("  ");
                    else
                    {
                        if (val == -1)
                            Console.Write(" |");
                        else if (val == -2)
                            Console.Write("||");
                        else if (val == -3)
                            Console.Write("--");
                        else
                            Console.Write("==");
                    }

                    Console.Write(" ");
                }

                Console.Write("\n");
            }
        }

        public string? DateString { get; set; }
        public int Size { get; set; }
        public Difficulty? Difficulty { get; set; }
        public int[,]? Grid { get; private set; }
        public int[,]? OriginalGrid { get; private set; }
    }

}
