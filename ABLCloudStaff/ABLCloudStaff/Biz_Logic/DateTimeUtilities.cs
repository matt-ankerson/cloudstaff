// Author: Matt Ankerson
// Date: 3 August 2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Biz_Logic
{
    public static class DateTimeUtilities
    {
        public static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
    }

    /// <summary>
    /// Object to hold a timestamp, transportable via json.
    /// </summary>
    public class TimeInfo
    {
        public string numeric_repr;
        public string dateString;
        public string year;
        public string month;
        public string day;
        public string hour;
        public string minute;
        public string second;
    }
}