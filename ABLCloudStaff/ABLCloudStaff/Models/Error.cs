// Author: Matt Ankerson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// Error class to capture erronous activity within the app.
    /// </summary>
    public class Error
    {
        public int ErrorID { get; set; }
        public string Exception { get; set; }
        public string Detail { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}