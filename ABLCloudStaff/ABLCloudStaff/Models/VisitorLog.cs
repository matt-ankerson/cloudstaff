using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Store information about a visitor's visit to the workplace here.
    /// </summary>
    /// <remarks>
    /// Query this table for auto population of fields in the 'add visitor' ui.
    /// </remarks>
    public class VisitorLog
    {
        public int VisitorLogID { get; set; }
        /// <summary>The user who is visiting the organisation.</summary>
        public int VisitorUserID { get; set; }
        /// <summary>The user who is being visited by the visitor</summary>
        public int VisitedUserID { get; set; }
        public string Company { get; set; }
        public DateTime ArrivedTime { get; set; }
        public DateTime IntendedDepartTime { get; set; }
        public DateTime? DepartedTime { get; set; }
    }
}