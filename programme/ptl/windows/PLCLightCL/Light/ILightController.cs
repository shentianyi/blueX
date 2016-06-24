using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLCLightCL.Enum;

namespace PLCLightCL.Light
{
    public interface ILightController
    {
        void Play(LightCmdType cmdType, List<int> indexes = null);
        void Close();
    }
}
