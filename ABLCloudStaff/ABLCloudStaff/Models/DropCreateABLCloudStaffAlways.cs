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

            // Populate tables
            PopulateStatuses();
            PopulateLocations();
            PopulateUsers();
            PopulateUserLocation();
            PopulateUserStatus();
            PopulateCore();
        }

        /// <summary>
        /// Seed the core table
        /// </summary>
        public void PopulateCore()
        {
            // We need core information entered for each user
            List<Core> cores = new List<Core>
            {
                new Core {UserID = 1, StatusID = 1, LocationID = 1, StateStart = DateTime.Now, StateEnd = null},
                new Core {UserID = 2, StatusID = 2, LocationID = 5, StateStart = DateTime.Now, StateEnd = null},
                new Core {UserID = 3, StatusID = 3, LocationID = 3, StateStart = DateTime.Now, StateEnd = null},
                new Core {UserID = 4, StatusID = 5, LocationID = 1, StateStart = DateTime.Now, StateEnd = null},
                new Core {UserID = 5, StatusID = 6, LocationID = 1, StateStart = DateTime.Now, StateEnd = null},
                new Core {UserID = 6, StatusID = 3, LocationID = 1, StateStart = DateTime.Now, StateEnd = null},
                new Core {UserID = 7, StatusID = 3, LocationID = 2, StateStart = DateTime.Now, StateEnd = null},
                new Core {UserID = 8, StatusID = 1, LocationID = 1, StateStart = DateTime.Now, StateEnd = null}
            };

            foreach (Core c in cores)
                dbContext.CoreTable.Add(c);
            dbContext.SaveChanges();
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

        /// <summary>
        /// Seed the Status table
        /// </summary>
        public void PopulateStatuses()
        {
            List<Status> statuses = new List<Status>
            {
                new Status {Name = "In Office", Worksite = true},
                new Status {Name = "On Farm", Worksite = false},
                new Status {Name = "Meeting", Worksite = false},
                new Status {Name = "Lunch", Worksite = false},
                new Status {Name = "Home", Worksite = false},
                new Status {Name = "On Leave", Worksite = false}
            };

            foreach (Status s in statuses)
                dbContext.Statuses.Add(s);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Seed the Location table
        /// </summary>
        public void PopulateLocations()
        {
            List<Location> locations = new List<Location>
            {
                new Location {Name = "AbacusBio Dunedin"},
                new Location {Name = "AbacusBio Sydney"},
                new Location {Name = "Alliance Grp Lorneville"},
                new Location {Name = "Fonterra Edendale"},
                new Location {Name = "Tuapeka West"}
            };

            foreach (Location l in locations)
                dbContext.Locations.Add(l);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Seed the User-Status table
        /// </summary>
        public void PopulateUserStatus()
        {
            // Add each status to each user

            List<UserStatus> userStatuses = new List<UserStatus>
            {
                // User 1
                new UserStatus {UserID = 1, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 1, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 1, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 1, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 1, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 1, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 2
                new UserStatus {UserID = 2, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 2, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 2, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 2, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 2, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 2, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 2
                new UserStatus {UserID = 3, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 3, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 3, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 3, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 3, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 3, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 4
                new UserStatus {UserID = 4, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 4, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 4, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 4, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 4, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 4, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 5
                new UserStatus {UserID = 5, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 5, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 5, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 5, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 5, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 5, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 6
                new UserStatus {UserID = 6, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 6, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 6, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 6, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 6, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 6, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 7
                new UserStatus {UserID = 7, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 7, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 7, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 7, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 7, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 7, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 8
                new UserStatus {UserID = 8, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 8, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 8, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 8, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 8, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 8, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)}
            };

            foreach (UserStatus us in userStatuses)
                dbContext.UserStatuses.Add(us);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Seed the User-Location table
        /// </summary>
        public void PopulateUserLocation()
        {
            List<UserLocation> userLocations = new List<UserLocation>
            {
                // User 1
                new UserLocation {UserID = 1, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 1, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 1, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 1, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 1, LocationID = 5, DateAdded = new DateTime(2015, 3, 5)},
                // User 2
                new UserLocation {UserID = 2, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 2, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 2, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 2, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 2, LocationID = 5, DateAdded = new DateTime(2015, 3, 5)},
                // User 3
                new UserLocation {UserID = 3, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 3, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 3, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 3, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 3, LocationID = 5, DateAdded = new DateTime(2015, 3, 5)},
                // User 4
                new UserLocation {UserID = 4, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 4, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 4, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 4, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 4, LocationID = 5, DateAdded = new DateTime(2015, 3, 5)},
                // User 5
                new UserLocation {UserID = 5, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 5, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 5, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 5, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 5, LocationID = 5, DateAdded = new DateTime(2015, 3, 5)},
                // User 6
                new UserLocation {UserID = 6, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 6, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 6, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 6, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 6, LocationID = 5, DateAdded = new DateTime(2015, 3, 5)},
                // User 7
                new UserLocation {UserID = 7, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 7, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 7, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 7, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 7, LocationID = 5, DateAdded = new DateTime(2015, 3, 5)},
                // User 8
                new UserLocation {UserID = 8, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 8, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 8, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 8, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 8, LocationID = 5, DateAdded = new DateTime(2015, 3, 5)}
            };

            foreach (UserLocation ul in userLocations)
                dbContext.UserLocations.Add(ul);
            dbContext.SaveChanges();
        }
    }
}