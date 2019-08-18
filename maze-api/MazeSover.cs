
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace syedtakbar_maze_api
{
    public class MazeSolverImpl: IMazeSolver
    {  
         //public Tuple<int, string> solveMaze(string [] mazeInput)
         public string solveMaze(string [] mazeInput)
         {
                                              
            var resolver = new MazeSolver(mazeInput, mazeInput.Length, mazeInput[0].Length);
            
            
            bool[,] grid = resolver.CreateGrid();
            int startPosition = resolver.Find('A') ;
            int endPosition = resolver.Find('B');
            
            //Console.WriteLine($"startPosition: {startPosition} Goal: {endPosition} NodeCount: {resolver.NODES} row: {resolver.ROWS} columns {resolver.COLS} grid.Length: {grid.Length} ");
    
            Tuple<int, IEnumerable<int>> aStarPath = new AStarAlgo().Find (
                                                                        (c) => Enumerable.Range(0, resolver.NODES).Where((n) => grid[c, n]), 
                                                                        (c, n) => (grid[c, n] ? 1 : int.MaxValue), 
                                                                        resolver.ManhattanDistance, 
                                                                        startPosition, endPosition, resolver.NODES);
                
        
            string[] solvedMaze = new string[resolver.ROWS];
            for(int i = 0; i < resolver.ROWS; ++i)
                solvedMaze[i] = resolver.MAP[i];            
            
            foreach(int node in aStarPath.Item2)            
                if (solvedMaze[node / resolver.COLS][node % resolver.COLS] == '.')
                    solvedMaze[node / resolver.COLS] = solvedMaze[node / resolver.COLS].Remove(node % resolver.COLS, 1).Insert(node % resolver.COLS, "@");
                              
            var numberOfSteps = aStarPath.Item1;
            var mazeResult = string.Join("\r\n", solvedMaze);

            
            //Console.WriteLine($"return value numberOfSteps: {numberOfSteps} and solvedMaze : {Environment.NewLine}{mazeResult}");
            //return new Tuple<int, string>(numberOfSteps,mazeResult);            
            return   $"{{{Environment.NewLine}steps:{numberOfSteps},{Environment.NewLine}solution:{Environment.NewLine}{mazeResult}{Environment.NewLine}}}";
                     
         } 
    }                 

    public class MazeSolver
    {  
        public string [] MAP { get; set; }
        public int ROWS { get; }
        public int COLS { get; }
        public int NODES { get; }
        
        public MazeSolver (string [] map, int rows, int columns) 
        {
            MAP = map;
            ROWS = rows;
            COLS = columns;
            NODES = rows * columns;
        }

        public bool[,] CreateGrid()
        {
            bool[,] grid = new bool[NODES, NODES];
            for(int r = 0; r < ROWS; ++r)
            {
                for(int c = 0; c < COLS; ++c)
                {
                    if (MAP[r][c] == '#')
                        continue;
                
                    int index = ToNode(r, c);
                    if (r > 0 && MAP[r - 1][c] != '#')
                        grid[index, ToNode(r - 1, c)] = true;
                    if (r < (ROWS - 1) && MAP[r + 1][c] != '#')
                        grid[index, ToNode(r + 1, c)] = true;
                    if (c > 0 && MAP[r][c - 1] != '#')
                        grid[index, ToNode(r, c - 1)] = true;
                    if (c < (COLS - 1) && MAP[r][c + 1] != '#')
                        grid[index, ToNode(r, c + 1)] = true;
                }
            }
            
            return grid;
        }
                        
        public int ToNode(int row, int col) {
            return row * COLS + col;
        }
        
        public int Find(char s) {
            for(int r = 0; r < ROWS; ++r)
                for(int c = 0; c < COLS; ++c)
                    if (MAP[r][c] == s)
                    {                        
                        return ToNode(r, c);
                    }   
            return -1;
        }
        
        public int ManhattanDistance(int start, int end) {
            int startRow = start / COLS;
            int startCol = start % COLS;
            int endRow = end / COLS;
            int endCol = end % COLS;            
            
            return Math.Abs(startRow - endRow) + Math.Abs(startCol - endCol);
        }
    }


    public class DistanceComparer : IComparer<int>
    {
        private readonly int target;
        private readonly Func<int, int, int> distanceHeuristic;
        
        public DistanceComparer(int target, Func<int, int, int> distanceHeuristic)
        {
            this.target = target;
            this.distanceHeuristic = distanceHeuristic;
        }

        public int Compare(int left, int right)
        {
            int distance = this.distanceHeuristic(left, target) - this.distanceHeuristic(right, target);            
            return (distance != 0) ? distance : left - right;
        }
    }

    public class AStarAlgo
    {        
        private static readonly int NOT_TRAVERSED = -1;
        public Tuple<int, IEnumerable<int>> Find (
                                                        Func<int, IEnumerable<int>> getNeighbors,
                                                        Func<int, int, int> getDistance,
                                                        Func<int, int, int> distanceHeuristic,
                                                        int start, int goal, int nodeCount)
        {
            BitArray closed = new BitArray(nodeCount);
            SortedSet<int> open = new SortedSet<int>(new DistanceComparer(goal, distanceHeuristic));
            int[] previous = new int[nodeCount];
            int[] score = new int[nodeCount];

            for(int node = 0; node < nodeCount; ++node)
            {
                previous[node] = NOT_TRAVERSED;
                score[node] = int.MaxValue;
            }

            score[start] = 0;                        
            open.Add(start);
            
            while(open.Count > 0)
            {
                int current = open.First();                
                if (current == goal)    
                    return new Tuple<int, IEnumerable<int>>(score[current], ReconstructPath(current, previous));
                
                open.Remove(current);
                closed[current] = true;
                foreach(int neighbor in getNeighbors(current))
                {
                    if (closed[neighbor]) continue;

                    int tentativeScore = score[current] + getDistance(current, neighbor);
                    if (!open.Contains(neighbor) || tentativeScore < score[neighbor])
                    {
                        previous[neighbor] = current;
                        score[neighbor] = tentativeScore;
                        open.Add(neighbor);
                    }
                }
            }
            return new Tuple<int, IEnumerable<int>>(-1, Enumerable.Empty<int>());;
        }
        private IEnumerable<int> ReconstructPath(int current, int[] previous)
        {
            yield return current;
            while(previous[current] != NOT_TRAVERSED) {
                current = previous[current];
                yield return current;
            }
        }
    }

}