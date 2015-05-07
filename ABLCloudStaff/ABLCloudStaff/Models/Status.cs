// Author: Matthew Ankerson
// Date: 25 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Holds information about a single status
    /// </summary>
    public class Status
    {
        public int StatusID { get; set; }
        public string Name { get; set; }

        public virtual List<UserStatus> UserStatuses { get; set; }
    }
}