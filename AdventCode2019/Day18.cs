using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System;
using System.Text;

namespace AdventCode2019
{
    [TestClass]
    public class Day18
    {
        string[] maze = Utils.StringsFromFile("day18.txt").ToArray();

        [TestMethod]
        public void TestA()
        {
            var maze = new string[]
            {
               "#################",
               "#i.G..c...e..H.p#",
               "########.########",
               "#j.A..b...f..D.o#",
               "########@########",
               "#k.E..a...g..B.n#",
               "########.########",
               "#l.F..d...h..C.m#",
               "#################",
            };

            long result = SolveMaze(maze);

            Assert.AreEqual(result, 136);
        }

        [TestMethod]
        public void TestA2()
        {
            var maze = new string[]
            {
               "#################",
               "#i.G..c...e..H.p#",
               "########.########",
               "#j.A..b...f..D.o#",
               "#######.@.#######",
               "#k.E..a...g..B.n#",
               "########.########",
               "#l.F..d...h..C.m#",
               "#################",
            };

            long result = SolveMaze(maze);

            Assert.AreEqual(result, 132);
        }

        [TestMethod]
        public void TestB()
        {
            var maze = new string[]
            {
               "########################",
               "#...............b.C.D.f#",
               "#.######################",
               "#.....@.a.B.c.d.A.e.F.g#",
               "########################",
            };

            long result = SolveMaze(maze);

            Assert.AreEqual(result, 132);
        }

        [TestMethod]
        public void TestC()
        {
            var maze = new string[]
            {
               "########################",
               "#@..............ac.GI.b#",
               "###d#e#f################",
               "###A#B#C################",
               "###g#h#i################",
               "########################",
            };

            long result = SolveMaze(maze);

            Assert.AreEqual(result, 81);
        }

