using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ScmWcfService.Model
{

    [DataContract]
    public class Warehouse
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string nr { get; set; }
    }
}