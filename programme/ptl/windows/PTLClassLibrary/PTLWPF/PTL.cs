using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using Brilliantech.Framwork.Utils.LogUtil;

namespace PTLWPF
{
    public enum CommandType
    {
        FIND,
        CANCEL,
        ALL_FIND,
        ALL_CANCEL
    }


    public class PTL
    {
        public SerialPort sp{get;set;}
        public string com { get; set; }
        public int baundRate { get; set; }
        public int readerAddress{get;set;}
        public CommandType cmdType { get; set; }
        public int retryTimes { get; set; }
        public int retryInterval { get; set; }

        public List<string> nrs { get; set; }
         
        private System.Timers.Timer timer;

        private Dictionary<string, int> executeTimesQ ;

    //    private int executeTimes = 0;

        public PTL() { }
        public PTL(string com,int baundRate=38400, int readerAddress=1,int retryTimes=6,int retryInterval=200)
        {
            this.com = com;
            this.baundRate = baundRate;
            this.readerAddress = readerAddress;
            this.retryTimes = retryTimes;
            this.retryInterval = retryInterval;
            this.OpenCom();
        }
         
        public void OpenCom()
        {
            try
            {
                this.sp = new SerialPort(this.com);
                this.sp.BaudRate = this.baundRate;
                sp.Open();
                sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                LogUtil.Logger.Info("COM Opend!");
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Debug(ex.Message);
            }
        }

        public void CloseCom()
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

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Byte[] recvbuf = new Byte[5];
            sp.Read(recvbuf, 0, recvbuf.Length);
            LogUtil.Logger.Info(recvbuf);
            HandleMsg(recvbuf);
        }

        public void HandleMsg(byte[] bytes)
        {
            string receive_string = byteToStringHex(bytes);
            LogUtil.Logger.Info(receive_string);
            if (receive_string.Length > 2)
            {
                string crc_string = receive_string.Substring(0, receive_string.Length - 2);
                byte[] crc_bytes = GetCRCBytes(crc_string);
                string crc_check_string = byteToStringHex(crc_bytes);
                LogUtil.Logger.Info("rcr string:" + crc_check_string);
                 
                // 是否通过CRC校验后
                if (receive_string.Equals(crc_check_string))
                {
                    // 判断是否是单标签返回

                    // 判断第四位是否是00 成功， 01失败
                    string success_fail_tag = receive_string.Substring(6, 2);

                    LogUtil.Logger.Info("success_fail_tag:" + success_fail_tag);

                    if (success_fail_tag.Equals("00"))
                    {
                        LogUtil.Logger.Info("【Success】" + success_fail_tag);

                        if (this.nrs != null && this.nrs.Count > 0)
                        {
                            if (this.executeTimesQ[this.nrs.First()] > 0)
                            {
                                this.nrs.RemoveAt(0);
                            }
                            //Thread.Sleep(100);

                            executeCmd();
                        }
                        else
                        {
                            if (timer != null)
                            {
                                timer.Stop();
                            }
                        }
                    }
                }
            }
        }

        public void ScanLabels()
        {
            throw new NotImplementedException();
        }


        public void FindLabels()
        {
            initVar();
        }

        public void CancelLabels()
        {
            initVar();
        }

        public void FindAllLabels() {
            initVar();
        }

        public void CancelAllLabels() {
            initVar();
        }

        public void initVar() {
            initExecuteQ();
            initTimer();
        }

        private void initExecuteQ()
        {
            executeTimesQ = new Dictionary<string, int>();
            if (CommandType.ALL_CANCEL == this.cmdType)
            {
                executeTimesQ.Add("ALL_CANCEL", 0);
            }
            else if (CommandType.ALL_FIND == this.cmdType) {
                executeTimesQ.Add("ALL_FIND",0);
            }
            else
            {
                foreach (string nnr in nrs)
                {
                    executeTimesQ.Add(nnr, 0);
                }
            }
        }