        [TestMethod]
        public void TestD()
        {
            var maze = new string[]
            {
   "#################################################################################",
   "#.#...#.........#i........U.........#...#.........#.......#.............#.....E.#",
   "#.#.#.#.###.###Q#########.#########I#X#####.#####.#####.#.#.#######.###.#.#####.#",
   "#...#..u..#...#.#.........#l#...D.#.#...#...#...#.......#.#.#.......#.#.#.#...#.#",
   "#############.#.#.#########.#.###.#.###.#.###.#.#.#######.###.#######.#.#.#B#.#.#",
   "#...A.......#.#...#.#.....#...#...#...#.#.#.#.#...#...#......p#.......#.#.#.#.#.#",
   "#.#########.#.#####.#.#.#S#.###.#####.#.#.#.#.#####.#.#########.###.###.###.#.#.#",
   "#.#.........#...#...#.#.#.#...#.....#.#.#.#...#.#...#.......#.....#.#...#...#...#",
   "#.#########.#.#.###.#.#.#####.###.#.#.#.#.#.###.#.#########.#####.#.#.###.#####.#",
   "#.#.......#.#q#...#...#.#.....#.#.#.#...#.#.....#.#.....#.#.K.#...#.......#...#.#",
   "#.#.#####.#.#.###W#####.#.#####.#.#.###.#.#######.###.#.#.###.#.###########.#.#.#",
   "#...#...#.#y#...#.....#.#...L...#.#d#...#.....#...#...#.....#.#.#...#...#...#...#",
   "#####.#.#.#.#########.#.#######.#.###.#######.#.###.#########.###.#.#.#.#.#######",
   "#.....#...#.#.........#.N.....#n#...#x#.#.....#.#.....#.......#...#.#.#.#f#..o..#",
   "#.#########.#.#########.###.###.###.#.#.#.###.#.#.###.#.#######.###.#.###.#.###.#",
   "#...#.#.....#...#.....#.#.#...#.#.#.....#..t#.#.#...#.#...#...#...#.#.#...#...#.#",
   "###O#.#.#####.#.#.#.###.#.###.#.#.#####.###.###.#####.###.#.#.#.###.#.#.#####.#.#",
   "#...#...#.....#.#.#.#..s#...#.....#..v#.#...#...........#.#.#...#...#.......#.#.#",
   "#.###.#########.#.#.#.###.#.#####.#.#.###.###.###########.#.#####.#########.###.#",
   "#.#..g..........#.#.......#.#...#.#.#...#.#...#.#.......#.......#.#...#.....#...#",
   "#.#.#############.#########.#.#.###.###.#.#.###.#.#####.#.#######.#.#.#.#####.#.#",
   "#.#.#...........#.#.#.#....z#.#.....#...#.#.....#...#.#.#.#.#...#.#.#.#.#.....#.#",
   "#.###.#########.#.#.#.#######.#######.###.#####.###.#.#.#.#.###.#.#.#.#.#####.#.#",
   "#.....#...#.....#...#.........#.....#...#.....#...#...#.#.....#.#...#.#.#.....#.#",
   "#.#####.#.#.#######.###########.#######.#.#######.###.#.###.###.#####.#.#.#####.#",
   "#.....#.#.#...#.Z.#.#....j..#.........#.#...#.......#.#.#...#...#.....#.#.#...#.#",
   "#######.#.###.#.###.#.#####.#.#####.###.#.#.#.#######.#.#####.###.#####.#.###.#.#",
   "#.......#.#...#.....#.#...#.#...#.#.....#.#.#.#.#.....#...#...#...#.....#...#.#.#",
   "#.#######.#.#.#####.#.###.#.###.#.#######.#.#.#.#.#######.#.###.#.###.#####.#.#.#",
   "#.#.#.....#.#.#...#.#...#.#...#...#.#...#.#...#.#.....#.#...#...#...#.......#...#",
   "#.#.#.#####.#.#.#.#####.#.###.###.#.#.#.#.#####.#####.#.#####.#####.#########.###",
   "#.#...#.....#.#.#.#...#...#.#.#...#...#.#.#...#.....#...#.#...#...#.....#...#.#.#",
   "#.###.###.#.###.#.#.#.###.#.#.#.#######.#.#.#.#####.###.#.#.###.#.#####.###.#.#.#",
   "#...#...#.#.#...#.#.#...#.#.#.#.#.......#...#.........#.#...#...#.#...#...#.#...#",
   "#.#.###.#.#.#.###.#.###.#.#.#.#.###.###.###########.###.#.###.###.###.###.#.###.#",
   "#.#...#.#.#.#...#...#...#.#.#.#.....#...#...#.....#.#...#.....#.....#.#...#..h#.#",
   "#.###.#.#.#####.#####.#.#.#.#.#######.###.#.#####.#.#.#############.#.#.###.#.#.#",
   "#...#.#.F.#...#.....#.#.#.#.#.#...#...#.#.#.......#.#.#...#...#...#...#.#.#.#...#",
   "###.#.#####.#.#####.#.###.#.#.#.#.#.###.#.#########.#.#.#.#.#.#.#.#####.#.#.#####",
   "#...#.......#.......#.......#...#..................r#...#...#...#.......#.......#",
   "#######################################.@.#######################################",
   "#...........#.............#.#.....#...........#...........#...#.....#.....#.....#",
   "#########.#.#.###########.#.#.#.###.#.#.#.#.###.#####.#####.#.#####.#.###.#.#.###",
   "#...#.....#.#...#.......#.#.#.#.....#.#.#.#.....#.....#.....#.#...#....w#...#...#",
   "#.#.#.#####.###.#####.#.#.#.#.#####.#.###.#######.###.#.#####.#.#.#####.#######.#",
   "#.#.H.#...#...#.#...#.#.#.#...#...#.#...#.#.....#.#...#.....#...#...#...#.....#.#",
   "#.#.###.#.#####.#.#.#.###.#.###.#.###.#.#.###.#.#.#####.#######.###.#####.###.#.#",
   "#.#.#...#.......#.#...#...#.#...#...#.#.#...#.#.#.......#.....#.#...#.....#.#a#.#",
   "#.#.#.###########.#####.###R#.#####.###.###.###.#########.###.#.#.###.#####.#.#.#",
   "#.#.#.#.....#.....#...#.#...#.#...#...#.#.#...#.....#.....#...#.#.#.......#...#.#",
   "#.#.#.#.###.#.#.###.#.#.#####.#.#####.#.#.###.#.#.#.#.#####.#####.#.#####.#.###.#",
   "#.#.#.#...#...#.....#.#...#...#.....#...#.#...#.#.#.#.#...........#.#.....#...#.#",
   "#.#.#.###.###########.###.#.###.###.###.#.#.###.#.###.#############.#.#######.#Y#",
   "#.#.#.#.#.#.........#...#.#...#.#.#...#.#.#...#.#.#...#...#.#.....#.#.....#...#.#",
   "#.#.#.#.#.#########.#.###.###.#.#.#.#.#.#.###.###.#.###.#.#.#.#.###.#####.#.###.#",
   "#.#.#.#...#.........#.........#...#.#.#.#...#.....#.....#.#...#.........#.#.#...#",
   "#.###.#.###.#.#######.###########.#.###.#.#######.#######.#.#############.#.#.#.#",
   "#...#.#.#...#.#.....#.#.....#...J.#.....#...#...#...#.....#.#...#...#.....#...#.#",
   "###.#.#.###.#.#.###.###.###.#.###########.#.#.#.###.#.#####.#.#.###.#.#########.#",
   "#...#.#.....#.#...#.....#...#.#...#.....#.#.#.#.....#.#.......#.....#.....#.....#",
   "#.###.###.#######.#######.###.#.###.#.#####.#.#######.###.#########.#####.#.#####",
   "#...#.#.#.#.......#.#.....#.#.#.....#...#.V.#.....#.#...#...#.#...#...#...#.....#",
   "###.#.#.#.#.#######.#######.#.#########.#.#######.#.###.###.#.#.#####.#.#######.#",
   "#...#.#...#.#.......#.......#.......#...#.......#.#...#...#.#.#...#...#.#...#...#",
   "#.###.#####.#.#####.#.#.###.#######.#.#.#.#.###.#.#.#.###.###.###.#.###.#.#G#.###",
   "#...#.....#...#...#.#.#.#.#.#.....#...#.#.#.#...#.#.#b..#...#...#.#.#c..#.#.....#",
   "#.#.#####.#####.#.#.#.#.#.#.###.#######.#.#.#####.#####.###.###.#.#.#.###########",
   "#.#.C...#.......#...#.#.#.......#...#...#.#.#...#.#.....#.#.#...#...#...#......m#",
   "#.#####.#############.#.###.#####.#.#.###.#.#.#T#.#.###.#.#.#.#####.###.#.#####.#",
   "#...#.#.......#.....#.#...#.#.....#...#.#.#.#.#...#.#...#.#.#.....#...#.....#...#",
   "###.#.#####.###.#.#.#.###.#.#.#########.#.#.#.#####.#.###.#.#####.###.#######.#.#",
   "#.#...#...#.#...#.#.#.#...#.#.....#.....#.#...#.....#.#...#.....#.#...#.#...#.#.#",
   "#.###.#.###M#.###.###.#.#########.###.#.#.#####.#.###.#.#.#.#####.###.#.#.#.#.#.#",
   "#.....#...#...#...#...#...#.......#...#.#.#...#.#...#.#.#.#.....#...#...#.#...#.#",
   "#.#######.#####.#.#.#####.#.#######.###.#.#.#.#####.#.#.#######.###.#####.#####.#",
   "#.#.........#...#.#.#.#...#.#.......#...#k#.#.......#.#.....#...#...#.....#.....#",
   "#.#.#######.#.###.#.#.#.###.###.#####.###.#.#########.#####.#.###.###.#####.#####",
   "#...#...#...#...#.#...#...#...#...#.#.#.#...#.#.....#.#e....#.....#...#...#.#...#",
   "#####.#.#.#####.#####.###P###.###.#.#.#.#####.#.#.###.#.###.#######.###.###.#.#.#",
   "#.....#.......#.........#.........#.....#.......#.......#...........#.........#.#",
   "#################################################################################",
            };

            long result = SolveMaze(maze);

            Assert.AreEqual(result, 2946);
        }

