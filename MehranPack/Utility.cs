using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MehranPack
{
    public class Utility
    {
        public static DateTime AdjustTimeOfDate(DateTime input)
        {
            TimeSpan ts = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            input += ts;
            return input;
        }
    }
}