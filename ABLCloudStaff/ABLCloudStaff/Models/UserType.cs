// Author: Matt Ankerson
// Date: 12 July 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Indicates the type of each user
    /// </summary>
    public class UserType
    {
        public int UserTypeID { get; set; }
        public string Type { get; set; }
    }
}