        [TestMethod]
        public void Problem1()
        {
            //FillMaze(ref maze);
            //Paint(maze);

            long result = SolveMaze(maze);

            Assert.AreEqual(result, 5392);
        }

        [TestMethod]
        public void Problem2()
        {
            string[] updated = new[] { "@#@", "###", "@#@" };
            for (int i = 0; i < 3; i++)
            {
                maze[39 + i] = maze[39 + i].Substring(0, 39) + updated[i] + maze[39+i].Substring(42);
            }

            var result = SolveMaze(maze);
            Assert.AreEqual(result, 1684);
        }

        private long SolveMaze(string[] maze)
        {
            // Get coordinates of all non-walls and non-floors
            var things = FindThings(maze);

            var startNodes = things.Where(t => t.Thing == '@').ToList();
            
            things = things.Except(startNodes).ToList();

            
            foreach (var startNode in startNodes)
            {
                // BFS the maze, linking nodes and setting blockers as go.
                LinkNodesBFS(startNode, maze, things);
            }

            things.RemoveAll(n => n.Thing < 'a');

            var groupedThings = things.GroupBy(t => t.TopMost);
            var measurements = new Dictionary<Node, Dictionary<(Node, Node), int>>();
            foreach (var group in groupedThings)
            {
                // Work out distances between all nodes 
                measurements[group.Key] = CalculateDistances(group.Key, group.ToList(), maze);
            }

            // Solve problem
            return SolveBFS(groupedThings, measurements);
        }

