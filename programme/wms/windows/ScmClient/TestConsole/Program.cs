using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex carRegex = new Regex(@"[A-E](\w{3})");
            Regex boxRegex = new Regex(@"[F-Z0-9](\w{3})");
            Match match = carRegex.Match("C1 11 30 98 49 19 00 21 10 50 AC 3D");

        }
    }
}
