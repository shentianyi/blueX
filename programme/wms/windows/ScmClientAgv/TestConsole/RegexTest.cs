using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TestConsole
{
    public class RegexTest
    {
        public static void Run() {

           // string msg = "00 FF 11 16 30 98 49 19 02 35 04 90 E3 7F 02 92 FF ";//.Replace(" ", "");
            string msg = "B1 16 30 98 49 19 02 35 04 90 E3 7F 02 92 FF ".Replace(" ","");
            string msg1 = "C1 16 30 98 49 19 02 35 04 90 E3 7F 02 92 FF ".Replace(" ", "");
            string msg2 = "Z1 16 30 98 49 19 02 35 04 90 E3 7F 02 92 FF ".Replace(" ", "");


            Regex msgRegex = new Regex(@"^(A\w{3})|^(B[0-9]{3})|^([C-F][0-9]{7})");
            Match m = msgRegex.Match(msg);
            Console.WriteLine(m.Success);
            Console.WriteLine(m.Value);

            Match m1 = msgRegex.Match(msg1);
            Console.WriteLine(m1.Success);
            Console.WriteLine(m1.Value);


            Match m2 = msgRegex.Match(msg2);
            Console.WriteLine(m2.Success);
            Console.WriteLine(m2.Value);

        }
    }
}
