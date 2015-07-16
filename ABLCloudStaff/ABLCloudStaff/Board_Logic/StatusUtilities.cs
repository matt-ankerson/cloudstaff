using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Board_Logic
{
    /// <summary>
    /// Provides utilities necessary for fetching and manipulating info from the Status, UserStatus and User tables.
    /// </summary>
    public static class StatusUtilities
    {
        /// <summary>
        /// Get all available statuses for a particular user
        /// </summary>
        /// <param name="userID">The user to search on</param>
        /// <returns>A List of statuses</returns>
        public static List<Status> GetAvailableStatuses(int userID)
        {
            List<Status> statuses = new List<Status>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    List<UserStatus> userStatuses = new List<UserStatus>();

                    // Get appropriate UserStatus objects
                    userStatuses = context.UserStatuses.Include("Status").Where(x => x.UserID == userID).ToList();

                    // Build the list of statuses
                    foreach (UserStatus us in userStatuses)
                        statuses.Add(us.Status);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return statuses;
        }

        /// <summary>
        /// Get all statuses, regarlesss of who they belong to.
        /// </summary>
        /// <returns>List of type Status</returns>
        public static List<Status> GetAllStatuses()
        {
            List<Status> statuses = new List<Status>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get status objects
                    statuses = context.Statuses.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return statuses;
        }
    }
}