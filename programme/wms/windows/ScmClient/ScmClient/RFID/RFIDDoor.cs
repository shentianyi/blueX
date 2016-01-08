using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WG3000_COMM.Core;
using ScmWcfService.Config;
using Brilliantech.Framwork.Utils.LogUtil;

namespace ScmClient.RFID
{
    public class RFIDDoor
    {
        public void OpenDoor()
        {
            try
            {
                if (DoorConfig.Enabled)
                {
                    wgMjController wgMjController = new wgMjController();
                    wgMjController.ControllerSN = DoorConfig.SN;
                    wgMjController.IP = DoorConfig.IP;
                    wgMjController.PORT = DoorConfig.Port;

                    if (wgMjController.RemoteOpenDoorIP(DoorConfig.DoorNo) > 0)
                    {
                        LogUtil.Logger.Info("打开门成功！");
                    }
                    else
                    {
                        LogUtil.Logger.Error("打开门失败！");
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Logger.Error("打开门失败！");
                LogUtil.Logger.Error(e.Message);
            }
        }
    }
}
