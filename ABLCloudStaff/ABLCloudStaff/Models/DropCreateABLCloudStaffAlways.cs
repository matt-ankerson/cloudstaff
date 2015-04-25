// Author: Matthew Ankerson
// Date: 25 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// For Testing. Drop, create and seed the database every time the app is started.
    /// </summary>
    public class DropCreateABLCloudStaffAlways : DropCreateDatabaseAlways<ABLCloudStaffContext>
    {
        protected override void Seed(ABLCloudStaffContext context)
        {
            base.Seed(context);

            // Populate all tables in the appropriate order
        }
    }
}