        public long SolveBFS(IEnumerable<IGrouping<Node, Node>> groupedThings, Dictionary<Node, Dictionary<(Node, Node), int>> measurements)
        {
            IEnumerable<Node> GetOpen(Node node, IEnumerable<Node> things, string visited) =>
                    things.Where(t => !visited.Contains(t.Thing) && !t.Blockers.Except(visited).Any());

            var targetLength = groupedThings.SelectMany(g => g).Distinct().Count();
            int minResult = int.MaxValue;
            string result = string.Empty;

            var explored = new Dictionary<(string, string), int>();
            var searches = new Stack<(Dictionary<Node, Node> nodes, string visited, int distance)>();

            var keys = groupedThings.Select(g => g.Key).ToArray();

            searches.Push((keys.ToDictionary(k => k, v => v), "", 0));

            while (searches.Count > 0)
            {
                (var currents, string currentVisited, int currentDistance) = searches.Pop();
                
                foreach (var group in groupedThings)
                {
                    var current = currents[group.Key];
                    foreach (var node in GetOpen(current, group, currentVisited))
                    {
                        var visiting = currents.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                        visiting[group.Key] = node;

                        var visited = currentVisited + node.Thing;
                        var distance = measurements[group.Key][(current, node)] + currentDistance;
                        var currentString = string.Concat(visiting.Values.Select(t => t.Thing));
                        var exploredKey = (currentString, String.Concat(currentVisited.OrderBy(c => c)));

                        if (distance < minResult)
                        {
                            if (visited.Length == targetLength)
                            {
                                result = visited;
                                minResult = distance;
                            }
                            else if (!explored.TryGetValue(exploredKey, out int previousDistance) || previousDistance > distance)
                            {
                                explored[exploredKey] = distance;

                                searches.Push((visiting, visited, distance));
                            }
                        }
                    }
                }
            }

            Debug.WriteLine($"{result} took {minResult}");
            return minResult;
        }

        private List<Node> FindThings(string[] maze)
            => maze.SelectMany((row, y) => row.Select((c, x) => new Node { X = x, Y = y, Thing = c }))
                                              .Where(n => n.Thing >= '@').ToList();

        [DebuggerDisplay("{Thing.ToString()} @ ({X}, {Y}) [{Distance}] via {Parent?.Thing.ToString() ?? \" \"} + {Blockers}")]
        public class Node
        {
            public int X;
            public int Y;

            public char Thing;

            public string Blockers = ""; // things that block between this node and it's parent

            public int Distance;

            public Node Parent;

            public override string ToString() => $"{Thing.ToString()} @ ({X}, {Y}) [{Distance}]";

            public (int x, int y) Location => (X, Y);

            public Node TopMost => Parent?.TopMost ?? this;
        }

        private void LinkNodesBFS(Node start, string[] maze, IEnumerable<Node> things)
        {
            var open = new List<((int x, int y) location, Node parent, String blockers)> { (start.Location, start, "") };
            var visited = new List<(int x, int y)>();
            int distance = 0;

            while (open.Any())
            {
                distance++;

                open = open.SelectMany(o => GetAdjacentLocations(maze, o.location).Select(l => (l, o.parent, o.blockers))).Distinct().ToList();

                for (int i = open.Count() - 1; i >= 0; i--)
                {
                    var (location, parent, blockers) = open[i];
                    if (visited.Contains(location))
                    {
                        open.RemoveAt(i);
                        continue;
                    }

                    var node = things.FirstOrDefault(t => t.Location == location);
                    if (node != null)
                    {
                        //Debug.WriteLine($"{node.Thing} at {location} has parent {parent.Thing} at distance {distance}");

                        node.Distance = distance;

                        if (node.Thing >= 'a')
                        {
                            node.Parent = parent;
                            node.Blockers = blockers;
                            blockers += node.Thing; // A key will 'block' subsequent keys
                        }
                        else
                        {
                            blockers += node.Thing;
                            node = parent; // don't replace the parent
                        }

                        open[i] = (location, node, String.Concat(blockers.ToLower().OrderBy(c => c).Distinct())); // all subsequent items will use this new parent
                    }

                    visited.Add(location);
                }
            }
        }

