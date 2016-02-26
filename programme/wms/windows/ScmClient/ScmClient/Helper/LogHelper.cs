using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace ScmClient.Helper
{
    public class LogHelper
    {
        private static ILog logger;

        static LogHelper()
        {
            //  log4net.Config.XmlConfigurator.Configure();
            XmlConfigurator.Configure();

            logger = LogManager.GetLogger(typeof(LogHelper));

            logger.Logger.IsEnabledFor(log4net.Core.Level.All);

        }

        public static ILog Logger
        {
            get { return LogHelper.logger; }
        }
    }
}
