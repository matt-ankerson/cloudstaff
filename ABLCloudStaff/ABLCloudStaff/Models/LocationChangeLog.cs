// Author: Matt Ankerson
// Date: 8 July 2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Every time a username changes Location, it is logged here.
    /// </summary>
    public class LocationChangeLog
    {
        public int LocationChangeLogID { get; set; }
        public int UserID { get; set; }
        public int NewLocationID { get; set; }
        public int OldLocationID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OldLocation { get; set; }
        public string NewLocation { get; set; }
        public DateTime LocationChangeTimeStamp { get; set; }
        public DateTime LocationInitTimeStamp { get; set; } // The time that the previous location was started
    }
}