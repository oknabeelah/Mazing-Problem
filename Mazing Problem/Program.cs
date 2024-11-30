using System;
using System.Collections.Generic;


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
                 {1, 0, 0, 0, 0, 0, 0, 0 }, //this is the exit, produces AN output...
                 {1, 1 ,1, 1, 1, 1, 1, 1 }, //also this is the second graph
             };

            /* int[,] maze =
            {
                 {1, 0, 1, 1, 1, 1},
                 {1, 1, 0, 1, 1, 1},
                 {1, 1, 1, 0, 1, 1},
                 {1, 1, 1, 1, 0, 1},
                 {1, 1, 1, 1, 1, 0}, //exiit
                 {1, 1, 1, 1, 1, 1}, //test 2! no path found
             };
             */

            /*
            int[,] maze =
           {
                {1, 0, 1, 1, 1, 1},
                {1, 1, 0, 1, 1, 1},
                {1, 1, 0, 0, 1, 1},
                {1, 1, 1, 0, 0, 1},
                {1, 1, 1, 1, 0, 0}, //exiit
                {1, 1, 1, 1, 1, 1}, //test 3 !!no path found
            };*/

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
             
             int[,] maze = 
            {
                {1, 0, 1, 1, 1, 1},
                {1, 1, 0, 1, 1, 1},
                {1, 1, 1, 0, 1, 1},
                {1, 1, 1, 1, 0, 1},
                {1, 1, 1, 1, 1, 0}, //exiit
                {1, 1, 1, 1, 1, 1},
            }

             */

            (int, int) entry = (0, 1);
            (int, int) exit = (9, 7); //(4, 5)in total 8 colums and 11 rows? but the exit is on 10
            // way to automate this
          
            List<List<(int, int)>> allPaths = new List<List<(int, int)>>();
            HashSet<string> uniquePaths = new HashSet<string>();  // To track unique paths
            List<(int, int)> path = new List<(int, int)> ();
            bool[,] visited = new bool[maze.GetLength(0), maze.GetLength(1)]; //boolean of column and rows
            
            FindPaths(maze, entry, exit, path, allPaths, uniquePaths, visited); //FUCNCTION CALL

            //output all paths
            Console.WriteLine("All Paths:");
            
            //find the shortest path
            if (allPaths.Count > 0)
            {
                foreach (var p in allPaths)
                {
                    Console.WriteLine(string.Join(" -> ", p));
                    Console.WriteLine("\nNext Path:\n");
                }

                // find and output shortest path
                var shortestPath = FindShortestPath(allPaths);
                Console.WriteLine("\nFastest Path:");
                Console.WriteLine(string.Join(" -> ", shortestPath));
            }
            else
            {
                Console.WriteLine("No valid paths found.");
            }
        }
        static void FindPaths(int[,] maze, (int, int) current, (int, int) exit, List<(int, int)> path, List<List<(int, int)>> allPaths, HashSet<string> uniquePaths, bool[,] visited)
        {
            int row = current.Item1;
            int col = current.Item2;

            //base : if the current position is the  exit
            if (current == exit)
            {
                path.Add(current);
                string canonicalPath = string.Join(" -> ", path); //change name? 

                //ensure the path is unique before addingg
                if (!uniquePaths.Contains(canonicalPath))
                {
                    allPaths.Add(new List<(int, int)>(path));
                    uniquePaths.Add(canonicalPath);
                }

                path.RemoveAt(path.Count - 1); //backtrack
                return;
            }

           // mark current cell as visited
            visited[row, col] = true;
            path.Add(current);

            for (int i = 0; i < 8; i++) //explore all 8 directions
            {
                int newRow = row + rowDir[i];
                int newCol = col + colDir[i];

                if (IsValidMove(maze, newRow, newCol, visited)) //check if next move is valid
                {
                    FindPaths(maze, (newRow, newCol), exit, path, allPaths, uniquePaths, visited);
                }
            }

            path.RemoveAt(path.Count - 1); //backtrack: unmark the current cell and remove it from the path
            visited[row, col] = false;
        }

        static bool IsValidMove(int[,] maze, int row, int col, bool[,] visited)
        {
            return row >= 0 && row < maze.GetLength(0) &&
                   col >= 0 && col < maze.GetLength(1) &&
                   maze[row, col] == 0 && !visited[row, col]; //confirm logic
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
