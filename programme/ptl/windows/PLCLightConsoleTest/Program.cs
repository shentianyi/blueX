using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLCLightCL;
using PLCLightCL.Model;
using PLCLightCL.Enum; 
namespace PLCLightConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string conn = "Data Source=svp37001.p37.leoni.local;Initial Catalog=LEPS;Persist Security Info=True;User ID=ASS_user;Password=ass_user";
            //GetHeadRecord();
            LEPSController lc = new LEPSController(conn);
            string productLine = "ASSEN11";
            string board = "BE001";
            string workplace = "ASSEN11_03";
            string ksk = "416T041901EN";
           // string moduleNr = "WI-1502";
            List<string> modules = new List<string>() { "WI-1502" };
            //string productLine = "ASSPR11";
            //string board = "TM101";
            //string workplace = "ASSPR11_03";
            //string ksk = "416T041901PR";
            
          //  List<string> modules = new List<string>() { "WI-0102", "WI-0201" };
            //List<string> modules = lc.GetModule(workplace, ksk);

            //List<string> basicmodules = lc.GetBasicModule(workplace, ksk);
            //HeadMessage msg = lc.GetHarnessByWorkplace(workplace);
            DateTime startTime = DateTime.Now;
            Console.WriteLine(startTime);
            // step 1
            HeadMessage msg = lc.StartAndGetHarnessByBoard(board, workplace);
            // lc.StartHarnessByBoard(board,workplace);
            // lc.StartHarness(board,workplace, msg.KSK);
            DateTime endTime = DateTime.Now;
            Console.WriteLine(endTime);
            //step 2
            // HeadMessage msg1 = lc.GetHarnessByWorkplace(workplace);

            //step 3
            foreach (var moduleNr in modules)
            {             
              //  LEPSAKMoudleResult result = lc.AKBasicModule(productLine, workplace, ksk, moduleNr);
            }

            // step 4
            DateTime endTime2 = DateTime.Now;
          //  HeadMessage msg2 = lc.CompleteHarness(board, workplace, ksk);

            Console.WriteLine(endTime2);
            Console.Read();
        }


        public  static void GetHeadRecord()
        {
            LEPSDataClassesDataContext dc = new LEPSDataClassesDataContext();
            string workpalce = "ASSEN11_03";
            string nodename = "ASSEN11_03";
            string appname = "AUTO_WORKSTATION";
            string username = "ass_user";


            string ksk = "416T041901EN";
            string kskType = "";
            string anline = "";
            string steering = "";
            string board = "TM101";

            int? processStatus = 0, result = null, manual_In = null, errorCode = null;
            long? bint_ErrorID = null;


            int i = dc.getHeadRecord(workpalce, nodename, appname, username,
                ref ksk, ref kskType, ref anline, ref steering, ref board,
                ref processStatus, ref result, ref manual_In, ref errorCode, ref bint_ErrorID);
            string s = "";
        }

        public static void SetHeadRecord() { 
         
        }

        public static void SumCheck() {

            //byte[] bytes = new byte[40]{
            //    0x31,0x31,0x30,0x43,0x38,0x31,0x36,0x31,0x32,0x33,0x34,0x41,0x42,0x43,0x44,0x32,0x33,0x37,0x38,0x41,0x46,0x43,0x44,0x43,0x44,0x45,0x46,0x31,0x46,0x35,0x41,0x43,0x41,0x46,0x45,0x46,0x30,0x43,0x31,0x03
            //};
            byte[] bytes = new byte[40]{
                0x31,0x31,0x30,0x43,0x38,0x31,0x36,0x31,0x32,0x33,0x34,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x03
            };
            byte b = 0x0;
            //  int num = 0;    
            for (int i = 0; i < bytes.Length; i++)
            {
                b += bytes[i];
                // num = (num + bytes[i]) % 0xffff; 
            }

            //  Console.WriteLine(num);
            byte[] memorySpage = BitConverter.GetBytes(b);

            List<char> chars = Convert.ToString(b, 16).ToCharArray().ToList<char>();


            //int ii=  BitConverter.ToInt16(new byte[] { memorySpage[0], memorySpage[1] }, 0);  

        
        }
    }
}


