// Author: Matt Ankerson
// Date: 8 July 2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Every time a username changes status, it is logged here
    /// </summary>
    public class StatusChangeLog
    {
        public int StatusChangeLogID { get; set; }
        public int UserID { get; set; }
        public int NewStatusID { get; set; }
        public int OldStatusID { get; set; }
        public DateTime StatusChangeTimeStamp { get; set; }
        public DateTime StatusInitTimeStamp { get; set; } // The time that the previous status was started

        public virtual User User { get; set; }
        public virtual Status NewStatus { get; set; }
        public virtual Status OldStatus { get; set; }
    }
}