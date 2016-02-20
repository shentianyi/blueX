using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ScmWcfService.Config;

namespace ScmClient.Helper
{
    public class PathHelper
    {
        public static string GetImagePath(string fileName)
        {
           fileName= Path.GetFileName(fileName);
           foreach (string extend in BaseConfig.ImageExtends)
           {
               string path = GetFullPath(Path.Combine("images", fileName+extend));
               if (File.Exists(path))
               {
                   return path;
               }
           }
           return null;
        }

        public static string GetSoundPath(string fileName)
        {
            string path= GetFullPath(Path.Combine("sounds", fileName));
            if (File.Exists(path))
            {
                return path;
            }
            return null;
        }

        public static string GetFullPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }
    }
}
