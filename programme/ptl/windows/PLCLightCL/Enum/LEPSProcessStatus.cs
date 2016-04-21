using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLCLightCL.Enum
{
    public enum LEPSProcessStatus
    {
        NULL=-1,
        INIT=0,
        REQUEST_START=60,
        PORCESSING=64,
        COMPLETE=70
    }
}
