using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScmClient.RFID
{
    public class Parser
    {
        static Regex labelRegex = new Regex(@"[A-E0-9]{24}");
        static Regex carRegex = new Regex(@"^[A-E](\w{3})");
        static Regex boxRegex = new Regex(@"^[F-Z0-9](\w{3})");

        public static List<RFIDMessage> StringToList(string data)
        {
            List<RFIDMessage> list = new List<RFIDMessage>();
            List<string> dataList = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string d in dataList)
            {
                string message = new String(d.Skip(8).Take(36).ToArray()).Replace(" ", "");

                if (labelRegex.Match(message).Success)
                {
                    Match match = carRegex.Match(message);
                    MessageType type = MessageType.NA;
                    if (match.Success)
                    {
                        type = MessageType.CAR;
                    }
                    else
                    {
                        match = boxRegex.Match(message);
                        if (match.Success)
                        {
                            type = MessageType.BOX;
                        }
                    }
                    if (type != MessageType.NA)
                    {
                        RFIDMessage msg = new RFIDMessage() { Nr = match.Value, Body = message, Type = type };
                        list.Add(msg);
                    }
                }
            }


            return list;
        }
    }
}
