using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WG3000_COMM.Core;
using ScmWcfService.Config;
using Brilliantech.Framwork.Utils.LogUtil;
using ScmWcfService.Model.Enum;
using ScmClient.ThridPart;
using RestSharp;

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
                    if (DoorConfig.Type == DoorType.Double)
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
                    else if (DoorConfig.Type == DoorType.Single)
                    {
                        SingleDoorTCPController singleController = new SingleDoorTCPController();
                        singleController.OpenIP(DoorConfig.IP, DoorConfig.Port);

                        bool result = singleController.Opendoor((byte)DoorConfig.DoorNo);

                        string msg = "成功";

                        if (!result)
                        {
                            switch (singleController.TCPLastError)
                            {
                                case 1: msg = "对象不存在"; break;
                                case 2: msg = "数据超出边界"; break;
                                case 3: msg = "操作超时"; break;
                                case 4: msg = "断开"; break;
                                case 5: msg = "返回数据错误"; break;
                                case 6: msg = "未知错误"; break;
                            }
                            LogUtil.Logger.Error(msg);
                        }
                        else {
                            LogUtil.Logger.Info(msg);
                        }

                        singleController.CloseTcpip();
                    }
                    else if (DoorConfig.Type == DoorType.TrickSingle) {
                        var client = new RestClient();
                        client.Timeout = 10000;
                        client.BaseUrl = "http://" + DoorConfig.IP+":"+DoorConfig.Port;

                        client.Authenticator = new HttpBasicAuthenticator(DoorConfig.User, DoorConfig.Password);

                        var req = new RestRequest("/cdor.cgi?open=1", Method.POST);
                        client.Execute(req);

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
