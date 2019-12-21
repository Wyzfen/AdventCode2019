using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day20
    {
        readonly string [] maze = Utils.StringsFromFile("day20.txt").ToArray();

        [TestMethod]
        public void Problem1()
        {

            //var maze = new string[] { "         A           ",
            //                           "         A           ",
            //                           "  #######.#########  ",
            //                           "  #######.........#  ",
            //                           "  #######.#######.#  ",
            //                           "  #######.#######.#  ",
            //                           "  #######.#######.#  ",
            //                           "  #####  B    ###.#  ",
            //                           "BC...##  C    ###.#  ",
            //                           "  ##.##       ###.#  ",
            //                           "  ##...DE  F  ###.#  ",
            //                           "  #####    G  ###.#  ",
            //                           "  #########.#####.#  ",
            //                           "DE..#######...###.#  ",
            //                           "  #.#########.###.#  ",
            //                           "FG..#########.....#  ",
            //                           "  ###########.#####  ",
            //                           "             Z       ",
            //                           "             Z       " };


            //var maze = new string [] {  "                   A               ",
            //                            "                   A               ",
            //                            "  #################.#############  ",
            //                            "  #.#...#...................#.#.#  ",
            //                            "  #.#.#.###.###.###.#########.#.#  ",
            //                            "  #.#.#.......#...#.....#.#.#...#  ",
            //                            "  #.#########.###.#####.#.#.###.#  ",
            //                            "  #.............#.#.....#.......#  ",
            //                            "  ###.###########.###.#####.#.#.#  ",
            //                            "  #.....#        A   C    #.#.#.#  ",
            //                            "  #######        S   P    #####.#  ",
            //                            "  #.#...#                 #......VT",
            //                            "  #.#.#.#                 #.#####  ",
            //                            "  #...#.#               YN....#.#  ",
            //                            "  #.###.#                 #####.#  ",
            //                            "DI....#.#                 #.....#  ",
            //                            "  #####.#                 #.###.#  ",
            //                            "ZZ......#               QG....#..AS",
            //                            "  ###.###                 #######  ",
            //                            "JO..#.#.#                 #.....#  ",
            //                            "  #.#.#.#                 ###.#.#  ",
            //                            "  #...#..DI             BU....#..LF",
            //                            "  #####.#                 #.#####  ",
            //                            "YN......#               VT..#....QG",
            //                            "  #.###.#                 #.###.#  ",
            //                            "  #.#...#                 #.....#  ",
            //                            "  ###.###    J L     J    #.#.###  ",
            //                            "  #.....#    O F     P    #.#...#  ",
            //                            "  #.###.#####.#.#####.#####.###.#  ",
            //                            "  #...#.#.#...#.....#.....#.#...#  ",
            //                            "  #.#####.###.###.#.#.#########.#  ",
            //                            "  #...#.#.....#...#.#.#.#.....#.#  ",
            //                            "  #.###.#####.###.###.#.#.#######  ",
            //                            "  #.#.........#...#.............#  ",
            //                            "  #########.###.###.#############  ",
            //                            "           B   J C                 ",
            //                            "           U   P P                 "};

            int distance = SolveWarpMaze(false);

            Assert.AreEqual(distance, 514);
        }

        [TestMethod]
        public void Problem2()
        {
            //var maze = new string[] {  "             Z L X W       C                 ",
            //                            "             Z P Q B       K                 ",
            //                            "  ###########.#.#.#.#######.###############  ",
            //                            "  #...#.......#.#.......#.#.......#.#.#...#  ",
            //                            "  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  ",
            //                            "  #.#...#.#.#...#.#.#...#...#...#.#.......#  ",
            //                            "  #.###.#######.###.###.#.###.###.#.#######  ",
            //                            "  #...#.......#.#...#...#.............#...#  ",
            //                            "  #.#########.#######.#.#######.#######.###  ",
            //                            "  #...#.#    F       R I       Z    #.#.#.#  ",
            //                            "  #.###.#    D       E C       H    #.#.#.#  ",
            //                            "  #.#...#                           #...#.#  ",
            //                            "  #.###.#                           #.###.#  ",
            //                            "  #.#....OA                       WB..#.#..ZH",
            //                            "  #.###.#                           #.#.#.#  ",
            //                            "CJ......#                           #.....#  ",
            //                            "  #######                           #######  ",
            //                            "  #.#....CK                         #......IC",
            //                            "  #.###.#                           #.###.#  ",
            //                            "  #.....#                           #...#.#  ",
            //                            "  ###.###                           #.#.#.#  ",
            //                            "XF....#.#                         RF..#.#.#  ",
            //                            "  #####.#                           #######  ",
            //                            "  #......CJ                       NM..#...#  ",
            //                            "  ###.#.#                           #.###.#  ",
            //                            "RE....#.#                           #......RF",
            //                            "  ###.###        X   X       L      #.#.#.#  ",
            //                            "  #.....#        F   Q       P      #.#.#.#  ",
            //                            "  ###.###########.###.#######.#########.###  ",
            //                            "  #.....#...#.....#.......#...#.....#.#...#  ",
            //                            "  #####.#.###.#######.#######.###.###.#.#.#  ",
            //                            "  #.......#.......#.#.#.#.#...#...#...#.#.#  ",
            //                            "  #####.###.#####.#.#.#.#.###.###.#.###.###  ",
            //                            "  #.......#.....#.#...#...............#...#  ",
            //                            "  #############.#.#.###.###################  ",
            //                            "               A O F   N                     ",
            //                            "               A A D   M                     "};

            int distance = SolveWarpMaze(true);

            Assert.AreEqual(distance, 6208);
        }

        private int SolveWarpMaze(bool useWarp)
        {
            var portals = FindPortals(maze);

            var (startX, startY) = portals["AA"][0]; // only one 'AA' portal
            var (endX, endY) = portals["ZZ"][0]; // only one 'ZZ' portal

            IEnumerable<(int x, int y, int z)> locations = new List<(int x, int y, int z)> { (startX, startY, 0) };
            IEnumerable<(int x, int y, int z)> current = new List<(int x, int y, int z)>(locations);
            int distance = 0;

            (int endX, int endY, int) end3D = (endX, endY, 0);
            while (current.Count() > 0 && !locations.Contains(end3D))
            {
                var next = current.SelectMany(c => GetAdjacentLocations(maze, c, locations, portals, useWarp)).Distinct().ToList();
                locations = current;
                current = next;

                distance++;
            }

            return distance - 1;
        }

        private MultiMap<(int x, int y)> FindPortals(string[] maze)
        {
            var letters = maze.SelectMany((row, y) => row.Select((c, x) => (c, x, y)).Where(c => c.c >= 'A' && c.c <= 'Z')).ToList();

            var result = new MultiMap<(int x, int y)>();

            foreach(var (c, x, y) in letters)
            {
                if (y > 0 && maze[y - 1][x] == '.') result.Add($"{c}{maze[y + 1][x]}", (x, y - 1));
                if (y < maze.Length - 1 && maze[y + 1][x] == '.') result.Add($"{maze[y - 1][x]}{c}", (x, y + 1));
                if (x > 0 && maze[y][x-1] == '.') result.Add($"{c}{maze[y][x + 1]}", (x - 1, y));
                if (x < maze[y].Length - 1 && maze[y][x + 1] == '.') result.Add($"{maze[y][x - 1]}{c}", (x + 1, y));
            }

            return result;
        }


        private IEnumerable<(int x, int y, int z)> GetAdjacentLocations(string [] maze, (int x, int y, int z) location, IEnumerable<(int x, int y, int z)> locations, MultiMap<(int x, int y)> portals, bool warps = false)
        {
            var up = GetOrCreateLocation(maze, location, location.x, location.y - 1, locations, portals, warps); // UP
            var down = GetOrCreateLocation(maze, location, location.x, location.y + 1, locations, portals, warps); // DOWN
            var left = GetOrCreateLocation(maze, location, location.x - 1, location.y, locations, portals, warps); // LEFT
            var right = GetOrCreateLocation(maze, location, location.x + 1, location.y, locations, portals, warps); // RIGHT

            if (up.HasValue) yield return up.Value;
            if (down.HasValue) yield return down.Value;
            if (left.HasValue) yield return left.Value;
            if (right.HasValue) yield return right.Value;
        }

        private (int x, int y, int z)? GetOrCreateLocation(string [] maze, (int x, int y, int z) location, int x, int y, IEnumerable<(int x, int y, int z)> locations, MultiMap<(int x, int y)> portals, bool warps)
        {
            if (x >= 0 && x < maze[0].Length && y >= 0 && y < maze.Length)
            {
                int depth = location.z;
                var test = (x, y, depth);
                if (!locations.Contains(test))
                {
                    var c = maze[y][x];

                    if (c >= 'A' && c <= 'Z')
                    {
                        (int x, int y) location2D = (location.x, location.y);
                        var portal = portals.Where(p => p.Value.Contains(location2D)).Select(p => p.Value).FirstOrDefault();
                        if (portal != null)
                        {
                            if (portal.Count == 1) return null; // 'AA' or 'ZZ'

                            if (warps)
                            {
                                bool outer = x == 1 || y == 1 || x == maze[y].Length - 2 || y == maze.Length - 2;
                                if (outer && depth == 0) return null; // outer portals on level 0 are walls

                                if (outer) // warp out
                                {
                                    depth--;
                                }
                                else // warp in
                                {
                                    depth++;
                                }
                            }

                            (x, y) = (portal[0] == (location.x, location.y) ? portal[1] : portal[0]);
                            var result = (x, y, depth);
                            if (!locations.Contains(result))
                            {
                                return result;
                            }
                        }
                    }
                    else if (c == '.')
                    {
                        return (x, y, depth);
                    }
                }
            }

            return null;
        }

        string[] Paint(string [] maze, List<(int x, int y, int z)> locations, int depth)
        {
            StringBuilder[] image = maze.Select(m => new StringBuilder(m)).ToArray(); 

            char[] pixels = { ':', '·', '#', 'O' };

            foreach(var (x, y, z) in locations.Where(l => l.z == depth))
            {
                image[y][x] = 'O';
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
