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
            PopulateUserTypes();
            PopulateStatuses();
            PopulateLocations();
            PopulateUsers();
            PopulateUserLocation();
            PopulateUserStatus();
            PopulateCore();
        }

        /// <summary>
        /// Seed the UserType table
        /// </summary>
        public void PopulateUserTypes()
        {
            List<UserType> userTypes = new List<UserType>
            {
                new UserType {Type = "General"},
                new UserType {Type = "Admin"},
                new UserType {Type = "Visitor"}
            };

            foreach (UserType ut in userTypes)
                dbContext.UserTypes.Add(ut);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Seed the core table
        /// </summary>
        public void PopulateCore()
        {
            // We need core information entered for each user
            List<Core> cores = new List<Core>
            {
                new Core {UserID = 1, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 2, StatusID = 2, LocationID = 2, StateStart = DateTime.Now},
                new Core {UserID = 3, StatusID = 3, LocationID = 3, StateStart = DateTime.Now},
                new Core {UserID = 4, StatusID = 5, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 5, StatusID = 6, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 6, StatusID = 3, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 7, StatusID = 3, LocationID = 2, StateStart = DateTime.Now},
                new Core {UserID = 8, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 9, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 10, StatusID = 2, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 11, StatusID = 3, LocationID = 3, StateStart = DateTime.Now},
                new Core {UserID = 12, StatusID = 5, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 13, StatusID = 6, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 14, StatusID = 3, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 15, StatusID = 3, LocationID = 2, StateStart = DateTime.Now},
                new Core {UserID = 16, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 17, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 18, StatusID = 2, LocationID = 2, StateStart = DateTime.Now},
                new Core {UserID = 19, StatusID = 3, LocationID = 3, StateStart = DateTime.Now},
                new Core {UserID = 20, StatusID = 5, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 21, StatusID = 6, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 22, StatusID = 3, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 23, StatusID = 3, LocationID = 2, StateStart = DateTime.Now},
                new Core {UserID = 24, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 25, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 26, StatusID = 2, LocationID = 2, StateStart = DateTime.Now},
                new Core {UserID = 27, StatusID = 3, LocationID = 3, StateStart = DateTime.Now},
                new Core {UserID = 28, StatusID = 5, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 29, StatusID = 6, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 30, StatusID = 3, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 31, StatusID = 3, LocationID = 2, StateStart = DateTime.Now},
                new Core {UserID = 32, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 33, StatusID = 3, LocationID = 2, StateStart = DateTime.Now},
                new Core {UserID = 34, StatusID = 1, LocationID = 1, StateStart = DateTime.Now}
            };

            foreach (Core c in cores)
                dbContext.Cores.Add(c);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Seed the User table
        /// </summary>
        public void PopulateUsers()
        {
            List<User> users = new List<User>
            {
                new User { FirstName = "Anna", LastName = "Campbell", UserTypeID = 1 },
                new User { FirstName = "Bram", LastName = "Visser", UserTypeID = 1 },
                new User { FirstName = "Bruno", LastName = "Santos", UserTypeID = 1 },
                new User { FirstName = "Daniel", LastName = "Martin-Collado", UserTypeID = 1 },
                new User { FirstName = "Fiona", LastName = "Hely", UserTypeID = 1 },
                new User { FirstName = "Gemma", LastName = "Jenkins", UserTypeID = 1 },
                new User { FirstName = "Bruce", LastName = "McCorkindale", UserTypeID = 1 },
                new User { FirstName = "Gertje", LastName = "Petersen", UserTypeID = 1 },
                new User { FirstName = "Grace", LastName = "Johnstone", UserTypeID = 1 },
                new User { FirstName = "Cheryl", LastName = "King", UserTypeID = 2 },
                new User { FirstName = "Hadyn", LastName = "Craig", UserTypeID = 1 },
                new User { FirstName = "Jason", LastName = "Archer", UserTypeID = 1 },
                new User { FirstName = "Joanne", LastName = "Kerslake", UserTypeID = 1 },
                new User { FirstName = "Jonathan", LastName = "Chuah", UserTypeID = 1 },
                new User { FirstName = "Jude", LastName = "Sise", UserTypeID = 1 },
                new User { FirstName = "Katarzyna", LastName = "Stachowicz", UserTypeID = 1 },
                new User { FirstName = "Kevin", LastName = "Wilson", UserTypeID = 1 },
                new User { FirstName = "Luke", LastName = "Proctor", UserTypeID = 1 },
                new User { FirstName = "Mark", LastName = "Teviotdale", UserTypeID = 2 },
                new User { FirstName = "Melanie", LastName = "Joubert", UserTypeID = 1 },
                new User { FirstName = "Nadia", LastName = "McLean", UserTypeID = 1 },
                new User { FirstName = "Nana", LastName = "Bortsie-Aryee", UserTypeID = 1 },
                new User { FirstName = "Natalie", LastName = "Howes", UserTypeID = 1 },
                new User { FirstName = "Neville", LastName = "Jopson", UserTypeID = 1 },
                new User { FirstName = "Nicola", LastName = "Dennis", UserTypeID = 1 },
                new User { FirstName = "Peter", LastName = "Amer", UserTypeID = 1 },
                new User { FirstName = "Peter", LastName = "Fennessy", UserTypeID = 1 },
                new User { FirstName = "Peter", LastName = "O'Neill", UserTypeID = 1 },
                new User { FirstName = "Peter", LastName = "Wong", UserTypeID = 1 },
                new User { FirstName = "Sammy", LastName = "Wong", UserTypeID = 1 },
                new User { FirstName = "Simon", LastName = "Glennie", UserTypeID = 1 },
                new User { FirstName = "Simon", LastName = "Ryan", UserTypeID = 2 },
                new User { FirstName = "Robyn", LastName = "Simpson", UserTypeID = 2 },
                new User { FirstName = "Tim", LastName = "Byne", UserTypeID = 1 }
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
                new Location {Name = "Otago Polytech"},
                new Location {Name = "Nelson"}
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
                new UserStatus {UserID = 8, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 9
                new UserStatus {UserID = 9, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 9, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 9, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 9, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 9, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 9, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 10
                new UserStatus {UserID = 10, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 10, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 10, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 10, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 10, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 10, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 11
                new UserStatus {UserID = 11, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 11, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 11, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 11, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 11, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 11, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 12
                new UserStatus {UserID = 12, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 12, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 12, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 12, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 12, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 12, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 13
                new UserStatus {UserID = 13, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 13, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 13, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 13, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 13, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 13, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 14
                new UserStatus {UserID = 14, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 14, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 14, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 14, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 14, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 14, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 15
                new UserStatus {UserID = 15, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 15, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 15, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 15, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 15, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 15, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 16
                new UserStatus {UserID = 16, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 16, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 16, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 16, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 16, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 16, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 17
                new UserStatus {UserID = 17, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 17, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 17, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 17, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 17, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 17, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 18
                new UserStatus {UserID = 18, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 18, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 18, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 18, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 18, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 18, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 19
                new UserStatus {UserID = 19, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 19, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 19, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 19, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 19, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 19, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 20
                new UserStatus {UserID = 20, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 20, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 20, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 20, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 20, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 20, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 21
                new UserStatus {UserID = 21, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 21, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 21, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 21, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 21, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 21, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 22
                new UserStatus {UserID = 22, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 22, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 22, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 22, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 22, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 22, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 23
                new UserStatus {UserID = 23, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 23, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 23, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 23, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 23, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 23, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 24
                new UserStatus {UserID = 24, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 24, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 24, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 24, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 24, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 24, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 25
                new UserStatus {UserID = 25, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 25, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 25, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 25, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 25, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 25, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 26
                new UserStatus {UserID = 26, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 26, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 26, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 26, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 26, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 26, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 27
                new UserStatus {UserID = 27, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 27, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 27, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 27, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 27, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 27, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 28
                new UserStatus {UserID = 28, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 28, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 28, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 28, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 28, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 28, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 29
                new UserStatus {UserID = 29, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 29, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 29, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 29, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 29, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 29, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 30
                new UserStatus {UserID = 30, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 30, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 30, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 30, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 30, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 30, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 31
                new UserStatus {UserID = 31, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 31, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 31, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 31, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 31, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 31, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 32
                new UserStatus {UserID = 32, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 32, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 32, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 32, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 32, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 32, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 33
                new UserStatus {UserID = 33, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 33, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 33, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 33, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 33, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 33, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)},
                // User 34
                new UserStatus {UserID = 34, StatusID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 34, StatusID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 34, StatusID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 34, StatusID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 34, StatusID = 5, DateAdded = new DateTime(2015, 3, 5)},
                new UserStatus {UserID = 34, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)}
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
                // User 2
                new UserLocation {UserID = 2, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 2, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 2, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 3
                new UserLocation {UserID = 3, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 3, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 3, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 4
                new UserLocation {UserID = 4, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 4, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 4, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 5
                new UserLocation {UserID = 5, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 5, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 5, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 6
                new UserLocation {UserID = 6, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 6, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 6, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 7
                new UserLocation {UserID = 7, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 7, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 7, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 8
                new UserLocation {UserID = 8, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 8, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 8, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 9
                new UserLocation {UserID = 9, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 9, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 9, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 10
                new UserLocation {UserID = 10, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 10, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 10, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 11
                new UserLocation {UserID = 11, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 11, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 11, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 12
                new UserLocation {UserID = 12, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 12, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 12, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 13
                new UserLocation {UserID = 13, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 13, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 13, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 14
                new UserLocation {UserID = 14, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 14, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 14, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 15
                new UserLocation {UserID = 15, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 15, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 15, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 16
                new UserLocation {UserID = 16, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 16, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 16, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 17
                new UserLocation {UserID = 17, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 17, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 17, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 18
                new UserLocation {UserID = 18, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 18, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 18, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 19
                new UserLocation {UserID = 19, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 19, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 19, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 19, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 20
                new UserLocation {UserID = 20, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 20, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 20, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 21
                new UserLocation {UserID = 21, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 21, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 21, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 22
                new UserLocation {UserID = 22, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 22, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 22, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 23
                new UserLocation {UserID = 23, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 23, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 23, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 24
                new UserLocation {UserID = 24, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 24, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 24, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 25
                new UserLocation {UserID = 25, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 25, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 25, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 26
                new UserLocation {UserID = 26, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 26, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 26, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 27
                new UserLocation {UserID = 27, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 27, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 27, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 28
                new UserLocation {UserID = 28, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 28, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 28, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 29
                new UserLocation {UserID = 29, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 29, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 29, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 30
                new UserLocation {UserID = 30, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 30, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 30, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 31
                new UserLocation {UserID = 31, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 31, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 31, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 32
                new UserLocation {UserID = 32, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 32, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 32, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 33
                new UserLocation {UserID = 33, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 33, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 33, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                // User 34
                new UserLocation {UserID = 34, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 34, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 34, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)}
            };

            foreach (UserLocation ul in userLocations)
                dbContext.UserLocations.Add(ul);
            dbContext.SaveChanges();
        }
    }
}