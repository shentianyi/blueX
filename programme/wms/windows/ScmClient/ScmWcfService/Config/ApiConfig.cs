using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.ConfigUtil;

namespace ScmWcfService.Config
{
    public class ApiConfig
    {
        private static ConfigUtil config;
        private static string host;
        private static string port;

        static ApiConfig()
        {
            try
            {
                config = new ConfigUtil("API", "Ini/api.ini");
                
                host = config.Get("Host"); 
                ApiUri = config.Get("ApiUri");
                BaseUri = host + ApiUri;

                LoginAction = config.Get("LoginAction");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
      
        public static string Host
        {
            get { return host; }
            set
            {
                host = value;
                BaseUri =  host + ApiUri;
                config.Set("Host", value);
                config.Save();
            }
        }
        

        public static string ApiUri { get; set; }
        public static string BaseUri { get; set; }
        public static string LoginAction { get; set; }
    }
}