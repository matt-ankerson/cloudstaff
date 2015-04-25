// Author: Matthew Ankerson
// Date: 25 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Holds information for a single Location
    /// </summary>
    public class Location
    {
        public int LocationID { get; set; }
        public string Name { get; set; }

        public virtual List<UserLocation> UserLocations { get; set; }
    }
}