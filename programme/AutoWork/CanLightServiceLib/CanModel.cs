using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanLightServiceLib.Config;

namespace CanLightServiceLib
{
    public class CanModel
    {
        public string Id { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        private bool onLine = false;
        public bool OnLine
        {
            get { return onLine; }
            set
            {
                onLine = value;
                CanConfig.Save(this);
            }
        }
    }
}
