using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.ConfigUtil;

namespace CanLightServiceLib.Config
{
    public class CanConfig
    {

        private static ConfigUtil canConfig;

        private static List<CanModel> canModels;
        static CanConfig()
        {
            try
            {
                canConfig = new ConfigUtil("Config/can.ini");
                List<string> ids = canConfig.Notes();
                canModels = new List<CanModel>();
                foreach (var id in ids)
                {
                    canModels.Add(new CanModel()
                    {
                        Id = canConfig.Get("Id", id),
                        IP = canConfig.Get("IP", id),
                        Port = int.Parse(canConfig.Get("Port", id)),
                        UniqKey = canConfig.Get("IP", id) + ":" + canConfig.Get("Port", id),
                        OnLine = false
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CanModel> CanModels
        {
            get
            {
                return canModels;
            }

            set
            {
                canModels = value;
            }
        }

        public  static void Save(CanModel canModel)
        {
            canConfig.Set("IP", canModel.IP, canModel.Id);
            canConfig.Set("Port", canModel.Port, canModel.Id);
            canConfig.Set("OnLine", canModel.OnLine, canModel.Id);
            canConfig.Save();
        }

        public static CanModel FindCan(string id)
        {
            return canModels.FirstOrDefault(s => s.Id == id);
        }
    }
}
