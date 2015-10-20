// Author: Matt Ankerson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Class intended to maintain groups of people that leave the office
    /// </summary>
    public class Group
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        /// <summary>True if the group is currently out.</summary>
        public bool Active { get; set; }
        public int Priority { get; set; }

        public virtual List<UserInGroup> UserInGroups { get; set; }
    }
}