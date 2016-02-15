﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScmWcfService.Model.Enum
{
    public enum OrderBoxStatus
    {
        INIT = 100,
        PICKING = 200
    }

    public class OrderBoxStatusDisplay
    {
        public static string GetStatusOfOrderBox(OrderBoxStatus status)
        {
            string result = string.Empty;
            switch (status)
            {
                case OrderBoxStatus.INIT:
                    result = "新建";
                    break;
                case OrderBoxStatus.PICKING:
                    result = "择货";
                    break;
                default:
                    result = "N/A";
                    break;
            }
            return result;
        }
    }
}