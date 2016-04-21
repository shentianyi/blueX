using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.Ports;
using Brilliantech.Framwork.Utils.LogUtil;
using System.Threading;

namespace PLCLightCL
{
    public enum CommandType
    {
        ON,
        OFF,
        BLINK,
        ALL_OFF_BEFORE_ON,
        ALL_OFF,
        ALL_ON
    }

    public class PlcLightController
    {
        const int lightCount = 48;
        static byte[] cmdPrefix = new byte[8] { 0x02, 0x31, 0x31, 0x30, 0x43, 0x38, 0x31, 0x36 };
        static byte[] cmdPostfix = new byte[3] { 0x03, 0x00, 0x00 };

        public SerialPort sp { get; set; }
        public string com { get; set; }
        public int baundRate { get; set; }

        public PlcLightController() { }

        private byte[] previousLightStates;
        private List<CommandType> validateIndexCmds;

        public PlcLightController(string com, int baundRate=9600)
        {
            this.previousLightStates = this.GetInitLightStates();
            this.com = com;
            this.baundRate = baundRate;
            this.OpenCom();
        }

        /// <summary>
        /// operate lights
        /// </summary>
        /// <param name="indexes">light index list, start from 0 to 127</param>
        /// <param name="cmdType">cmd type</param>
        public void Play(CommandType cmdType, List<int> indexes = null)
        {
            // init light state from previous light states
            byte[] lightStates = new byte[lightCount];
            for (int i = 0; i < lightCount; i++)
            {
                lightStates[i] = previousLightStates[i];
            }

            // validate light index
            if (this.ValidateIndexCmds.Contains(cmdType))
            {
                if (indexes == null || indexes.Count == 0)
                {
                    throw new IndexOutOfRangeException("No Light Index!");
                }
                foreach (int i in indexes)
                {
                    if (i < 0 || i >= lightCount)
                    {
                        throw new IndexOutOfRangeException("light index out of range, should between 0 and 127");
                    }
                }
                indexes = indexes.Distinct().ToList();
            }

            // switch cmd type
            switch (cmdType)
            {
                case CommandType.ON:
                    foreach (int i in indexes)
                    {
                        lightStates[i] = 1;
                    }
                    break;
                case CommandType.OFF:
                    foreach (int i in indexes)
                    {
                        lightStates[i] = 0;
                    }
                    break;
                case CommandType.ALL_OFF_BEFORE_ON:
                    this.Play(CommandType.ALL_OFF);
                    Thread.Sleep(100);
                    this.Play(CommandType.ON, indexes);
                    return; 
                case CommandType.ALL_ON:
                    for (int i = 0; i < lightCount; i++)
                    {
                        lightStates[i] = 1;
                    }
                    break;
                case CommandType.ALL_OFF: 
                    for (int i = 0; i < lightCount; i++)
                    {
                        lightStates[i] = 0;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            this.SendCmd(lightStates);
            for (int i = 0; i < lightCount; i++)
            {
                if (previousLightStates[i] != lightStates[i])
                {
                    previousLightStates[i] = lightStates[i];
                }
            }
        }

        private void SendCmd(byte[] lightStates)
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
                OpenCom(false);
            }
            catch (Exception e)
            {
                throw e;
                // string ex = e.Message; 
            }
           // string sssssss = "s";

        }

        private byte[] GetSumCheck(byte[] bytes)
        {
            byte b = 0x0;
            byte[] check = new byte[2];
            for (int i = 0; i < bytes.Length; i++)
            {
                b += bytes[i];
            }
            List<char> chars = Convert.ToString(b, 16).ToUpper().ToCharArray().ToList<char>();
            if (chars.Count == 1)
            {
                check[0] = 0x30;
            }
            else
            {
                check[0] = (byte)chars[0];
                check[1] = (byte)chars[1];
            }
            return check;
        }

        private string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF"
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2") + " ");
                }

                hexString = strB.ToString().TrimEnd();
            }
            return hexString;
        }



        private string ByteToString(byte[] bytes) // 0xae00cf => "AE00CF"
        {
            string tmp = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString());
                }

                tmp = strB.ToString().TrimEnd();
            }
            return tmp;
        }


        private List<CommandType> ValidateIndexCmds
        {
            get
            {
                if (this.validateIndexCmds == null)
                {
                    this.validateIndexCmds = new List<CommandType>() { CommandType.ON, CommandType.OFF, CommandType.ALL_OFF_BEFORE_ON };
                }
                return this.validateIndexCmds;
            }
        }

        private byte[] GetInitLightStates() 
        {
            byte[] lightStates = new byte[lightCount];

            for (int i = 0; i < lightCount; i++)
            {
                lightStates[i] = 0;
            }
            return lightStates;
        }



        /// <summary>
        /// close plc light
        /// </summary>
        public void Close()
        {
            this.CloseCom();
        }

        private void OpenCom(bool throwEx=true)
        {
            try
            {
                this.sp = new SerialPort(this.com, this.baundRate);
                this.sp.BaudRate = this.baundRate;
                sp.Open();
                LogUtil.Logger.Info("COM Opend!");
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Debug(ex.Message);
                if (throwEx)
                {
                    throw ex;
                }
            }
        }


        private void CloseCom()
        {
            try
            {
                if (sp != null)
                {
                    sp.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Debug(ex.Message);
            }
        }
    }
}
