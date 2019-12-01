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
    }
}
