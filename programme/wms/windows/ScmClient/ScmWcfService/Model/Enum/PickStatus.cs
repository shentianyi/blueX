using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScmWcfService.Model.Enum
{
    public enum PickStatus
    {
        INIT=100,
        PICKING=200,
        PICKED=300,
        ABORTED=400
    }

    public class PickStatusDisplay {
        public static string GetStatusOfPick(PickStatus status) {
            string result = string.Empty;
            switch (status) { 
                case PickStatus.INIT:
                    result = "NEW";
                    break;
                case PickStatus.PICKING:
                    result = "PICKING";
                    break;
                case PickStatus.PICKED:
                    result = "PICKED";
                    break;
                case PickStatus.ABORTED:
                    result = "ABORTED";
                    break;
                default:
                    result = "N/A";
                    break;
            }
            return result;
        }
    }
}