        private Dictionary<(Node, Node), int> CalculateDistances(Node start, IList<Node> nodes, string[] maze)
        {
            var distances = new Dictionary<(Node, Node), int>();

            for (int i = 0; i < nodes.Count(); i++)
            {
                // BFS until find other nodes on list
                var node = nodes[i];
                var location = node.Location;

                // add root connections
                distances[(node, start)] = node.Distance;
                distances[(start, node)] = node.Distance;

                var open = new List<(int x, int y)> { location };
                var visited = new List<(int x, int y)> { node.Location };
                int distance = 0;

                while (open.Any())
                {
                    distance++;

                    open = open.SelectMany(o => GetAdjacentLocations(maze, o)).Distinct().Except(visited).ToList();
                    visited.AddRange(open);

                    var found = nodes.Where(n => open.Contains(n.Location));

                    foreach (var f in found)
                    {
                        distances[(node, f)] = distance;
                        distances[(f, node)] = distance;
                    }
                }
            }

            return distances;
        }

        private IEnumerable<(int x, int y)> GetAdjacentLocations(string[] maze, (int x, int y) location, (int x, int y)? previous = null)
        {
            var up = GetOrCreateLocation(maze, location.x, location.y - 1, previous); // UP
            var down = GetOrCreateLocation(maze, location.x, location.y + 1, previous); // DOWN
            var left = GetOrCreateLocation(maze, location.x - 1, location.y, previous); // LEFT
            var right = GetOrCreateLocation(maze, location.x + 1, location.y, previous); // RIGHT

            if (up.HasValue) yield return up.Value;
            if (down.HasValue) yield return down.Value;
            if (left.HasValue) yield return left.Value;
            if (right.HasValue) yield return right.Value;
        }

        private (int x, int y)? GetOrCreateLocation(string[] maze, int x, int y, (int x, int y)? previous = null)
        {
            if (x >= 0 && x < maze[0].Length && y >= 0 && y < maze.Length)
            {
                if (!previous.HasValue || previous?.x != x || previous?.y != y)
                {
                    if (maze[y][x] != '#')
                    {
                        return (x, y);
                    }
                }
            }

            return null;
        }

        string[] Paint(string[] maze)
        {
            StringBuilder[] image = maze.Select(m => new StringBuilder(m)).ToArray();

            char[] pixels = { ':', '·', '#', 'O' };


            foreach (var line in image)
            {
                Debug.WriteLine(line);
            }

            Debug.WriteLine("");

            return image.Select(i => i.ToString()).ToArray();
        }


        private void FillMaze(ref string[] maze)
        {
            StringBuilder[] image = maze.Select(m => new StringBuilder(m)).ToArray();

            IEnumerable<(int x, int y)> FindDeadEnds()
            {
                for (int y = 1; y < image.Length - 1; y++)
                {
                    var row = image[y];
                    for (int x = 1; x < row.Length - 1; x++)
                    {
                        if (image[y][x] == '.')
                        {
                            int count = 0;
                            count += image[y - 1][x] == '#' ? 1 : 0;
                            count += image[y + 1][x] == '#' ? 1 : 0;
                            count += image[y][x - 1] == '#' ? 1 : 0;
                            count += image[y][x + 1] == '#' ? 1 : 0;

                            if (count >= 3) yield return (x, y);
                        }
                    }
                }
            }

            var deadEnds = FindDeadEnds();
            while (deadEnds.Any())
            {
                foreach (var (x, y) in deadEnds)
                {
                    image[y][x] = '#';
                }
            }

            maze = image.Select(i => i.ToString()).ToArray();
        }
    }
}
