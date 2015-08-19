// Author: Matt Ankerson
// Date: 17 August 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Model class to hold authentication details for a single user
    /// </summary>
    public class Authentication
    {
        public int AuthenticationID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}