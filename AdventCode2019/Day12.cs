using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day12
    {
        readonly Vector[] input = new Vector[] { new Vector(-10, -10, -13), new Vector(5, 5, -9), new Vector(3, 8, -16), new Vector(1, 3, -3) };

        [TestMethod]
        public void Problem1()
        {
            Vector[] planets = input;
            Vector[] velocities = new Vector[4];

            for (int i = 0; i < 1000; i++)
            {
                ApplyGravity(planets, velocities);
                ApplyVelocity(planets, velocities);
            }

            int result = planets.Zip(velocities, (p, v) => p.Energy * v.Energy).Sum();

            Assert.AreEqual(result, 6678);
        }

        [TestMethod]
        public void Problem2()
        {
            Vector[] planets = input;
            Vector[] velocities = new Vector[4];

            int x = -1, y = -1, z = -1;
            int count = 0;

            Vector[] startState = (Vector[])planets.Clone();

            while (x < 0 || y < 0 || z < 0)
            {
                ApplyGravity(planets, velocities);
                ApplyVelocity(planets, velocities);

                count++;

                if (x < 0 && velocities.All(v => v.X == 0) && planets.Select(p => p.X).SequenceEqual(startState.Select(p => p.X)))
                {
                    x = count;
                }

                if (y < 0 && velocities.All(v => v.Y == 0) && planets.Select(p => p.Y).SequenceEqual(startState.Select(p => p.Y)))
                {
                    y = count;
                }

                if (z < 0 && velocities.All(v => v.Z == 0) && planets.Select(p => p.Z).SequenceEqual(startState.Select(p => p.Z)))
                {
                    z = count;
                }
            }

            ulong result = Utils.LeastCommonMultiple((ulong)x, (ulong)y, (ulong)z);

            Assert.AreEqual(result, (ulong)496734501382552);
        }

        public struct Vector
        {
            public int X, Y, Z;

            public Vector(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public Vector CompareTo(Vector other) => new Vector { X = X.CompareTo(other.X), Y = Y.CompareTo(other.Y), Z = Z.CompareTo(other.Z) };

            public static Vector operator +(Vector first, Vector other) => new Vector(first.X + other.X, first.Y + other.Y, first.Z + other.Z);
            public static Vector operator -(Vector first, Vector other) => new Vector(first.X - other.X, first.Y - other.Y, first.Z - other.Z);

            public int Energy => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        }

        public static void ApplyGravity(Vector[] positions, Vector[] velocities)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                for (int j = i + 1; j < positions.Length; j++)
                {
                    var comp = positions[i].CompareTo(positions[j]);
                    velocities[i] -= comp;
                    velocities[j] += comp;
                }
            }
        }

        public static void ApplyVelocity(Vector[] positions, Vector[] velocities)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] += velocities[i];
            }
        }
    }
}
