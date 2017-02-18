using System;
using AutoWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCLightCL.Enum;

namespace UnitTestProjectForAutoWork
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            LightHelper helper = new LightHelper();
            helper.Play(LightCmdType.ALL_OFF, "ASSEN11_01");

        }
    }
}
