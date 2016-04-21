using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLCLightCL.CusException
{
    public class LEPSStartRequestException : InvalidOperationException
    {
        public LEPSStartRequestException()
            : base("LEPS Start Request Error")
        {

        }
    }
}
