using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLCLightCL.CusException
{
    public class LEPSTimeOutException : TimeoutException
    {
        public LEPSTimeOutException()
            : base("LEPS TimeoutException")
        {

        }
    }
}
