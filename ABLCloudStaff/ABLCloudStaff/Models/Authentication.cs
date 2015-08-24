// Author: Matt Ankerson
// Date: 17 August 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Model class to hold authentication details for a single user
    /// </summary>
    /// <remarks>
    /// Authentication has a 1 - 1 relationship with the User table
    /// </remarks>
    public class Authentication
    {
        [Key, ForeignKey("User")]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public byte[] Token { get; set; }      // api key
        
        public virtual User User { get; set; }
    }
}