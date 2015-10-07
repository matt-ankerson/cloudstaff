using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Biz_Logic
{
    /// <summary>
    /// Provides utilities necessary for fetching and manipulating username info.
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
                    users = context.Users.Include("UserType").Include("Authentication").OrderBy(x => x.FirstName).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return users;
        }

        /// <summary>
        /// Get all users who are either admins or of general type.
        /// </summary>
        /// <returns>List of general and admin users.</returns>
        public static List<User> GetGeneralAndAdminUsers()
        {
            List<User> users = new List<User>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    users = context.Users.Include("UserType").Include("Authentication").Where(x => x.UserType.Type == Constants.ADMIN_TYPE ||
                        x.UserType.Type == Constants.GENERAL_TYPE).OrderBy(x => x.FirstName).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return users;
        }

        /// <summary>
        /// Gets a single username
        /// </summary>
        /// <param name="userID">Which username do you want?</param>
        /// <returns>The appropriate username</returns>
        public static User GetUser(int userID)
        {
            User user = new User();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    user = context.Users.Include("Authentication").Where(u => u.UserID == userID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return user;
        }

        /// <summary>
        /// Add a new user to the User table
        /// </summary>
        /// <param name="firstName">Users first name</param>
        /// <param name="lastName">Users last name</param>
        /// <param name="userType">General, Visior or Admin</param>
        /// <param name="isDeleted">Indicate whether or not to make this username Active</param>
        /// <param name="password">This user's password.</param>
        /// <param name="username">This user's username.</param>
        public static void AddUser(string firstName, string lastName, int userType, bool isDeleted, string username, string password)
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

                // Now that we have the userID for the new user, we need to add some associative information:

                // Add Authentication for the new user,
                AuthenticationUtilities.AddAuthentication(userID, username, password);

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
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }

        /// <summary>
        /// Add a new user to the user table, but with less verbosity.
        /// </summary>
        /// <remarks>
        /// Don't add an Authentication instance, don't add all the statuses and locations (only defaults).
        /// </remarks>
        /// <param name="firstName">Firstname of visitor</param>
        /// <param name="lastName">Lastname of visitor</param>
        /// <returns>The userID of the newly added visitor, or 0 if there was a problem.</returns>
        public static int AddUserAsVisitor(string firstName, string lastName)
        {
            int newVisitorID = 0;

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the UserTypeID 'visitor'.
                    int visitorTypeID = context.UserTypes.Where(x => x.Type == Constants.VISITOR_TYPE).Select(x => x.UserTypeID).FirstOrDefault();

                    // Create the new user and add it to the db
                    User newVisitor = new User { FirstName = firstName, LastName = lastName, UserTypeID = visitorTypeID, IsActive = true };
                    context.Users.Add(newVisitor);
                    context.SaveChanges();     
                }

                // Use a different context instance
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the userID back out from the added record
                    newVisitorID = context.Users.OrderBy(x => x.UserID).Select(x => x.UserID).ToList().LastOrDefault();
                }

                // Now that we have the userID for our new visitor, we need to add some limited associated information.
                // For a start they need the default status and location
                AddUserStatus(newVisitorID, Constants.DEFAULT_IN_STATUS);
                AddUserLocation(newVisitorID, Constants.DEFAULT_LOCATION);

                // Add a core instance for this user, with defaults for status and location.
                CoreUtilities.AddCore(newVisitorID, Constants.DEFAULT_IN_STATUS, Constants.DEFAULT_LOCATION);

            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return newVisitorID;
        }

        /// <summary>
        /// Make a status available for the given username
        /// </summary>
        /// <param name="userID">The username to add the status to</param>
        /// <param name="statusID">The status we'd like to add.</param>
        public static void AddUserStatus(int userID, int statusID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Ensure that both the username and status actually exist
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
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }

        /// <summary>
        /// Make a given location available for a given username.
        /// </summary>
        /// <param name="userID">The username to add the location to</param>
        /// <param name="locationID">The location to add</param>
        public static void AddUserLocation(int userID, int locationID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Ensure that both the username and location actually exist
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
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }

        /// <summary>
        /// Remove the indicated username from the database
        /// </summary>
        /// <param name="userID">User to delete</param>
        public static void DeleteUser(int userID)
        {
            try
            {
                // Remove the core instance for this username
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
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }

        /// <summary>
        /// Set the IsDeleted flag on the indicated username to True
        /// </summary>
        /// <param name="userID">The username to flag as deleted</param>
        public static void FlagUserDeleted(int userID)
        {
            try
            {
                // Remove the core instance for this username
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
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }

        /// <summary>
        /// Get all available username types
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
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return userTypes;
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="userID">The username to update</param>
        /// <param name="newFirstName">New firstname</param>
        /// <param name="newLastName">New Lastname</param>
        /// <param name="newUserTypeID">New username type</param>
        /// <param name="newIsActive">Indicates whether the username is to be active or not</param>
        public static void UpdateUser(int userID, string newFirstName, string newLastName, int newUserTypeID, bool newIsActive)
        {
            try
            {
                // Update the fields on the user table.

                using (var context = new ABLCloudStaffContext())
                {
                    // Get the appropriate user:
                    User userToModify = context.Users.Where(x => x.UserID == userID).FirstOrDefault();

                    if (userToModify == null)
                        throw new Exception("Bad UserID, username does not exist.");

                    // Update the fields on this username
                    userToModify.FirstName = newFirstName;
                    userToModify.LastName = newLastName;
                    userToModify.UserTypeID = newUserTypeID;
                    userToModify.IsActive = newIsActive;

                    context.SaveChanges();
                }  
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }

        /// <summary>
        /// Assess whether the indicated username is active or not. Return a boolean active.
        /// </summary>
        /// <param name="userID">The username to query for.</param>
        /// <returns>Boolean indicative of activeness.</returns>
        public static bool IsActive(int userID)
        {
            // Assume false
            bool active = false;

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    User u = context.Users.Where(x => x.UserID == userID).FirstOrDefault();

                    if (u != null)
                    {
                        active = u.IsActive;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return active;
        }

    }
    /// <summary>
    /// Object to hold information about a username in verbose detail
    /// </summary>
    public class UserInfo
    {
        public string userID;
        public string firstName;
        public string lastName;
        public string userType;
        public string userTypeID;
        public string isActive;
        public string username;
    }
}