// Author: Matthew Ankerson
// Date: 25 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Holds information about a single User-Status relationship
    /// </summary>
    public class UserStatus
    {
        public int UserStatusID { get; set; }
        public int UserID { get; set; }
        public int StatusID { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual User User { get; set; }
        public virtual Status Status { get; set; }
    }
}