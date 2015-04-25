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
        // Create the context
        private ABLCloudStaffContext dbContext = new ABLCloudStaffContext();

        protected override void Seed(ABLCloudStaffContext context)
        {
            base.Seed(context);

            // Populate all tables in the appropriate order
        }

        /// <summary>
        /// Seed the User table
        /// </summary>
        public void PopulateUsers()
        {
            List<User> users = new List<User>
            {
                new User { FirstName = "Peter", LastName = "Brock", Rank = ERank.General },
                new User { FirstName = "Colin", LastName = "Bond", Rank = ERank.General },
                new User { FirstName = "Mark", LastName = "Winterbottom", Rank = ERank.General },
                new User { FirstName = "Allan", LastName = "Moffat", Rank = ERank.General },
                new User { FirstName = "Dick", LastName = "Johnson", Rank = ERank.General },
                new User { FirstName = "Jim", LastName = "Richards", Rank = ERank.Admin },
                new User { FirstName = "Glenn", LastName = "Seton", Rank = ERank.Admin },
                new User { FirstName = "Craig", LastName = "Lowndes", Rank = ERank.General }
            };

            foreach (User u in users)
                dbContext.Users.Add(u);
            dbContext.SaveChanges();
        }
    }
}