using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScmClient.RFID
{
    public class Parser
    {
        static Regex messageRegex = new Regex(@"[A-Z0-9]{24}");
        static Regex labelRegex = new Regex(@"[A-Z0-9]{4}");
        static Regex carRegex = new Regex(@"^[A-E](\w{3})");
        static Regex boxRegex = new Regex(@"^[F-Z0-9](\w{3})");

        public static List<RFIDMessage> StringToList(string data)
        {
            List<RFIDMessage> list = new List<RFIDMessage>();
            List<string> dataList = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string d in dataList)
            {
                string dd = new String(d.Skip(8).Take(36).ToArray()).Replace(" ", "");
                if (messageRegex.Match(dd).Success)
                {
                    RFIDMessage msg = StringToMessage(new String(dd.Take(4).ToArray()));
                    if (msg != null)
                    {
                        list.Add(msg);
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
