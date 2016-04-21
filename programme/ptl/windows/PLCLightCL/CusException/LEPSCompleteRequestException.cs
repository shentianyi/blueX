using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLCLightCL.CusException
{
    public class LEPSCompleteRequestException : InvalidOperationException
    {
        public LEPSCompleteRequestException()
            : base("LEPS Complete Request Error")
        {

        }
    }
}
