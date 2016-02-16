using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.ConfigUtil;

namespace ScmWcfService.Config
{
    public class RFIDConfig
    {
        private static ConfigUtil config;
       
        static RFIDConfig()
        {
            try
            {
                config = new ConfigUtil("RFID", "Ini/rfid.ini");

                USE_DLL = bool.Parse(config.Get("USE_DLL"));

                RFIDCOM = config.Get("RFIDCOM");
                RFIDBaudRate = int.Parse(config.Get("RFIDBaudRate"));
                RFIDInterVal = int.Parse(config.Get("RFIDInterVal"));
                
                MessageRegex = config.Get("MessageRegex");
                LabelRegex = config.Get("LabelRegex");
                OrderCarLabelRegex = config.Get("OrderCarLabelRegex");
                OrderBoxLabelRegex = config.Get("OrderBoxLabelRegex");
                OutAutoLoadPick = bool.Parse(config.Get("OutAutoLoadPick"));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool USE_DLL { get; set; }
        public static string MessageRegex { get; set; }
        public static string LabelRegex { get; set; }
        public static string OrderCarLabelRegex { get; set; }
        public static string OrderBoxLabelRegex { get; set; }
        public static string RFIDCOM { get; set; }
        public static int RFIDBaudRate { get; set; }
        public static int RFIDInterVal { get; set; }
        public static bool OutAutoLoadPick { get; set; }
    }
}