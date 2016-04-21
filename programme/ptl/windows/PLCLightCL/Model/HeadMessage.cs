using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLCLightCL.Enum;

namespace PLCLightCL.Model
{
    public class HeadMessage
    {
        public string KSK { get; set; }
        public string KSKType { get; set; }
        public string ANLIE { get; set; }
        public string Steering { get; set; }
        public string Board { get; set; }

        public LEPSProcessStatus ProcessStatus { get; set; }
        public int? Result { get; set; }
        public int? ManualIn { get; set; }
        public int? ErrorCode { get; set; }
        public long? BintErrorID { get; set; }

        public int ReturnValue { get; set; }
    }
}
