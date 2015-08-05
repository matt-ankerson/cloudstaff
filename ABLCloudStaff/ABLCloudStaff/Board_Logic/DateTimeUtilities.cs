// Author: Matt Ankerson
// Date: 3 August 2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Board_Logic
{
    public static class DateTimeUtilities
    {
        public static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
    }
}