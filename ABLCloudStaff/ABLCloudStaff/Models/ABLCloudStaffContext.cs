// Author: Matthew Ankerson
// Date: 25 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// This is the class we use to interact with the database
    /// </summary>
    public class ABLCloudStaffContext : DbContext
    {
        public ABLCloudStaffContext() : base(ConnectionString)
        {
        }

        private static string ConnectionString
        {
            get
            {
                var builder = new SqlConnectionStringBuilder
                {
#if DEBUG
                    DataSource=@".\SQLEXPRESS",
                    InitialCatalog="ABLCloudStaff",
                    IntegratedSecurity=true,
                    MultipleActiveResultSets = true // For Entity Framework
#else
                    // OP
                    //DataSource = @".\SQLEXPRESS",
                    //InitialCatalog = "ABLCloudStaff",
                    //UserID = "test",
                    //Password = "P@ssw0rd",
                    //MultipleActiveResultSets = true // For Entity Framework

                    // AbacusBio
                    DataSource = @"AB-DEPLOY",
                    InitialCatalog = "ABLCloudStaff",
                    UserID = "CloudStaffTest",
                    Password = "P@ssw0rd",
                    MultipleActiveResultSets = true // For Entity Framework
#endif
                };

                return builder.ConnectionString;
            }
        }

        public DbSet<Core> Cores { get; set; }
        public DbSet<StatusChangeLog> StatusChangeLogs { get; set; }
        public DbSet<LocationChangeLog> LocationChangeLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Authentication> Authentications { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<VisitorLog> VisitorLogs { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserInGroup> UserInGroups { get; set; }
    }
}