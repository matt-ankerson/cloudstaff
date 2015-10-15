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
    /// <remarks>
    /// It was decided that this class would be quite minimal. For the collection of historical data, we're relying on the records
    /// created by changing the state of each individual user.
    /// </remarks>
    public class Group
    {
        public int GroupID { get; set; }
        public string Members { get; set; }         // Serialised collection of (comma delimited) integers representing members by UserID.
        public DateTime Initiated { get; set; }     // Time that this group was initiated.
        public bool Active { get; set; }            // Whether or not this group has returned or not.
    }
}