using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day12
    {
        [DebuggerDisplay("({x}, {y}, {z})")]
        struct Vector
        {
            public int x, y, z;

            public Vector SignedDelta(Vector other) => new Vector
            {
                x = x.CompareTo(other.x),
                y = y.CompareTo(other.y),
                z = z.CompareTo(other.z),
            };

            public static Vector operator+ (Vector left, Vector right) =>
                new Vector { x = left.x + right.x, y = left.y + right.y, z = left.z + right.z };

            public static Vector operator- (Vector left, Vector right) =>
                new Vector { x = left.x - right.x, y = left.y - right.y, z = left.z - right.z };

            public static bool operator== (Vector left, Vector right) => left.Equals(right);
            public static bool operator!=(Vector left, Vector right) => !(left == right);

            public override string ToString() => $"({x}, {y}, {z})";

            public override bool Equals(object obj)
            {
                if (obj is Vector other)
                {
                    return x == other.x && y == other.y && z == other.z;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        [DebuggerDisplay("p:{position} v:{velocity}")]
        struct State
        {
            public Vector position;
            public Vector velocity;

            public override bool Equals(object obj)
            {
                if (obj is State state)
                {
                    return position == state.position && velocity == state.velocity;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        Vector [] positions = new Vector [] { new Vector{ x = -10, y = -10, z = -13 },
                                              new Vector{ x = 5, y = 5, z = -9 },
                                              new Vector{ x = 3, y = 8, z = -16},
                                              new Vector{ x = 1, y = 3, z = -3 } };
        Vector[] velocities = new Vector[4];

        List<State []> history = new List<State []>();

        [TestMethod]
        public void Problem1()
        {
            //var positions = new Vector[] { new Vector { x = -1, y = 0, z = 2},
            //                                new Vector { x = 2, y = -10, z = -7 },
            //                                new Vector { x = 4, y = -8, z = 8 },
            //                                new Vector { x = 3, y = 5, z = -1 }};

            for(int i = 0; i < 1000; i++)
            {
                ApplyGravity(positions, velocities);
                ApplyVelocity(positions, velocities);
            }

            int result = Energy(positions).Zip(Energy(velocities), (a, b) => a * b).Sum();

            Assert.AreEqual(result, 6678);
        }

        [TestMethod]
        public void Problem2()
        {
            //var positions = new Vector[] { new Vector { x = -1, y = 0, z = 2},
            //                                new Vector { x = 2, y = -10, z = -7 },
            //                                new Vector { x = 4, y = -8, z = 8 },
            //                                new Vector { x = 3, y = 5, z = -1 }};

            history.Add(positions.Zip(velocities, (p, v) => new State { position = p, velocity = v }).ToArray());

            long count = 0;

            while(true)
            {
                count++;

                ApplyGravity(positions, velocities);
                ApplyVelocity(positions, velocities);

                //var newState = positions.Zip(velocities, (p, v) => new State { position = p, velocity = v }).ToArray();
                //if (history.Any(h => h.SequenceEqual(newState))) break;
                //history.Add(newState);
                int result = Energy(positions).Zip(Energy(velocities), (a, b) => a * b).Sum();
                //Debug.WriteLine($"{count} : {result}");
            }

            Assert.AreEqual(count, 4825810);
        }

        void ApplyGravity(Vector [] positions, Vector [] velocities)
        {
            for(int i = 0; i < positions.Length; i++)
            {
                for(int j = i + 1; j < positions.Length; j++)
                {
                    var delta = positions[i].SignedDelta(positions[j]);
                    velocities[i] -= delta;
                    velocities[j] += delta;
                }
            }
        }

        void ApplyVelocity(Vector[] positions, Vector[] velocities)
        {
            for(int i = 0; i < positions.Length; i++)
            {
                positions[i] += velocities[i];
            }
        }

        IEnumerable<int> Energy(IEnumerable<Vector> items) => items.Select(i => Math.Abs(i.x) + Math.Abs(i.y) + Math.Abs(i.z));
    }
}
