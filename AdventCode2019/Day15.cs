using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day15
    {
        readonly long[] intCode = Utils.LongsFromCSVFile("day15.txt")[0];

        [TestMethod]
        public void Problem1()
        {
            var locations = new List<Location> { new Location { X = 0, Y = 0, Tile = Tile.Floor } };

            var droid = new ShipsComputer(intCode);
            Location current = locations[0];

            while(current.Tile != Tile.Target) // !locations.Any(l => l.Tile == Tile.Target))
            {
                IEnumerable<Location> path = null;

                // See if there's a position next to us
                var next = GetUnknownNeighbour(current, locations);
                if(next != null)
                {
                    path = new Location[] { next };
                }

                // Go back to start and look for another
                if (next == null)
                {   
                    droid = new ShipsComputer(intCode);
                    current = locations[0];

                    next = GetUnvisitedLocation(locations);
                    if (next != null)
                    {
                        path = PathFromLocation(next, locations).Reverse();
                    }
                }

                //// Revisit any that aren't marked as explored - shouldn't be necessary, as they're not marked as all their neighbours are
                //if (path == null)
                //{
                //    foreach(var unexplored in locations.Where(l => !l.Explored))
                //    {
                //        var next = GetUnknownNeighbour(unexplored, locations);
                //        if (next != null)
                //        {
                //            path = new Location[] { next };
                //            break;
                //        }
                //    }
                //}


                //if (path == null) break; // Should never happen


                // Follow the path to the location
                foreach (Location location in path)
                {
                    var input = GetMove(location, current, locations); // will only move through known locations
                    var output = droid.Execute(input).ToList();

                    switch(output.First())
                    {
                        case 0:
                            location.Tile = Tile.Wall;
                            location.Explored = true;
                            break;
                        case 1:
                            location.Tile = Tile.Floor;
                            current = location;
                            break;
                        case 2:
                            location.Tile = Tile.Target;
                            current = location;
                            break;
                    }

                    if (current.Tile == Tile.Target) break;
                }
            }

            Paint(locations);


            Assert.AreEqual(current.Tile, Tile.Target);
            Assert.AreEqual(current.Distance, 272);
        }


        [TestMethod]
        public void Problem2()
        {
        }

        private int GetMove(Location location, Location current, List<Location> locations)
        {
            if (location.Y > current.Y) return 1; // UP
            if (location.Y < current.Y) return 2; // DOWN
            if (location.X < current.X) return 3; // LEFT
            if (location.X > current.X) return 4; // RIGHT

            return 0; // INVALID
        }

        private Location GetUnknownNeighbour(Location location, List<Location> locations)
        {
            if (location.Explored) return null;

            var neighbours = GetAdjacentLocations(location, Tile.Unknown, locations);
            var next = neighbours.FirstOrDefault(l => l.Tile == Tile.Unknown);

            if (next == null) location.Explored = true;

            return next;
        }

        private IEnumerable<Location> GetAdjacentLocations(Location location, Tile type, List<Location> locations)
        {
            var up = GetOrCreateLocation(location.X, location.Y + 1, location.Distance, type, locations); // UP
            var down = GetOrCreateLocation(location.X, location.Y - 1, location.Distance, type, locations); // DOWN
            var left = GetOrCreateLocation(location.X - 1, location.Y, location.Distance, type, locations); // LEFT
            var right = GetOrCreateLocation(location.X + 1, location.Y, location.Distance, type, locations); // RIGHT

            if (up != null) yield return up;
            if (down != null) yield return down;
            if (left != null) yield return left;
            if (right != null) yield return right;
        }

        private Location GetOrCreateLocation(int x, int y, int distance, Tile type, List<Location> locations)
        {
            var location = locations.FirstOrDefault(l => l.X == x && l.Y == y);
            if(location == null && type == Tile.Unknown) // only create if they're searching for new tiles
            {
                location = new Location { X = x, Y = y, Tile = Tile.Unknown, Distance = distance + 1 };
                locations.Add(location);
            }

            return location?.Tile == type ? location : null;
        }

        private Location GetUnvisitedLocation(List<Location> locations) => locations.Where(l => l.Tile == Tile.Unknown).OrderBy(l => l.Distance).FirstOrDefault();

        private IEnumerable<Location> PathFromLocation(Location location, List<Location> locations)
        {
            var current = location;
            while(current.X != 0 || current.Y != 0)
            {
                yield return current;

                current = GetAdjacentLocations(current, Tile.Floor, locations).OrderBy(l => l.Distance).FirstOrDefault();
            }
        }

        public enum Tile : int
        {
            Unknown = 0,
            Floor = 1,
            Wall = 2,
            Target = 3
        }

        [DebuggerDisplay("{X},{Y} : {Tile} @ {Distance} ({Explored})")]
        public class Location
        {
            public Tile Tile = Tile.Unknown;
            public int X, Y;
            public int Distance = 0;

            public bool Explored = false; // Set when it and all it's neighbours are visited
        }

        string[] Paint(List<Location> input, IEnumerable<Location> path = null)
        {
            int maxX = input.Max(k => k.X);
            int maxY = input.Max(k => k.Y);
            int minX = input.Min(k => k.X);
            int minY = input.Min(k => k.Y);

            StringBuilder[] image = Enumerable.Range(0, maxY - minY + 1).Select(_ => new StringBuilder(new string(' ', maxX - minX + 1))).ToArray();
            char[] pixels = { ':', '·', '█', '*' };

            foreach (var item in input)
            {
                image[item.Y - minY][item.X - minX] = pixels[(int) item.Tile];
            }

            image[0 - minY][0 - minX] = 'x';

            if (path != null)
            {
                foreach (var location in path)
                {
                    image[location.Y - minY][location.X - minX] = '+';
                }

                image[path.Last().Y - minY][path.Last().X - minX] = '@';
            }

            foreach (var line in image)
            {
                Debug.WriteLine(line);
            }

            Debug.WriteLine("");

            return image.Select(i => i.ToString()).ToArray();
        }
    }
}