        private void initTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }

            timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = this.retryInterval;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            executeCmd();
        }


        public void FindLabel(string labelNr)
        {
            //    string cmd = "01 AD 01 00 00 05 B6"; 标签是： 1462
            string cmd = "0" + this.readerAddress + " AD 01 00 00 " + stringToStringHex(labelNr);
       
            byte[] bytes = GetCRCBytes(cmd);
            sp.Write(bytes, 0, bytes.Length);
        }
         
        public void CancelLabel(string labelNr) { 
            string cmd = getAddress() + " AE 01 00 00 " + stringToStringHex(labelNr);

            byte[] bytes = GetCRCBytes(cmd);
            sp.Write(bytes, 0, bytes.Length);
        }

        public void FindAll() {
            string cmd = getAddress() + " AD 00 00 00 ";

            byte[] bytes = GetCRCBytes(cmd);
            sp.Write(bytes, 0, bytes.Length);
        }

        public void CancelAll()
        {
            string cmd = getAddress() + " AE 00 00 00 ";

            byte[] bytes = GetCRCBytes(cmd);
            sp.Write(bytes, 0, bytes.Length);
        }

        public byte[] GetCRCBytes(string cmd)
        {
            byte[] baseb = hexStringToBytes(cmd);
            byte crc = CRC8(baseb);

            byte[] bytes = baseb;
            List<byte> blist = bytes.ToList();
            blist.Add(crc);
            bytes = blist.ToArray();
            return bytes;
        }

        public string byteToStringHex(byte[] bytes){
            string hex = string.Empty;
            foreach (byte b in bytes)
            {
                hex += b.ToString("X0").PadLeft(2, '0');
            }
            return hex;
        }

        public string stringToStringHex(string s)
        {
            return Convert.ToString(int.Parse(s), 16).PadLeft(4, '0');
        }
        public byte[] hexStringToBytes(string hexstr)
        {
            hexstr = hexstr.Replace(" ", "");
            byte[] b = new byte[hexstr.Length / 2];  
                int j = 0;  
                for (int i = 0; i < b.Length; i++)  
                {  
                    char c0 = hexstr[j++];  
                    char c1 = hexstr[j++];  
                    b[i] = (byte)((parse(c0) << 4) | parse(c1));  
                }  
                return b;  
        }
        public int parse(char c)
        {
            if (c >= 'a')
                return (c - 'a' + 10) & 0x0f;
            if (c >= 'A')
                return (c - 'A' + 10) & 0x0f;
            return (c - '0') & 0x0f;
        }


        public  byte CRC8(byte[] buffer)
        {
            byte crc = 0xFF;
            for (int j = 0; j < buffer.Length; j++)
            {
                crc ^= buffer[j];
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x01) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0x31;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }
            return crc;
        }

        private string getAddress() {
            return "0" + this.readerAddress;
        }

        private bool canExecuteCmd()
        {
            bool result = false;
            if (this.cmdType == CommandType.FIND || this.cmdType == CommandType.CANCEL)
            {
                if (this.nrs.Count > 0)
                {
                    if (this.executeTimesQ[this.nrs.First()] < this.retryTimes)
                    {
                        LogUtil.Logger.Info("【发送】 第" + this.executeTimesQ[this.nrs.First()] + "次");
                        this.executeTimesQ[this.nrs.First()] += 1;
                        result = true;
                    }
                    else
                    {
                        this.nrs.RemoveAt(0);
                        LogUtil.Logger.Info("【结束重试】 ");
                    }
                }
                else
                {
                    timer.Stop();
                    LogUtil.Logger.Info("【结束全部操作】 ");
                }
            }
            else if (this.cmdType == CommandType.ALL_CANCEL || this.cmdType==CommandType.ALL_FIND)
            {
                if (this.executeTimesQ.First().Value < this.retryTimes)
                {
                    this.executeTimesQ[this.executeTimesQ.First().Key] += 1;
                    result = true;
                }
                else
                {
                    timer.Stop();
                    LogUtil.Logger.Info("【结束次重试】 ");
                    LogUtil.Logger.Info("【结束全部操作】 ");
                }
            }
            return result;
        }

        private void executeCmd()
        {
            if (this.cmdType == CommandType.FIND)
            {
                if (canExecuteCmd())
                {
                    LogUtil.Logger.Info("【find】 " + nrs.First());
                    FindLabel(nrs.First());
                }
            }
            else if (this.cmdType == CommandType.CANCEL)
            {
                if (canExecuteCmd())
                {
                    LogUtil.Logger.Info("【cancel】 " + nrs.First());
                    CancelLabel(nrs.First());
                }
            }
            else if (this.cmdType == CommandType.ALL_FIND) 
            {
                if (canExecuteCmd())
                {
                    LogUtil.Logger.Info("【find all】 ");
                    FindAll();
                }
            }
            else if (this.cmdType == CommandType.ALL_CANCEL)
            {
                if (canExecuteCmd())
                {
                    LogUtil.Logger.Info("【cancel all】 ");
                    CancelAll();
                }
            }
        }
    }
}
