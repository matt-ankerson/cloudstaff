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
    /// <remarks>
    /// String fields have been added in favour of relationships to other tables to prevent SQL reference errors
    /// when deleting records from status table.
    /// </remarks>
    public class StatusChangeLog
    {
        public int StatusChangeLogID { get; set; }
        public int UserID { get; set; }
        public int NewStatusID { get; set; }
        public int OldStatusID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime StatusChangeTimeStamp { get; set; }
        public DateTime StatusInitTimeStamp { get; set; } // The time that the previous status was started
    }
}