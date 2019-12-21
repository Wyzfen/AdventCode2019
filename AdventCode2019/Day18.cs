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
            var things = FindThings(maze);

            var startNode = things.First(t => t.Thing == '@');

            // Maze is in quarters; do each quadrant separately.
            Set(ref maze[40], 39, '#');
            Set(ref maze[40], 41, '#');
            Set(ref maze[39], 40, '#');
            Set(ref maze[41], 40, '#');

            DepthFirst(startNode, (39, 39), (40, 40), 2, maze, things); // (40, 40) is basically empty
            DepthFirst(startNode, (41, 39), (40, 40), 2, maze, things); // (40, 40) is basically empty
            DepthFirst(startNode, (39, 41), (40, 40), 2, maze, things); // (40, 40) is basically empty
            DepthFirst(startNode, (41, 41), (40, 40), 2, maze, things); // (40, 40) is basically empty

            // Now now how to get to every node, and the distance from their parent
            var open = things.Where(t => t.Parent == startNode);

            foreach(var a in open)
            {
                foreach(var b in open)
                {
                    Debug.WriteLine($"Distance {a} to {b} = {DistanceBetween(a, b, startNode)}");
                }
            }

            var result = 0L;
            Assert.AreEqual(result, 19355391);
        }

        [TestMethod]
        public void Problem2()
        {
            var result = 0L;
            Assert.AreEqual(result, 1143770635);
        }

        private int DistanceBetween(Node a, Node b, Node centre)
        {
            int distance = 0;

            // Find common parent
            Node common = GetCommonNode(a, b);

            // Distance is sum of distances to parents (or just distance if it is to a parent)
            distance = ChildDistance(a, common);
            distance += ChildDistance(b, common);

            // Need to handle case where common is @ as path will rarely pass through there
            if (common == centre)
            {
                // If the nodes are in different quadrants, adjust distance
                if (a.X < 40 == b.X < 40) distance -= 2;
                if (a.Y < 40 == b.Y < 40) distance -= 2;
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

        [DebuggerDisplay("{Thing.ToString()} @ ({X}, {Y}) [{Distance}] via {Parent?.Thing.ToString() ?? \" \"}")]
        public class Node
        {
            public int X;
            public int Y;

            public char Thing;

            public int Distance;
            
            public Node Parent;

            public override string ToString() => $"{Thing.ToString()} @ ({X}, {Y}) [{Distance}]";
        }

        private void DepthFirst(Node parent, (int x, int y) location, (int x, int y) previous, int distance, string [] maze, List<Node> things)
        {
            foreach(var newLocation in GetAdjacentLocations(maze, location, previous))
            {
                var node = things.FirstOrDefault(t => t.X == newLocation.x && t.Y == newLocation.y);

                if (node != null)
                {
                    node.Distance = distance + 1;
                    node.Parent = parent;

                    DepthFirst(node, newLocation, location, 0, maze, things); // start from new node, and distance = 0
                }
                else
                {
                    DepthFirst(parent, newLocation, location, distance + 1, maze, things);
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
