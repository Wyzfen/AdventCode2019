using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019
{
    public static class Utils
    {
        public static IEnumerable<int> IntsFromFile(string filename) => 
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => int.Parse(s));

        public static IEnumerable<int> IntsFromString(string input) =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Select(s => int.Parse(s));

        // Returns an array of arrays from a CSV file
        public static String[][] StringsFromCSVFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => s.Split(',')).ToArray();

        // Returns an array of arrays from a CSV file
        public static int[][] IntsFromCSVFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => s.Split(',').Select(i => int.Parse(i)).ToArray()).ToArray();

        public static long[][] LongsFromCSVFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => s.Split(',').Select(i => long.Parse(i)).ToArray()).ToArray();

        // Returns an array of arrays from a CSV file
        public static String[][] StringsFromCSVString(string input) =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Select(s => s.Split(',')).ToArray();

        public static IEnumerable<T> Generate<T>(T value, Func<T, T> func)
        {
            while(true)
            {
                yield return value;
                value = func(value);
            }
        }

        public static Dictionary<string, string> SplitFromFile(string filename, char split = ')') =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(i => i.Split(split)).ToDictionary(s => s[1], s => s[0]);

        public static Dictionary<string, string> SplitFromString(string input, char split = ')') =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Select(i => i.Split(split)).ToDictionary(s => s[1], s => s[0]);



        /// <summary>
        /// Uses factorial notation to give all permutations of input set.
        /// if input set is in lexigraphical order, the results will be too.
        /// ie, pass in 012 (the lowest combination of 0,1 and 2) and the next one will be 021
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static IEnumerable<List<int>> Permutations(IEnumerable<int> set)
        {
            int count = set.Count();
            ulong number = Factorial(count);
            int[] factors = new int[count];

            for (ulong n = 0; n < number; n++)
            {
                List<int> workingSet = new List<int>(set);
                List<int> result = new List<int>();

                for (int i = count - 1; i >= 0; i--)
                {
                    int j = factors[i];
                    result.Add(workingSet[j]);
                    workingSet.RemoveAt(j);
                }

                yield return result;

                for (int index = 1; index < count; index++)
                {
                    factors[index]++;
                    if (factors[index] <= index) break;

                    factors[index] = 0;
                }
            }
        }

        public static ulong Factorial(int n)
        {
            ulong value = 1;
            for (int i = 2; i <= n; i++)
            {
                value *= (ulong)i;
            }

            return value;
        }
    }
}
