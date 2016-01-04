using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScmClient.RFID
{
    public class RFIDMessage
    {
        public string Nr { get; set; }
        public string Body { get; set; }
        public string Ant { get; set; }
        public int Times { get; set; }
        public MessageType Type { get; set; }
    }
}
