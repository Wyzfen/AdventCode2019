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
            Vector[] velocities = new Vector[] { new int[3], new int[3], new int[3], new int[3] };

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
            Vector[] velocities = new Vector[] { new int[3], new int[3], new int[3], new int[3] };


            int x = -1, y = -1, z = -1;
            int count = 0;

            Vector[] startState = (Vector[])planets.Clone();

            while (x < 0 || y < 0 || z < 0)
            {
                ApplyGravity(planets, velocities);
                ApplyVelocity(planets, velocities);

                count++;

                if (x < 0 && velocities.All(v => v[0] == 0) && planets.Select(p => p[0]).SequenceEqual(startState.Select(p => p[0])))
                {
                    x = count;
                }

                if (y < 0 && velocities.All(v => v[1] == 0) && planets.Select(p => p[1]).SequenceEqual(startState.Select(p => p[1])))
                {
                    y = count;
                }

                if (z < 0 && velocities.All(v => v[2] == 0) && planets.Select(p => p[2]).SequenceEqual(startState.Select(p => p[2])))
                {
                    z = count;
                }
            }

            ulong result = Utils.LeastCommonMultiple((ulong)x, (ulong)y, (ulong)z);

            Assert.AreEqual(result, (ulong)496734501382552);
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

    public struct Vector
    {
        private int[] values;

        public Vector(IEnumerable<int> values) => this.values = values.ToArray();

        public Vector(params int[] values) => this.values = values;

        public static implicit operator Vector(int[] values) => new Vector(values);

        public Vector CompareTo(Vector other) => values.Zip(other.values, (t, o) => t.CompareTo(o)).ToArray();

        public int this[int index] => values[index];

        public static Vector operator +(Vector left, Vector right) => left.values.Zip(right.values, (l, r) => l + r).ToArray();
        public static Vector operator -(Vector left, Vector right) => left.values.Zip(right.values, (l, r) => l - r).ToArray();
        
        public int Energy => values.Sum(v => Math.Abs(v));
    }
}
