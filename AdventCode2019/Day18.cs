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
            

            // Get coordinates of all non-walls and non-floors
            var things = FindThings(maze);

            var startNode = things.First(t => t.Thing == '@');
            things.Remove(startNode);

            // BFS the maze, linking nodes and setting blockers as go.
            LinkNodesBFS(startNode, maze, things);

            things.RemoveAll(n => n.Thing < 'a');

            // Work out distances between start nodes (those that link to @)
            var measurements = RootDistances(things.Where(t => t.Parent == startNode).ToList(), maze);

            // Work out distances between all nodes for convenience
            CalculateDistances(things, measurements, startNode);
            
            // Solve problem


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

        private void CalculateDistances(List<Node> nodes, Dictionary<(Node from, Node to), int> measurements, Node startNode)
        {
            List<Node> toSet = new List<Node>(nodes.Except(nodes.Where(t => t.Parent == startNode)));
            List<Node> available = new List<Node>(nodes.Except(toSet));

            while(toSet.Any())
            {
                var settable = toSet.Where(n => available.Contains(n.Parent)).ToList();
                foreach(var item in settable)
                {
                    toSet.Remove(item);

                    measurements[(item, startNode)] = item.Distance;
                    measurements[(startNode, item)] = item.Distance;

                    var parent = item.Parent;
                    var distance = item.Distance - parent.Distance;

                    measurements.Add((item, item.Parent), distance);
                    measurements.Add((item.Parent, item), distance);

                    foreach (var otherItem in available.Where(a => a != parent))
                    {
                        distance = measurements[(parent, otherItem)] - parent.Distance + item.Distance;
                        //Debug.WriteLine($"Connect {item}, {otherItem}. Distance = {distance}");

                        measurements.Add((item, otherItem), distance);
                        measurements.Add((otherItem, item), distance);
                    }

                    available.Add(item);
                }
            }
        }

        private Dictionary<(Node, Node), int> RootDistances(IList<Node> nodes, string [] maze)
        {
            var distances = new Dictionary<(Node, Node), int>();

            for(int i = 0; i < nodes.Count(); i++)
            {
                // BFS until find other nodes on list
                var node = nodes[i];
                var missing = nodes.Except( nodes.Take(i + 1) );
                var location = node.Location;

                // add root connections
                distances[(node, node.Parent)] = node.Distance;
                distances[(node.Parent, node)] = node.Distance;

                var open = GetAdjacentLocations(maze, location);
                var visited = new List<(int x, int y)> { location };
                int distance = 0;

                while (missing.Any())
                {
                    Debug.Assert(open.Any());

                    distance++;

                    open = open.SelectMany(o => GetAdjacentLocations(maze, o)).Distinct().Except(visited).ToList();

                    var found = missing.Where(n => open.Contains(n.Location));
                    foreach(var f in found)
                    {
                        distances.Add((node, f), distance);
                        distances.Add((f, node), distance);
                    }

                    missing = missing.Except(found).ToList();
                    visited.AddRange(open);
                }
            }

            return distances;
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
                open = group[true].OrderBy(t => measurements[(t, node)]).Concat(group[false].OrderBy(t => measurements[(t, node)])).ToList();
            }
        }


        public long SolveBFS(Node start, IEnumerable<Node> things, Dictionary<(Node, Node), int> measurements, string preVisit = "")
        {
            var targetLength = things.Count();

            var open = things.Where(t => !t.Blockers.Except(preVisit).Any() && t.Parent == start && t.Thing == 'f').OrderBy(t => t.Distance).Reverse();
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

                Debug.WriteLine($"Try {search.distance} : {search.node.Thing} -> {search.visited} + {node.Thing}");

                var visited = search.visited + node.Thing;
                var distance = measurements[(search.node, node)] + search.distance;

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

        private void LinkNodesBFS(Node start, string [] maze, IEnumerable<Node> things)
        {
            var open = GetAdjacentLocations(maze, start.Location).Select(o => (location:o, parent:start, blockers:"")).ToList();
            var visited = new List<(int x, int y)>();
            int distance = 0;

            while(open.Count() > 0)
            {
                distance++;

                open = open.SelectMany(o => GetAdjacentLocations(maze, o.location).Select(l => (l, o.parent, o.blockers))).Distinct().ToList();

                for(int i = open.Count() - 1; i >= 0; i--)
                {
                    var (location, parent, blockers) = open[i];
                    if(visited.Contains(location))
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
                        }
                        else
                        {
                            blockers += node.Thing;
                            node = parent; // don't replace the parent
                        }

                        open[i] = (location, node, blockers); // all subsequent items will use this new parent
                    }

                    visited.Add(location);
                }
            }
        }

        private IEnumerable<(int x, int y)> GetAdjacentLocations(string[] maze, (int x, int y) location, (int x, int y)? previous = null)
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

        private (int x, int y)? GetOrCreateLocation(string[] maze, int x, int y, (int x, int y)? previous = null)
        {
            if (x >= 0 && x < maze[0].Length && y >= 0 && y < maze.Length)
            {
                if(!previous.HasValue || previous?.x != x || previous?.y != y)
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
