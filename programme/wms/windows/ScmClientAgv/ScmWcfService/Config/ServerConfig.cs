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
                agvPort = config.Get("AgvPort");
                ptlHost = config.Get("PtlHost");
                ptlPort = config.Get("AgvPort");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string agvHost { get; set; }
        public static string agvPort { get; set; }
        public static string ptlHost { get; set; }
        public static string ptlPort { get; set; }
    }
}