using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace TestConsole
{
   public class Com
    {

      public static SerialPort sp;
       public static void Open() {
           try
           { 
               sp = new SerialPort("COM13");
               sp.BaudRate = 38400;
               sp.Open();
               sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
               Console.WriteLine("opened!");
              // sp.Close();
           }
           catch (Exception e) {
               Console.WriteLine(e.Message);
           }
       }

       static void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
       {

           Byte[] recvbuf = new Byte[17];
           sp.Read(recvbuf, 0, recvbuf.Length);

           string data = System.Text.Encoding.Default.GetString(recvbuf);

           Console.WriteLine(data);
           //throw new NotImplementedException();
       }
    }
}
