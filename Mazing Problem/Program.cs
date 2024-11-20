using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace MazeProblem
{
    class Program
    {
        static readonly int[] rowDir = { -1, 1, 0, 0, -1, -1, 1, 1 };
        static readonly int[] colDir = { 0, 0, -1, 1, -1, 1, -1, 1 };
        static void Main(string[] args)
        {
            int[,] maze = //dataset
            {
                {1, 0, 1, 1, 1, 1, 1, 1 },
                {1, 0, 0, 0, 0, 0, 0, 1 },
                {1, 1, 1, 1, 1, 1, 0, 1 },
                {1, 0, 0, 0, 0, 0, 0, 1 },
                {1, 0, 1, 1, 1, 1, 1, 1 },
                {1, 0, 0, 0, 0, 0, 0, 1 },
                {1, 1, 1, 1, 1, 1, 0, 1 },
                {1, 0, 0, 0, 0, 0, 0, 1 },
                {1, 0, 1, 1, 1, 1, 1, 1 },
                {1, 0, 0, 0, 0, 0, 0, 0 }, //this is the exit
                {1, 1 ,1, 1, 1, 1, 1, 1 }, //also this is the second graph
            };
            /*
             int[,] maze2 = 
            {
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                {},
                {},
                {},
                {},
                {},
                {},
                {},
                {},
                {},
                {},
                {}, //exit here
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            }; //first graph
             */
            (int, int) entry = (0, 1);
            (int, int) exit = (8, -10); //in total 8 colums and 11 rows? but the exit is on 10

            List<List<(int, int)>> allPaths = new List<List<(int, int)>>();
            List<(int, int)> path = new List<(int, int)> ();
            bool[,] visited = new bool[maze.GetLength(0), maze.GetLength(1)]; //boolean of column and rows
            
            FindPaths(maze, entry, exit, path, allPaths, visited); //FUCNCTION CALL

            //output all paths
            Console.WriteLine("All Paths:");
            foreach (var p in allPaths)
            {
                Console.WriteLine(string.Join(" -> ",p));
            }
            //find the shortest path
            if (allPaths.Count > 0)
            {
                var shortestPath = FindShortestPath(allPaths);
                Console.WriteLine("\nFastst Path:");
                Console.WriteLine(string.Join(" -> ", shortestPath));
            }
            else
            {
                Console.WriteLine("No valid paths found.");
            }
        }
        static void FindPaths(int[,] maze, (int, int) current, (int, int) exit, List<(int, int)> path, List<List<(int, int)>> allPaths, bool[,] visited)
        {
            int row = current.Item1;
            int col = current.Item2;

            if (current == exit)
            {
                path.Add(current);
                allPaths.Add(new List<(int, int)>(path));
                path.RemoveAt(path.Count - 1);
                return;
            }

            visited[row, col] = true;
            path.Add(current);

            for (int i = 0; i < 8; i++)
            {
                int newRow = row + rowDir[i];
                int newCol = col + colDir[i];

                if (IsValidMove(maze, newRow, newCol, visited))
                {
                    FindPaths(maze, (newRow, newCol), exit, path, allPaths, visited);
                }
            }

            path.RemoveAt(path.Count - 1);
            visited[row, col] = false;
        }

        static bool IsValidMove(int[,] maze, int row, int col, bool[,] visited)
        {
            return row >= 0 && row < maze.GetLength(0) &&
                   col >= 0 && col < maze.GetLength(1) &&
                   maze[row, col] == 0 && !visited[row, col];
        }
        static List<(int, int)> FindShortestPath(List<List<(int, int)>> allPaths)
        {
            List<(int, int)> shortestPath = allPaths[0];

            foreach (var path in allPaths)
            {
                if (path.Count < shortestPath.Count)
                {
                    shortestPath = path;
                }
            }

            return shortestPath;
        }
    }

}
