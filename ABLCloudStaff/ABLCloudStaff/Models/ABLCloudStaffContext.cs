// Author: Matthew Ankerson
// Date: 25 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// This is the class we use to interact with the database
    /// </summary>
    public class ABLCloudStaffContext : DbContext
    {
        public DbSet<Core> CoreTable { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
    }
}