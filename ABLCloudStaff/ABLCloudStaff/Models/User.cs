// Author: Matthew Ankerson
// Date: 25 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Holds information relavent to a generic user of the system
    /// </summary>
    /// <remarks>
    /// User has a 1 - 1 relationship with the Authentication table
    /// </remarks>
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserTypeID { get; set; }
        public bool IsActive { get; set; }
        public int? AuthenticationID { get; set; }  // The inclusion of Authentication is optional because
                                                    // we would like the user model to double for visitors.

        public virtual List<UserStatus> UserStatuses { get; set; }
        public virtual List<UserLocation> UserLocations { get; set; }
        public virtual UserType UserType { get; set; }

        [ForeignKey("AuthenticationID")]
        public virtual Authentication Authentication { get; set; }
    }
}