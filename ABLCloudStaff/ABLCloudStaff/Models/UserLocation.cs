// Author: Matthew Ankerson
// Date: 25 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Holds information about a single User-Location relationship
    /// </summary>
    public class UserLocation
    {
        public int UserLocationID { get; set; }
        public int UserID { get; set; }
        public int LocationID { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual User User { get; set; }
        public virtual Location Location { get; set; }
    }
}