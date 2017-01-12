using Brilliantech.Framwork.Utils.ConfigUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScmWcfService.Config
{
    public class ServerConfig
    {
        private static ConfigUtil config;
        //private static string agvHost;
        //private static string ptlHost;

        static ServerConfig()
        {
            try
            {
                config = new ConfigUtil("SERVER", "Ini/server.ini");

                agvHost = config.Get("AgvHost");
                agvPort = int.Parse(config.Get("AgvPort"));
                ptlHost = config.Get("PtlHost");
                ptlPort = int.Parse(config.Get("PtlPort"));
                agvCarNr = int.Parse(config.Get("AgvCarNr"));
                ptlComPort = config.Get("PtlComPort");
                PtlSwitch = int.Parse(config.Get("PtlSwitch"));
                AgvSwitch = int.Parse(config.Get("AgvSwitch"));
                ButtonLedSwitch = int.Parse(config.Get("ButtonLedSwitch"));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string agvHost { get; set; }
        public static int agvPort { get; set; }
        public static string ptlHost { get; set; }
        public static int ptlPort { get; set; }
        public static int agvCarNr { get; set; }
        public static string ptlComPort { get; set; }

        public static int PtlSwitch { get; set; }
        public static int AgvSwitch { get; set; }
        public static int ButtonLedSwitch { get; set; }
    }
}