// Author: Matt Ankerson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Indicates a user is a member of a group.
    /// </summary>
    public class UserInGroup
    {
        public int UserInGroupID { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }

        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
    }
}