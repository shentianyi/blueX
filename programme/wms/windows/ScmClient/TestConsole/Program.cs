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
            //string msg = "00 FF 11 16 30 98 49 19 02 35 04 90 E3 7F 02 92 FF ".Replace(" ", "");
            //Regex msgRegex = new Regex(@"^(00FF)[A-Z0-9]{28}(FF)$");
            //Match m = msgRegex.Match(msg);
            //Console.WriteLine(m.Success);

            ////Regex carRegex = new Regex(@"[A-E](\w{3})");
            ////Regex boxRegex = new Regex(@"[F-Z0-9](\w{3})");
            ////Match match = carRegex.Match("C1 11 30 98 49 19 00 21 10 50 AC 3D");
            //byte[] recvbuf = new byte[] { 0, 255, 0, 32, 48, 152, 73, 25, 0, 33, 18, 16, 154, 215, 1, 2, 255 };
            //string data = string.Empty;
            //foreach (byte b in recvbuf)
            //{
            //    //string bb = b.ToString("X0");
            //    //if (bb.Length == 1) {
            //    //    bb = "0" + bb;
            //    //}
            //    //data +=bb + " ";
            //    data += b.ToString("X0").PadLeft(2, '0');
            //}

            //Console.WriteLine(data);


            //string dd = new String(data.Skip(6).Take(36).ToArray()).Replace(" ", "");
            //Console.WriteLine(dd);


            //string ddd= new String(msg.Skip(4).Take(4).ToArray()).Replace(" ", "");
            //Console.WriteLine(ddd);

            Com.Open();
            Console.Read();

        }
    }
}
