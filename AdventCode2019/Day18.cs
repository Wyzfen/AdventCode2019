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
        string [] maze = Utils.StringsFromFile("day18.txt").ToArray();

        [TestMethod]
        public void Problem1()
        {
            var maze = new string[]
            {
               //"#################",
               //"#i.G..c...e..H.p#",
               //"########.########",
               //"#j.A..b...f..D.o#",
               //"########@########",
               //"#k.E..a...g..B.n#",
               //"########.########",
               //"#l.F..d...h..C.m#",
               //"#################",

               //"########################",
               //"#...............b.C.D.f#",
               //"#.######################",
               //"#.....@.a.B.c.d.A.e.F.g#",
               //"########################",

               "########################",
               "#@..............ac.GI.b#",
               "###d#e#f################",
               "###A#B#C################",
               "###g#h#i################",
               "########################",
            };

            var things = FindThings(maze);

            var startNode = things.First(t => t.Thing == '@');

            //// Maze is in quarters; do each quadrant separately.
            //Set(ref maze[startNode.Y], startNode.X - 1, '#');
            //Set(ref maze[startNode.Y], startNode.X + 1, '#');
            //Set(ref maze[startNode.Y - 1], startNode.X, '#');
            //Set(ref maze[startNode.Y + 1], startNode.X, '#');

            //DepthFirst(startNode, (startNode.X - 1, startNode.Y - 1), startNode.Location, 2, "", maze, things); // startNode.Location is basically empty
            //DepthFirst(startNode, (startNode.X + 1, startNode.Y - 1), startNode.Location, 2, "", maze, things); // startNode.Location is basically empty
            //DepthFirst(startNode, (startNode.X - 1, startNode.Y + 1), startNode.Location, 2, "", maze, things); // startNode.Location is basically empty
            //DepthFirst(startNode, (startNode.X + 1, startNode.Y + 1), startNode.Location, 2, "", maze, things); // startNode.Location is basically empty

            LinkThings(startNode, startNode.Location, startNode.Location, 0, "", maze, things);

            var measurements = MeasureThings(things.Where(t => t.Parent == startNode).ToList(), maze);

            // Now now how to get to every node, and the distance from their parent
            var visited = "";
            var visible = things.Where(t => t.Parent == startNode);
            var lookup = visible.ToLookup(t => t.Blockers.Except(visited).Any(), t => t);

            var open = lookup[false];
            var closed = lookup[true];

            long result = long.MaxValue;

            Solve(startNode, open, closed, visited, 0, things, measurements, ref result);

            Assert.AreEqual(result, 136);
        }

        [TestMethod]
        public void Problem2()
        {
            var result = 0L;
            Assert.AreEqual(result, 1143770635);
        }

        public void Solve(Node previous, IEnumerable<Node> open, IEnumerable<Node> closed, string visited, long distance, IEnumerable<Node> things, Dictionary<(Node, Node), int> measurements, ref long minDistance)
        {
            if (closed.Count() == 0 && open.Count() == 0)
            {
                minDistance = Math.Min(minDistance, distance);
                Debug.WriteLine($"{visited} takes {distance}");
            }
            else
            {
                foreach (var node in open)
                {
                    var newDistance = DistanceBetween(node, previous, measurements) + distance;
                    if (newDistance >= minDistance) continue;

                    var children = things.Where(t => t.Thing >= 'a' && t.Parent == node);
                    string newVisited = visited + node.Thing;
                    var lookup = children.Union(closed).ToLookup(t => t.Blockers.Except(newVisited).Any(), t => t);

                    var newOpen = lookup[false].Union(open.Except(new[] { node }));
                    var newClosed = lookup[true];

                    Solve(node, newOpen, newClosed, newVisited, newDistance, things, measurements, ref minDistance);
                }
            }
        }

        private int DistanceBetween(Node a, Node b, Dictionary<(Node, Node), int> measurements)
        {
            // TODO: once know distance between all nodes that touch the root, can work out distance between all nodes; work out distance to the root-adjacent for each, then add the root adjacent
            // can pre-calculate this and cache it.

            int distance = 0;

            if (!measurements.TryGetValue((a, b), out distance) && !measurements.TryGetValue((b, a), out distance))
            {
                // Find common parent
                Node common = GetCommonNode(a, b);

                // Distance is sum of distances to parents (or just distance if it is to a parent)
                distance = ChildDistance(a, common);
                distance += ChildDistance(b, common);

                //// Need to handle case where common is @ as path will rarely pass through there
                //if (centre != null && common == centre)
                //{
                //    // If the nodes are in different quadrants, adjust distance
                //    if (a.X < centre.X == b.X < centre.X) distance -= 2;
                //    if (a.Y < centre.Y == b.Y < centre.Y) distance -= 2;
                //}
            }

            return distance;
        }

        private int ChildDistance(Node child, Node parent)
        {
            Node current = child;
            int distance = 0;

            while (current != parent)
            {
                distance += current.Distance;
                current = current.Parent;
            }

            return distance;
        }

        private Node GetCommonNode(Node a, Node b)
        {
            var listA = new List<Node> { a };
            var listB = new List<Node> { b };

            // TODO: could do with a single list, as once traversal of list b hits a common node, we're done.
            while(a.Parent != null)
            {
                listA.Add(a.Parent);
                a = a.Parent;
            }

            while (b.Parent != null)
            {
                listB.Add(b.Parent);
                b = b.Parent;
            }

            var intersect = listA.Intersect(listB); // now has all nodes in common
            var result = intersect.First();

            if(result.Parent == null) // root of graph - not used here!
            {
                // TODO
            }
            
            return intersect.First(); // MUST be a common ancestor
        }

        private void Set(ref string input, int index, char c)
        {
            var sb = new StringBuilder(input);
            sb[index] = c;
            input = sb.ToString();
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
        }

        private void LinkThings(Node parent, (int x, int y) location, (int x, int y) previous, int distance, string blockers, string [] maze, List<Node> things)
        {
            foreach(var newLocation in GetAdjacentLocations(maze, location, previous))
            {
                var node = things.FirstOrDefault(t => t.X == newLocation.x && t.Y == newLocation.y);

                if (node != null)
                {
                    node.Distance = distance + 1;
                    node.Parent = parent;
                    node.Blockers = blockers.ToLower();

                    // Dont set Doors as nodes in the graph, set them as blockers
                    if (node.Thing >= 'A' && node.Thing <= 'Z')
                    {
                        things.Remove(node);
                        LinkThings(parent, newLocation, location, distance + 1, blockers + node.Thing, maze, things);
                    }
                    else
                    {
                        LinkThings(node, newLocation, location, 0, "", maze, things); // start from new node, and distance = 0, with no blockers
                    }
                }
                else
                {
                    LinkThings(parent, newLocation, location, distance + 1, blockers, maze, things);
                }
            }
        }

        private Dictionary<(Node, Node), int> MeasureThings(List<Node> things, string[] maze)
        {
            var result = new Dictionary<(Node, Node), int>();

            for (int i = 0; i < things.Count; i++)
            {
                Dictionary<Node, int> distances = new Dictionary<Node, int>();
                Node node = things[i];
                Measure(node.Location, node.Location, 0, maze, things, ref distances);

                foreach (var pair in distances)
                {
                    if (!result.ContainsKey((pair.Key, node)))
                    {
                        result[(node, pair.Key)] = pair.Value;
                    }
                }
            }

            return result;
        }

        private void Measure((int x, int y) location, (int x, int y) previous, int distance, string[] maze, IEnumerable<Node> things, ref Dictionary<Node, int> distances)
        {
            foreach (var newLocation in GetAdjacentLocations(maze, location, previous))
            {
                var node = things.FirstOrDefault(t => t.X == newLocation.x && t.Y == newLocation.y);

                if (node != null)
                {
                    distances[node] = distance + 1;
                    // Stops searching along this branch
                    // TODO: could stop search if found everything in things list
                }
                else
                {
                    Measure(newLocation, location, distance + 1, maze, things, ref distances);
                }
            }
        }

        private IEnumerable<(int x, int y)> GetAdjacentLocations(string[] maze, (int x, int y) location, (int x, int y) previous)
        {
            var up =    GetOrCreateLocation(maze, location.x    , location.y - 1, previous); // UP
            var down =  GetOrCreateLocation(maze, location.x    , location.y + 1, previous); // DOWN
            var left =  GetOrCreateLocation(maze, location.x - 1, location.y    , previous); // LEFT
            var right = GetOrCreateLocation(maze, location.x + 1, location.y    , previous); // RIGHT

            if (up.HasValue) yield return up.Value;
            if (down.HasValue) yield return down.Value;
            if (left.HasValue) yield return left.Value;
            if (right.HasValue) yield return right.Value;
        }

        private (int x, int y)? GetOrCreateLocation(string[] maze, int x, int y, (int x, int y) previous)
        {
            if (x >= 0 && x < maze[0].Length && y >= 0 && y < maze.Length)
            {
                if(previous.x != x || previous.y != y)
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
    }
}
