// Author: Matt Ankerson
// Date: 9 November 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Holds status IDs inidcating the system's default statuses for adding new users
    /// </summary>
    public class DefaultStatus
    {
        public int DefaultStatusID { get; set; }
        public int StatusID { get; set; }

        public virtual Status Status { get; set; }
    }
}