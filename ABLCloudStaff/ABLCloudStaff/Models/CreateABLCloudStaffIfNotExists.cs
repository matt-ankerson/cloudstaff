﻿// Author: Matt Ankerson
// Date: 28 July 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Text;
using ABLCloudStaff.Models;
using ABLCloudStaff.Biz_Logic;

namespace ABLCloudStaff.Models
{
    /// <summary>
    /// For deployment, create and seed the database if it doesn't already exist
    /// </summary>
    public class CreateABLCloudStaffIfNotExists : CreateDatabaseIfNotExists<ABLCloudStaffContext>
    {
        // Create the context
        private ABLCloudStaffContext dbContext = new ABLCloudStaffContext();
        // Number of users that will be initialised (important for the authentication table population.)
        private const int N_USERS = 33;

        protected override void Seed(ABLCloudStaffContext context)
        {
            base.Seed(context);

            // Populate tables
            PopulateUserTypes();
            PopulateStatuses();
            PopulateLocations();
            PopulateUsers();
            PopulateAuthentication();
            PopulateUserLocation();
            PopulateUserStatus();
            PopulateCore();
            //PopulateGroup();
            //PopulateUserInGroup();
            PopulateDefaultStatuses();
            PopulateDefaultLocations();
        }

        /// <summary>
        /// Seed the DefaultStatus table
        /// </summary>
        public void PopulateDefaultStatuses()
        {
            List<DefaultStatus> defaultStatuses = new List<DefaultStatus>();

            foreach (int id in Constants.DEFAULT_STATUSES)
            {
                dbContext.DefaultStatuses.Add(new DefaultStatus { StatusID = id });
            }

            dbContext.SaveChanges();
        }

        /// <summary>
        /// Seed the DefaultLocation table
        /// </summary>
        public void PopulateDefaultLocations()
        {
            List<DefaultLocation> defaultLocations = new List<DefaultLocation>();

            foreach (int id in Constants.DEFAULT_LOCATIONS)
            {
                dbContext.DefaultLocations.Add(new DefaultLocation { LocationID = id });
            }

            dbContext.SaveChanges();
        }

