using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.ConfigUtil;

namespace ScmWcfService.Config
{
    public class BaseConfig
    {
        private static ConfigUtil config;

        static BaseConfig()
        {
            try
            {
                config = new ConfigUtil("Base", "Ini/base.ini");
                ForceWeightPass = bool.Parse(config.Get("ForceWeightPass"));
                PlayWeightSound = bool.Parse(config.Get("PlayWeightSound"));
                ImageExtends = config.Get("ImageExtends").Split(',').ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool ForceWeightPass { get; set; }
        public static bool PlayWeightSound { get; set; }
        public static List<string> ImageExtends { get; set; }
    }
}