// Author: Matt Ankerson
// Date: 9 November 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Holds location IDs inidcating the system's default locations for adding new users
    /// </summary>
    public class DefaultLocation
    {
        public int DefaultLocationID { get; set; }
        public int LocationID { get; set; }

        public virtual Location Location { get; set; }
    }
}