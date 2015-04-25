// Author: Matthew Ankerson
// Date: 25 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Holds IDs to User, Location and Status, amongst other metadata
    /// </summary>
    public class Core
    {
        public int CoreID { get; set; }
        public int UserID { get; set; }
        public int LocationID { get; set; }     // Will this be a compulsary field?
        public int StatusID { get; set; }
        public DateTime StateStart { get; set; }
        public DateTime StateEnd { get; set; }

        public virtual User User { get; set; }
        public virtual Status Status { get; set; }
        public virtual Location Location { get; set; }
    }
}