using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ScmWcfService.Config;

namespace ScmClient.RFID
{
    public class Parser
    {
        static Regex messageRegex = new Regex(RFIDConfig.MessageRegex);
        static Regex labelRegex = new Regex(RFIDConfig.LabelRegex);
        static Regex carRegex = new Regex(RFIDConfig.OrderCarLabelRegex);
        static Regex boxRegex = new Regex(RFIDConfig.OrderBoxLabelRegex);

        public static List<RFIDMessage> StringToList(string data)
        {
            List<RFIDMessage> list = new List<RFIDMessage>();
            List<string> dataList = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string d in dataList)
            {
                string dd = d.Replace(" ","");

                if (RFIDConfig.USE_DLL)
                {
                    dd = new String(d.Skip(6).Take(38).ToArray()).Replace(" ", "");
                    if (messageRegex.Match(dd).Success)
                    {
                        RFIDMessage msg = StringToMessage(new String(dd.Skip(0).Take(4).ToArray()));
                        if (msg != null)
                        {
                            list.Add(msg);
                        }
                    }
                }
                else
                {
                    if (messageRegex.Match(dd).Success)
                    {
                        RFIDMessage msg = StringToMessage(new String(dd.Skip(4).Take(4).ToArray()));
                        if (msg != null)
                        {
                            list.Add(msg);
                        }
                    }
                }
            }
            return list;
        }

        public static RFIDMessage StringToMessage(string data)
        {
            RFIDMessage message = null;
            if (labelRegex.Match(data).Success)
            {
                Match match = carRegex.Match(data);
                MessageType type = MessageType.NA;
                if (match.Success)
                {
                    type = MessageType.CAR;
                }
                else
                {
                    match = boxRegex.Match(data);
                    if (match.Success)
                    {
                        type = MessageType.BOX;
                    }
                }
                if (type != MessageType.NA)
                {
                    message = new RFIDMessage() { Nr = match.Value, Body = data, Type = type };
                }
            }
            return message;
        }
    }
}
