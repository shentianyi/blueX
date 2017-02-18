using Brilliantech.Framwork.Utils.LogUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using CanLightServiceLib;
using CanLightService.Properties;
using CanLightServiceLib.Config;

namespace CanLightService
{
    public partial class CanLightService : ServiceBase
    {
       
        public CanLightService()
        {
            InitializeComponent();
        }

        private CanLightTcpServer server;

        protected override void OnStart(string[] args)
        {
            try
            {
                LogUtil.Logger.InfoFormat("IP:{0} PORT:{1} 服务启动中....",Settings.Default.ServiceIP,Settings.Default.ServicePort);

                server = new CanLightTcpServer(Settings.Default.ServiceIP, Settings.Default.ServicePort, CanConfig.CanModels);
                server.Start();

                LogUtil.Logger.Info("服务启动【成功】");
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error("服务启动【失败】", ex);
                if (this.CanStop)
                {
                    this.Stop();
                }
            }
        }

         

       

        protected override void OnStop()
        {
            try
            {
                LogUtil.Logger.Info("服务停止中....");
                server.Stop();

                LogUtil.Logger.Info("服务停止【成功】");
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error("服务停止【失败】",ex);
            }
        }
    }
}
