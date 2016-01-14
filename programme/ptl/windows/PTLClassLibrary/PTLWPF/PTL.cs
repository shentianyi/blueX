using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace PTLWPF
{
    public class PTL
    {
        public SerialPort sp{get;set;}
        public int readerAddress{get;set;}
        public List<string> nrs { get; set; }

        private bool success = false;
        public PTL() { }

        public PTL(SerialPort sp, int readerAddress)
        {
            this.sp = sp;
            this.readerAddress = readerAddress;
        }

        public void HandleMsg(byte[] bytes)
        {
            string data = byteToStringHex(bytes);
            if (data.Length > 2)
            {
                string crc_string = data.Substring(0, data.Length - 2);
                byte[] crc_bytes = GetCRCBytes(crc_string);
                string crc_check_string = byteToStringHex(crc_bytes);
                if (data.Equals(crc_bytes))
                {
                    this.success = true;
                    if (this.nrs.Count > 0)
                    {
                        this.nrs.RemoveAt(0);
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

            int totalTimes = nrs.Count;
            while (nrs.Count > 0)
            {
                success = false;
                int times = 0;
                while (times < 3 || !success)
                {
                    FindLabel(nrs.First());
                    Thread.Sleep(10);
                }

                totalTimes -= 1;
                if (totalTimes < 0) {
                    break;
                }
            }
        }

        public void FindLabels(List<string> nrs)
        {
            
            foreach (string nr in nrs)
            {
                FindLabel(nr);
              // Thread.Sleep(1000);
            }
        }


        public void CancelLabels(List<string> nrs)
        {
            foreach (string nr in nrs)
            {
                CancelLabel(nr);

               // Thread.Sleep(1000);
            }
        }

        public void FindLabel(string labelNr)
        {
            //    string cmd = "01 AD 01 00 00 05 B6"; 标签是： 1462
            string cmd = "0" + this.readerAddress + " AD 01 00 00 " + stringToStringHex(labelNr);
       
            byte[] bytes = GetCRCBytes(cmd);
            sp.Write(bytes, 0, bytes.Length);
        }
         
        public void CancelLabel(string labelNr) { 
            string cmd = "0" + this.readerAddress + " AE 01 00 00 " + stringToStringHex(labelNr);

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
            //hexString = hexString.Replace(" ", "");
            //if ((hexString.Length % 2) != 0)
            //    hexString += " ";
            //byte[] returnBytes = new byte[hexString.Length / 2];
            //for (int i = 0; i < returnBytes.Length; i++)
            //    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            //return returnBytes;
        }
        public   int parse(char c)
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
    }
}
