using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.Ports;
using Brilliantech.Framwork.Utils.LogUtil;
using System.Threading;
using PLCLightCL.Enum;

namespace PLCLightCL.Light
{
    public class PlcLightController : LightController
    {
       const int _lightCount = 48;
        static byte[] cmdPrefix = new byte[8] { 0x02, 0x31, 0x31, 0x30, 0x43, 0x38, 0x31, 0x36 };
        static byte[] cmdPostfix = new byte[3] { 0x03, 0x00, 0x00 };

       


        public PlcLightController(string com, int baundRate = 9600)
            : base(com, baundRate,_lightCount)
        {
        }

        /// <summary>
        /// operate lights
        /// </summary>
        /// <param name="indexes">light index list, start from 0 to 127</param>
        /// <param name="cmdType">cmd type</param>
        //public  void Play(LightCmdType cmdType, List<int> indexes = null)
        //{
          
        //}

        protected override void SendCmd(byte[] lightStates)
        {
            int lightCmdLength=(lightCount / 4);
            byte[] cmd = new byte[cmdPrefix.Length + lightCmdLength+ cmdPostfix.Length];
            for (int i = 0; i < cmdPrefix.Length; i++)
            {
                cmd[i] = cmdPrefix[i];
            }
            int m1 = 0;
            for(int i=cmdPrefix.Length;m1<lightCmdLength;i++){
                cmd[i] = 0x30;
                m1++;
            }
            int m2=0;
            for (int i = cmdPrefix.Length + lightCmdLength; m2 < cmdPostfix.Length; i++)
            {
                cmd[i] = cmdPostfix[m2];
                m2++;
            }

            byte[] stateCmd = new byte[lightCount/4];
            // init state cmd
            for (var i = 0; i < stateCmd.Length; i++) {
                stateCmd[i] = 0x30;
            }
            int m = 0;
            for (int j = 0; j < stateCmd.Length/4; j++)
            {
                byte[] light = new byte[16];
                int highGroup = j * 2 + 1;
                int lowGroup = j * 2;

                # region init high/low group
                int n = 0;
                for (int i = highGroup * 8; i < (highGroup + 1) * 8; i++)
                {
                    light[n] = lightStates[i];
                    n++;
                }
                for (int i = lowGroup * 8; i < (lowGroup + 1) * 8; i++)
                {
                    light[n] = lightStates[i];
                    n++;
                }
                #endregion


               //  string s1 = "0000001000110100";
              
               string s1 = new string(ByteToString(light).Reverse().ToArray());
                int i1 = Convert.ToInt32(s1, 2);
                List<char> chars = Convert.ToString(i1, 16).ToUpper().ToCharArray().ToList<char>();

                int lack = 4 - chars.Count;
                if (lack >0) {
                    for (int i = 0; i < lack; i++) {
                        chars.Insert(i, '0');
                    }
                }
                foreach (char c in chars)
                {
                  //  int value = Convert.ToInt32(c);
                  //  string hexOutput = String.Format("{0:X}", value);

                    stateCmd[m] = (byte)c;//Convert.ToByte(hexOutput);// (byte)c;//(byte)Convert.ToInt32(c.ToString(),16);
                    m++;

                }
            }
            for (int i = 0; i < stateCmd.Length; i++) {
               cmd[8 + i] = stateCmd[i];
            }
            byte[] bb = cmd.Skip(1).Take(cmd.Length - 3).ToArray();
            byte[] check = GetSumCheck(bb);
            cmd[cmd.Length - 2] = check[0];
            cmd[cmd.Length - 1] = check[1];

            try
            {
                sp.Write(cmd, 0, cmd.Length);
            }
            catch (InvalidOperationException e) {
                Open(false);
            }
            catch (Exception e)
            {
                throw e;
                // string ex = e.Message; 
            }
           // string sssssss = "s";

        }
    }
}
