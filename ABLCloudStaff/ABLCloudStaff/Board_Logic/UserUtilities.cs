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
    }
}