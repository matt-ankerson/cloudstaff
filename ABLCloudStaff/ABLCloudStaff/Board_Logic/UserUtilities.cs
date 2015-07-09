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
                    users = context.Users.ToList();
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
        /// <param name="rank">Enum. General, Visior or Admin</param>
        public static void AddUser(string firstName, string lastName)
        {
            try
            {
                int userID = 0;

                using (var context = new ABLCloudStaffContext())
                {
                    // Create the new user and add it to the db
                    User newUser = new User {FirstName = firstName, LastName = lastName };
                    context.Users.Add(newUser);
                    context.SaveChanges();

                    // Get the userID back out from the added record
                    userID = context.Users.Select(x => x.UserID).ToList().LastOrDefault();
                }

                // Add a core instance for this user, with defaults for status and location.
                CoreUtilities.AddCore(userID, Constants.DEFAULT_STATUS, Constants.DEFAULT_LOCATION);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}