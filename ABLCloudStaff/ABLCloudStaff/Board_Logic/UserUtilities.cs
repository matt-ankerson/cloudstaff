using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Board_Logic
{
    /// <summary>
    /// Provides utilities necessary for fetching and manipulating user info.
    /// </summary>
    public static class UserUtilities
    {
        /// <summary>
        /// Gets all Users
        /// </summary>
        /// <returns>All Users currently storeed in db</returns>
        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    users = context.Users.Include("UserType").OrderBy(x => x.LastName).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return users;
        }

        /// <summary>
        /// Gets a single user
        /// </summary>
        /// <param name="userID">Which user do you want?</param>
        /// <returns>The appropriate user</returns>
        public static User GetUser(int userID)
        {
            User user = new User();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    user = context.Users.Where(u => u.UserID == userID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return user;
        }

        /// <summary>
        /// Add a new user to the User table
        /// </summary>
        /// <param name="firstName">Users first name</param>
        /// <param name="lastName">Users last name</param>
        /// <param name="userType">General, Visior or Admin</param>
        /// <param name="isDeleted">Indicate whether or not to make this user Active</param>
        public static void AddUser(string firstName, string lastName, int userType, bool isDeleted)
        {
            try
            {
                int userID = 0;

                using (var context = new ABLCloudStaffContext())
                {
                    // Create the new user and add it to the db
                    User newUser = new User {FirstName = firstName, LastName = lastName, UserTypeID = userType, IsActive = isDeleted };
                    context.Users.Add(newUser);
                    context.SaveChanges();      
                }

                // Use a different context instance
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the userID back out from the added record
                    userID = context.Users.OrderBy(x => x.UserID).Select(x => x.UserID).ToList().LastOrDefault();
                }

                // Add default statuses and locations for the new user.
                // Statuses
                foreach (var statusID in Constants.DEFAULT_STATUSES)
                {
                    AddUserStatus(userID, statusID);
                }
                // Locations
                foreach (var locationID in Constants.DEFAULT_LOCATIONS)
                {
                    AddUserLocation(userID, locationID);
                }

                // Add a core instance for this user, with defaults for status and location.
                CoreUtilities.AddCore(userID, Constants.DEFAULT_IN_STATUS, Constants.DEFAULT_LOCATION);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Make a status available for the given user
        /// </summary>
        /// <param name="userID">The user to add the status to</param>
        /// <param name="statusID">The status we'd like to add.</param>
        public static void AddUserStatus(int userID, int statusID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Ensure that both the user and status actually exist
                    List<int> allUsers = context.Users.Select(x => x.UserID).ToList();
                    List<int> allStatuses = context.Statuses.Select(x => x.StatusID).ToList();

                    if (!allUsers.Contains(userID))
                        throw new Exception("User does not exist.");
                    if (!allStatuses.Contains(statusID))
                        throw new Exception("Status does not exist.");

                    // Create the UserStatus instance and add it to the database
                    UserStatus newUserStatus = new UserStatus { UserID = userID, StatusID = statusID, DateAdded = DateTime.Now };
                    context.UserStatuses.Add(newUserStatus);
                    context.SaveChanges();
                }
                 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Make a given location available for a given user.
        /// </summary>
        /// <param name="userID">The user to add the location to</param>
        /// <param name="locationID">The location to add</param>
        public static void AddUserLocation(int userID, int locationID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Ensure that both the user and location actually exist
                    List<int> allUsers = context.Users.Select(x => x.UserID).ToList();
                    List<int> allLocations = context.Locations.Select(x => x.LocationID).ToList();

                    if (!allUsers.Contains(userID))
                        throw new Exception("User does not exist.");
                    if (!allLocations.Contains(locationID))
                        throw new Exception("Location does not exist.");

                    // Create the UserLocation instance and add it to the database
                    UserLocation newUserLocation = new UserLocation { UserID = userID, LocationID = locationID, DateAdded = DateTime.Now };
                    context.UserLocations.Add(newUserLocation);
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Remove the indicated user from the database
        /// </summary>
        /// <param name="userID">User to delete</param>
        public static void DeleteUser(int userID)
        {
            try
            {
                // Remove the core instance for this user
                CoreUtilities.DeleteCore(userID);

                using (var context = new ABLCloudStaffContext())
                {
                    var userToDelete = context.Users.SingleOrDefault(x => x.UserID == userID);

                    if (userToDelete != null)
                    {
                        context.Users.Remove(userToDelete);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Set the IsDeleted flag on the indicated user to True
        /// </summary>
        /// <param name="userID">The user to flag as deleted</param>
        public static void FlagUserDeleted(int userID)
        {
            try
            {
                // Remove the core instance for this user
                CoreUtilities.DeleteCore(userID);

                using (var context = new ABLCloudStaffContext())
                {
                    var userToDelete = context.Users.SingleOrDefault(x => x.UserID == userID);

                    if (userToDelete != null)
                    {
                        userToDelete.IsActive = false;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get all available user types
        /// </summary>
        /// <returns>A lis of type UserType</returns>
        public static List<UserType> GetAllUserTypes()
        {
            List<UserType> userTypes = new List<UserType>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    userTypes = context.UserTypes.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return userTypes;
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="userID">The user to update</param>
        /// <param name="newFirstName">New firstname</param>
        /// <param name="newLastName">New Lastname</param>
        /// <param name="newUserTypeID">New user type</param>
        /// <param name="newIsActive">Indicates whether the user is to be active or not</param>
        public static void UpdateUser(int userID, string newFirstName, string newLastName, int newUserTypeID, bool newIsActive)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the appropriate user:
                    User userToModify = context.Users.Where(x => x.UserID == userID).FirstOrDefault();

                    if (userToModify == null)
                        throw new Exception("Bad UserID, user does not exist.");

                    // Update the fields on this user
                    userToModify.FirstName = newFirstName;
                    userToModify.LastName = newLastName;
                    userToModify.UserTypeID = newUserTypeID;
                    userToModify.IsActive = newIsActive;

                    context.SaveChanges();
                }        
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    /// <summary>
    /// Object to hold information about a user in verbose detail
    /// </summary>
    public class UserInfo
    {
        public string userID;
        public string firstName;
        public string lastName;
        public string userType;
        public string userTypeID;
        public string isActive;
    }
}