        /// <summary>
        /// Seed the group table
        /// </summary>
        public void PopulateGroup()
        {
            List<Group> groups = new List<Group>();

            groups.Add(new Group { Active = false, Name = "Coffee", Priority = 0 });
            groups.Add(new Group { Active = false, Name = "Golf", Priority = 1 });

            foreach (Group g in groups)
                dbContext.Groups.Add(g);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Seed the UserInGroup table
        /// </summary>
        public void PopulateUserInGroup()
        {
            List<UserInGroup> uigs = new List<UserInGroup>();
            uigs.Add(new UserInGroup { GroupID = 1, UserID = 20 });
            uigs.Add(new UserInGroup { GroupID = 1, UserID = 21 });
            uigs.Add(new UserInGroup { GroupID = 1, UserID = 22 });
            uigs.Add(new UserInGroup { GroupID = 2, UserID = 23 });
            uigs.Add(new UserInGroup { GroupID = 2, UserID = 24 });
            uigs.Add(new UserInGroup { GroupID = 2, UserID = 25 });
            foreach (UserInGroup uig in uigs)
                dbContext.UserInGroups.Add(uig);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Seed the Authentication table
        /// </summary>
        public void PopulateAuthentication()
        {
            List<Authentication> auths = new List<Authentication>();

            for (int i = 0; i < N_USERS; i++)
            {
                string password = EncryptionUtilities.HashPassword("P@ssw0rd");
                auths.Add(new Authentication { UserID = i + 1, UserName = "CloudStaff" + (i + 1).ToString(), Password = password });
            }

            foreach (Authentication a in auths)
                dbContext.Authentications.Add(a);
            dbContext.SaveChanges();
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
            // We need core information entered for each username
            List<Core> cores = new List<Core>
            {
                new Core {UserID = 1, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 2, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 3, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 4, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 5, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 6, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 7, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 8, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 9, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 10, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 11, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 12, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 13, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 14, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 15, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 16, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 17, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 18, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 19, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 20, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 21, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 22, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 23, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 24, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 25, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 26, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 27, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 28, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 29, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 30, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 31, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 32, StatusID = 1, LocationID = 1, StateStart = DateTime.Now},
                new Core {UserID = 33, StatusID = 1, LocationID = 1, StateStart = DateTime.Now}
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
                

                new User { FirstName = "Sue", LastName = "Gregory", UserTypeID = 1, IsActive = true, AuthenticationID = 1 },
                new User { FirstName = "Myles", LastName = "McKay", UserTypeID = 1, IsActive = true, AuthenticationID = 2 },
                new User { FirstName = "Fred", LastName = "Ingles", UserTypeID = 1, IsActive = true, AuthenticationID = 3 },
                new User { FirstName = "Daniel", LastName = "Long", UserTypeID = 1, IsActive = true, AuthenticationID = 4 },
                new User { FirstName = "Fiona", LastName = "Shingles", UserTypeID = 1, IsActive = true, AuthenticationID = 5 },
                new User { FirstName = "Greg", LastName = "West", UserTypeID = 1, IsActive = true, AuthenticationID = 6 },
                new User { FirstName = "Bruce", LastName = "Howatt", UserTypeID = 1, IsActive = true, AuthenticationID = 7 },
                new User { FirstName = "Russel", LastName = "Winders", UserTypeID = 1, IsActive = true, AuthenticationID = 8 },
                new User { FirstName = "Hank", LastName = "West", UserTypeID = 1, IsActive = true, AuthenticationID = 9 },
                new User { FirstName = "Cheryl", LastName = "Faye", UserTypeID = 2, IsActive = true, AuthenticationID = 10 },
                new User { FirstName = "Aaron", LastName = "Craig", UserTypeID = 1, IsActive = true, AuthenticationID = 11 },
                new User { FirstName = "Travis", LastName = "Archer", UserTypeID = 1, IsActive = true, AuthenticationID = 12 },
                new User { FirstName = "Josh", LastName = "Knowles", UserTypeID = 1, IsActive = true, AuthenticationID = 13 },
                new User { FirstName = "Mike", LastName = "McGregor", UserTypeID = 1, IsActive = true, AuthenticationID = 14 },
                new User { FirstName = "Ruby", LastName = "Kaye", UserTypeID = 1, IsActive = true, AuthenticationID = 15 },
                new User { FirstName = "Kate", LastName = "Roderique", UserTypeID = 1, IsActive = true, AuthenticationID = 16 },
                new User { FirstName = "Kay", LastName = "Wilson", UserTypeID = 1, IsActive = true, AuthenticationID = 17 },
                new User { FirstName = "Luke", LastName = "Wilson", UserTypeID = 1, IsActive = true, AuthenticationID = 18 },
                new User { FirstName = "Ross", LastName = "Andrews", UserTypeID = 2, IsActive = true, AuthenticationID = 19 },
                new User { FirstName = "Mel", LastName = "Benington", UserTypeID = 1, IsActive = true, AuthenticationID = 20 },
                new User { FirstName = "Dean", LastName = "Huakau", UserTypeID = 1, IsActive = true, AuthenticationID = 21 },
                new User { FirstName = "Matthew", LastName = "Ankerson", UserTypeID = 1, IsActive = true, AuthenticationID = 22 },
                new User { FirstName = "Hayley", LastName = "Hownes", UserTypeID = 1, IsActive = true, AuthenticationID = 23 },
                new User { FirstName = "Nigel", LastName = "Jopp", UserTypeID = 1, IsActive = true, AuthenticationID = 24 },
                new User { FirstName = "Jo", LastName = "Dennis", UserTypeID = 1, IsActive = true, AuthenticationID = 25 },
                new User { FirstName = "Angus", LastName = "Fredrickson", UserTypeID = 1, IsActive = true, AuthenticationID = 26 },
                new User { FirstName = "Peter", LastName = "Anderson", UserTypeID = 1, IsActive = true, AuthenticationID = 27 },
                new User { FirstName = "John", LastName = "Fowler", UserTypeID = 1, IsActive = true, AuthenticationID = 28 },
                new User { FirstName = "Susan", LastName = "Clark", UserTypeID = 1, IsActive = true, AuthenticationID = 29 },
                new User { FirstName = "Simon", LastName = "Townsend", UserTypeID = 1, IsActive = true, AuthenticationID = 30 },
                new User { FirstName = "Pat", LastName = "Stewart", UserTypeID = 1, IsActive = true, AuthenticationID = 31 },
                new User { FirstName = "Tim", LastName = "Forde", UserTypeID = 2, IsActive = true, AuthenticationID = 32 },
                new User { FirstName = "Miriam", LastName = "Thompson", UserTypeID = 2, IsActive = true, AuthenticationID = 33 }
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
                new Status {Name = "In Office", Available = true},
                new Status {Name = "Out of Office", Available = false},
                new Status {Name = "Meeting", Available = false},
                new Status {Name = "Lunch", Available = false},
                new Status {Name = "Working from Home", Available = false},
                new Status {Name = "On Leave", Available = false},
                new Status {Name = "Visiting", Available = true},
                new Status {Name = "Sick", Available = false}
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
                new Location {Name = "Not applicable"},
                new Location {Name = "Dunedin"},
                new Location {Name = "Mosgiel"},
                new Location {Name = "On Farm"},
                new Location {Name = "Nelson"},
                new Location {Name = "Overseas"},
                new Location {Name = "North Island"}
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
            // Add each status to each username

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
                new UserStatus {UserID = 33, StatusID = 6, DateAdded = new DateTime(2015, 3, 5)}
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
                // User 2
                new UserLocation {UserID = 2, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 2, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 2, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 2, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 3
                new UserLocation {UserID = 3, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 3, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 3, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 3, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 4
                new UserLocation {UserID = 4, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 4, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 4, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 4, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 5
                new UserLocation {UserID = 5, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 5, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 5, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 5, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 6
                new UserLocation {UserID = 6, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 6, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 6, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 6, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 7
                new UserLocation {UserID = 7, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 7, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 7, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 7, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 8
                new UserLocation {UserID = 8, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 8, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 8, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 8, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 9
                new UserLocation {UserID = 9, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 9, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 9, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 9, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 10
                new UserLocation {UserID = 10, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 10, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 10, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 10, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 11
                new UserLocation {UserID = 11, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 11, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 11, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 11, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 12
                new UserLocation {UserID = 12, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 12, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 12, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 12, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 13
                new UserLocation {UserID = 13, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 13, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 13, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 13, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 14
                new UserLocation {UserID = 14, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 14, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 14, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 14, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 15
                new UserLocation {UserID = 15, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 15, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 15, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 15, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 16
                new UserLocation {UserID = 16, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 16, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 16, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 16, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 17
                new UserLocation {UserID = 17, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 17, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 17, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 17, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 18
                new UserLocation {UserID = 18, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 18, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 18, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 18, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 19
                new UserLocation {UserID = 19, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 19, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 19, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 19, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 19, LocationID = 5, DateAdded = new DateTime(2015, 3, 5)},
                // User 20
                new UserLocation {UserID = 20, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 20, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 20, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 20, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 21
                new UserLocation {UserID = 21, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 21, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 21, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 21, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 22
                new UserLocation {UserID = 22, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 22, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 22, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 22, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 23
                new UserLocation {UserID = 23, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 23, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 23, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 23, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 24
                new UserLocation {UserID = 24, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 24, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 24, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 24, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 25
                new UserLocation {UserID = 25, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 25, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 25, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 25, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 26
                new UserLocation {UserID = 26, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 26, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 26, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 26, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 27
                new UserLocation {UserID = 27, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 27, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 27, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 27, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 28
                new UserLocation {UserID = 28, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 28, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 28, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 28, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 29
                new UserLocation {UserID = 29, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 29, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 29, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 29, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 30
                new UserLocation {UserID = 30, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 30, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 30, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 30, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 31
                new UserLocation {UserID = 31, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 31, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 31, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 31, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 32
                new UserLocation {UserID = 32, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 32, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 32, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 32, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)},
                // User 33
                new UserLocation {UserID = 33, LocationID = 1, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 33, LocationID = 2, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 33, LocationID = 3, DateAdded = new DateTime(2015, 3, 5)},
                new UserLocation {UserID = 33, LocationID = 4, DateAdded = new DateTime(2015, 3, 5)}
            };

            foreach (UserLocation ul in userLocations)
                dbContext.UserLocations.Add(ul);
            dbContext.SaveChanges();
        }
    }
}