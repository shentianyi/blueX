using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLCLightCL.Enum;
using System.IO.Ports;
using Brilliantech.Framwork.Utils.LogUtil;
using System.Threading;

namespace PLCLightCL.Light
{
    public class LightController:ILightController
    {

        protected int lightCount = 48;
        protected byte[] previousLightStates;
        protected List<LightCmdType> validateIndexCmds;

        public SerialPort sp { get; set; }
        public string com { get; set; }
        public int baundRate { get; set; }

        public LightController() { }

        public LightController(string com, int baundRate = 9600) {
            this.com = com;
            this.baundRate = baundRate;
            this.OpenCom();
        }

        public LightController(string com, int baundRate = 9600,int lightCount=48):this(com,baundRate)
        {
            this.lightCount = lightCount;
            this.previousLightStates = this.GetInitLightStates();
        }

        public  void Play(LightCmdType cmdType, List<int> indexes = null)
        {
            //throw new NotImplementedException();
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
                case LightCmdType.ON:
                    foreach (int i in indexes)
                    {
                        lightStates[i] = 1;
                    }
                    break;
                case LightCmdType.OFF:
                    foreach (int i in indexes)
                    {
                        lightStates[i] = 0;
                    }
                    break;
                case LightCmdType.ALL_OFF_BEFORE_ON:
                    this.Play(LightCmdType.ALL_OFF);
                    Thread.Sleep(100);
                    this.Play(LightCmdType.ON, indexes);
                    return;
                case LightCmdType.ALL_ON:
                    for (int i = 0; i < lightCount; i++)
                    {
                        lightStates[i] = 1;
                    }
                    break;
                case LightCmdType.ALL_OFF:
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

        protected virtual void SendCmd(byte[] lightStates) {
            //string s = "";
        }

        protected byte[] GetSumCheck(byte[] bytes)
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

        protected string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF"
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



        protected string ByteToString(byte[] bytes) // 0xae00cf => "AE00CF"
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


        protected List<LightCmdType> ValidateIndexCmds
        {
            get
            {
                if (this.validateIndexCmds == null)
                {
                    this.validateIndexCmds = new List<LightCmdType>() { LightCmdType.ON, LightCmdType.OFF, LightCmdType.ALL_OFF_BEFORE_ON };
                }
                return this.validateIndexCmds;
            }
        }

        protected byte[] GetInitLightStates()
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

        protected void OpenCom(bool throwEx = true)
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


        protected void CloseCom()
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
