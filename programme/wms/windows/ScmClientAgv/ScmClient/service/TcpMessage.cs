using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScmClient.service
{
    //[DataContract]
    public class TcpMessage<T>
    {
        public TcpMessage() { }
        public TcpMessage(Boolean result)
        {
            this.result = result;
        }
        public Boolean result { get; set; }

        public string content { get; set; }

        //[DataMember]
        public T data { get; set; }
    }
}
