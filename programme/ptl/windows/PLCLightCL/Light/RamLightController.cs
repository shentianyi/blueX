using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLCLightCL.Light
{
    public class RamLightController : LightController
    {

        const int _lightCount = 112;
        static byte[] cmdPrefix = new byte[1] { 0xAA };
        
        public RamLightController(string com, int baundRate = 9600)
            : base(com, baundRate, _lightCount)
        { 
        }
        
        protected override void SendCmd(byte[] lightStates)
        {
            int lightCmdLength = (lightCount / 8);
            byte[] cmd = new byte[cmdPrefix.Length+lightCmdLength];
            for (int i = 0; i < cmdPrefix.Length; i++)
            {
                cmd[i] = cmdPrefix[i];
            }
            int m = 0;
            int m1 = 0;
            for (int i = cmdPrefix.Length; m1 < lightCmdLength; i++)
            {
                cmd[i] = 0x00;
                m1++;
            }
            byte[] stateCmd = new byte[lightCount / 8];
            // init state cmd
            for (var i = 0; i < stateCmd.Length; i++)
            {
                stateCmd[i] = 0x00;
            }
            for (int i = 0; i < lightStates.Length / 8;   i++) {
                byte[] g = lightStates.Skip(8 * i).Take(8).ToArray();
                string s1 = new string(ByteToString(g).Reverse().ToArray());
                int i1 = Convert.ToInt32(s1, 2);
                stateCmd[m] = (byte)i1;
                m++;
            }
            for (int i = 0; i < stateCmd.Length; i++)
            {
                cmd[cmdPrefix.Length + i] = stateCmd[i];
            }

            try
            {
                sp.Write(cmd, 0, cmd.Length);
            }
            catch (InvalidOperationException e)
            {
                Open(false);
            }
            catch (Exception e)
            {
                throw e;
                // string ex = e.Message; 
            }
        }


    }
}
