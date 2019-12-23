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
            //var maze = new string[]
            //{
            //   "#################",
            //   "#i.G..c...e..H.p#",
            //   "########.########",
            //   "#j.A..b...f..D.o#",
            //   "########@########",
            //   "#k.E..a...g..B.n#",
            //   "########.########",
            //   "#l.F..d...h..C.m#",
            //   "#################",

            //   //"########################",
            //   //"#...............b.C.D.f#",
            //   //"#.######################",
            //   //"#.....@.a.B.c.d.A.e.F.g#",
            //   //"########################",

            //   //"########################",
            //   //"#@..............ac.GI.b#",
            //   //"###d#e#f################",
            //   //"###A#B#C################",
            //   //"###g#h#i################",
            //   //"########################",
            //};

            var things = FindThings(maze);

            var startNode = things.First(t => t.Thing == '@');
            things.Remove(startNode);

            // Maze is in quarters; do each quadrant separately.
            Set(ref maze[startNode.Y], startNode.X - 1, '#');
            Set(ref maze[startNode.Y], startNode.X + 1, '#');
            Set(ref maze[startNode.Y - 1], startNode.X, '#');
            Set(ref maze[startNode.Y + 1], startNode.X, '#');

            LinkThings(startNode, (startNode.X - 1, startNode.Y - 1), startNode.Location, 2, "", maze, things); // startNode.Location is basically empty
            LinkThings(startNode, (startNode.X + 1, startNode.Y - 1), startNode.Location, 2, "", maze, things); // startNode.Location is basically empty
            LinkThings(startNode, (startNode.X - 1, startNode.Y + 1), startNode.Location, 2, "", maze, things); // startNode.Location is basically empty
            LinkThings(startNode, (startNode.X + 1, startNode.Y + 1), startNode.Location, 2, "", maze, things); // startNode.Location is basically empty

            //Set(ref maze[startNode.Y], startNode.X - 1, '.');
            //Set(ref maze[startNode.Y], startNode.X + 1, '.');
            //Set(ref maze[startNode.Y - 1], startNode.X, '.');
            //Set(ref maze[startNode.Y + 1], startNode.X, '.');

            //LinkThings(startNode, startNode.Location, startNode.Location, 0, "", maze, things);
            things.RemoveAll(n => n.Thing < 'a');

            var measurements = new Dictionary<(Node, Node), int>(); // MeasureThings(things.Where(t => t.Parent == startNode).ToList(), maze);

            // Now now how to get to every node, and the distance from their parent
            //var visited = "";
            //var visible = things.Where(t => t.Parent == startNode);
            //var lookup = visible.ToLookup(t => t.Blockers.Except(visited).Any(), t => t);

            //var open = lookup[false];
            //var closed = lookup[true];

            //long result = long.MaxValue;

            //Solve(startNode, open, closed, visited, 0, things, measurements, ref result);

            long result = SolveBFS(startNode, things, measurements, "fnagxoels");

            Assert.AreEqual(result, 6074); // fnagxoezkmwpuvtdlsirqjyhcb
        }

        [TestMethod]
        public void Problem2()
        {
            var result = 0L;
            Assert.AreEqual(result, 1143770635);
        }

        [DebuggerDisplay("{node} : {visited} with {open.Count} open [{distance}]")]
        private class BFSSearch
        {
            public Node node;
            public List<Node> open = new List<Node>();
            public string visited = "";
            public int distance = 0;

            public void Update(IEnumerable<Node> things, Dictionary<(Node, Node), int> measurements)
            {
                var available = things.Where(t => t != node && !t.Blockers.Except(visited).Any() && !visited.Contains(t.Thing));
                var group = available.ToLookup(a => a.Parent == node); // prioritise those with this as parent
                open = group[true].OrderBy(t => DistanceBetween(node, t, measurements)).Concat(group[false].OrderBy(t => DistanceBetween(node, t, measurements))).ToList();
            }
        }


        public long SolveBFS(Node start, IEnumerable<Node> things, Dictionary<(Node, Node), int> measurements, string preVisit = "")
        {
            var targetLength = things.Where(t => t.Thing >= 'a').Count();

            var open = things.Where(t => !t.Blockers.Except(preVisit).Any() && t.Parent == start && t.Thing == 'f').OrderBy(t => DistanceBetween(start, t, measurements)).Reverse();
            var searches = new List<BFSSearch> { new BFSSearch { open = open.ToList(), node = start } };//open.Select(o => new BFSSearch { open = open.ToList(), node = o }).ToList();

            int solutionCount = 0;
            int minResult = int.MaxValue;

            while(searches.First().visited.Length < targetLength /*&& solutionCount < 10000*/)
            {
                var search = searches.First();

                if (search.open.Count() == 0 || search.distance >= minResult)
                {
                    searches.Remove(search);
                    continue;
                }

                var node = search.open.First();
                search.open.RemoveAt(0);

                if (search.open.Count() == 0)
                {
                    searches.Remove(search);
                }

                //Debug.WriteLine($"Try {search.distance} : {search.node.Thing} -> {search.visited} + {node.Thing}");

                var visited = search.visited + node.Thing;
                var distance = DistanceBetween(search.node, node, measurements) + search.distance;

                if (distance < minResult)
                {
                    if (visited.Length == targetLength)
                    {
                        if (minResult > distance)
                        {
                            minResult = distance;
                        }

                        Debug.WriteLine($"{visited} in {distance} [{minResult}] after {solutionCount + 1} tries");

                        solutionCount++;
                    }
                    else
                    {
                        var newSearch = new BFSSearch { node = node, open = new List<Node>(search.open), visited = visited, distance = distance };
                        newSearch.Update(things, measurements);

                        if (newSearch.open.Count() > 0)
                        {
                            searches.Add(newSearch);

                            // TODO: necessary to sort everytime?
                            // Sorting by anything but distance means that the solution return may not be the shortest
                            searches.Sort((a, b) => b.visited.Length.CompareTo(a.visited.Length));
                        }
                    }
                }
                else
                {
                    searches.Remove(search);
                }

            }

            
            return minResult;
        }

        public void Solve(Node previous, IEnumerable<Node> open, IEnumerable<Node> closed, string visited, long distance, IEnumerable<Node> things, Dictionary<(Node, Node), int> measurements, ref long minDistance)
        {
            // TODO: rather than using depth-first search, should use breadth-first
            // Have a list of traversals, sorted by distance travelled - pick the shortest distance, and process the nearest node. This then gets added to the list of traversals.
            // maybe only need to store visited list and distance - can get open list from this, then take the nearest to the current location (end of visited list)
            // only need to check nearest on open list but still worth cacheing it for each option
            /* e.g. @->a = 2, @->b = 4, a->b = 6
             list = @+a [2], @+b [4]
              @+a : visit a [2]
             list = @+b [4], a+b [10]
              @+b : visit b [4]
             list = b+a [10], a+b [10]
              b+a : visit a [10]
             list = ba [10], a+b [10]   
              return ab
            */
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

                    var children = things.Where(t => t.Parent == node);
                    string newVisited = visited + node.Thing;
                    var lookup = children.Union(closed).ToLookup(t => t.Blockers.Except(newVisited).Any(), t => t);

                    var newOpen = lookup[false].Union(open.Except(new[] { node }));
                    var newClosed = lookup[true];

                    Solve(node, newOpen, newClosed, newVisited, newDistance, things, measurements, ref minDistance);
                }
            }
        }

        private static int DistanceBetween(Node a, Node b, Dictionary<(Node, Node), int> measurements)
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

                // Need to handle case where common is @ as path will rarely pass through there
                if (common.Parent == null)
                {
                    // If the nodes are in different quadrants, adjust distance
                    if (a.X < common.X == b.X < common.X) distance -= 2;
                    if (a.Y < common.Y == b.Y < common.Y) distance -= 2;
                }
            }

            return distance;
        }

        private static int ChildDistance(Node child, Node parent)
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

        private static Node GetCommonNode(Node a, Node b)
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
                        LinkThings(parent, newLocation, location, distance + 1, blockers + node.Thing, maze, things);
                    }
                    else
                    {
                        LinkThings(node, newLocation, location, 0, blockers, maze, things); // start from new node, and distance = 0, with parent's blockers
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
                    Debug.WriteLine($"Measure {newLocation} from {location} from {previous} [{distance + 1}]");
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
