using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ScmClient
{
   public class ImportRFIDDll
   {

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComOpenCom(ref IntPtr hCom, string ComPort, int BaudRate);//打开串口
    }
}
