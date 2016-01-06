using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScmWcfService.Model.Enum
{
    public enum OrderStatus
    {
        INIT,
        ABORTED,
        PICKING,
        PICKED,
        DELIVERYED
    }
}