// Author: Matthew Ankerson
// Date: 25 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Holds information relavent to a generic user of the system
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserTypeID { get; set; }
        public bool IsActive { get; set; }
        public int? AuthenticationID { get; set; }

        public virtual List<UserStatus> UserStatuses { get; set; }
        public virtual List<UserLocation> UserLocations { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual Authentication Authentication { get; set; }
    }
}