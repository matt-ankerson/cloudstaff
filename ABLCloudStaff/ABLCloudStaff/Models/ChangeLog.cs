// Author: Matt Ankerson
// Date: 1 July 2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Every time a user changes their state (status or location), it is logged here.
    /// </summary>
    public class ChangeLog
    {
        public int ChangeLogID { get; set; }
        public int UserID { get; set; }
        public int NewStatusID { get; set; }
        public int OldStatusID { get; set; }
        public int NewLocationID { get; set; }
        public int OldLocationID { get; set; }
        public DateTime StateChangeTimeStamp { get; set; }
        public DateTime StateInitTimeStamp { get; set; } // Logs the time that the previous state was started

        public virtual User User { get; set; }
        public virtual Status NewStatus { get; set; }
        public virtual Status OldStatus { get; set; }
        public virtual Location NewLocation { get; set; }
        public virtual Location OldLocation { get; set; }
    }
}