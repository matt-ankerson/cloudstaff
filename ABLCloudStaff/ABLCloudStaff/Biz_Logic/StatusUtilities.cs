﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Biz_Logic
{
    /// <summary>
    /// Provides utilities necessary for fetching and manipulating info from the Status, UserStatus and User tables.
    /// </summary>
    public static class StatusUtilities
    {
        /// <summary>
        /// Get all available statuses for a particular username
        /// </summary>
        /// <param name="userID">The username to search on</param>
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
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return statuses;
        }

        /// <summary>
        /// Get all statuses, regardless of who they belong to.
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
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return statuses;
        }

        /// <summary>
        /// Get all default statuses (that all users should have)
        /// </summary>
        /// <returns>List of statuses</returns>
        public static List<Status> GetDefaultStatuses()
        {
            List<Status> statuses = new List<Status>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get default statuses.
                    List<int> defaultStatuses = context.DefaultStatuses.Select(x => x.StatusID).ToList();

                    statuses = context.Statuses.Where(x => defaultStatuses.Contains(x.StatusID)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return statuses;
        }

        /// <summary>
        /// Get a list of IDs corresponding to all statuses that're currently in use.
        /// </summary>
        /// <returns>Lit of StatusIDs</returns>
        public static List<int> GetCurrentlyUsedStatusIDs()
        {
            List<int> statusIDs = new List<int>();

            try
            {
                using(var context = new ABLCloudStaffContext())
                {
                    statusIDs = context.Cores.Select(x => x.StatusID).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return statusIDs;
        }

        /// <summary>
        /// Add a new status for all users
        /// </summary>
        /// <remarks>
        /// It is important that we add the status for future users. In order to do this, add the new status' ID to the DefaultStatus table.
        /// </remarks>
        /// <param name="statusName">The name of the status in question</param>
        /// <param name="available">Indicates wheter or not this status is considered 'in office'.</param>
        public static void AddStatusForAllUsers(string statusName, bool available)
        {      
            try
            {
                // Add the new Status
                using (var context = new ABLCloudStaffContext())
                {
                    // Add the new status to the Status table.
                    Status s = new Status { Name = statusName, Available = available };
                    context.Statuses.Add(s);
                    context.SaveChanges();
                }

                // Refresh the context
                using (var context = new ABLCloudStaffContext())
                {
                    // Pull out the statusID of the recently added status.
                    int latestStatusID = context.Statuses.OrderBy(x => x.StatusID).Select(x => x.StatusID).ToList().LastOrDefault();

                    // Get a list of all users.
                    List<User> allUsers = UserUtilities.GetAllUsers();

                    // Loop over the users
                    foreach (User u in allUsers)
                    {
                        // Add a UserStatus object for this new statusID for every username.
                        UserStatus us = new UserStatus { UserID = u.UserID, StatusID = latestStatusID, DateAdded = DateTime.Now};
                        context.UserStatuses.Add(us);
                        context.SaveChanges();
                    }

                    // Add the status as a default status in the DefaultStatus table.
                    context.DefaultStatuses.Add(new DefaultStatus { StatusID = latestStatusID });
                    // Save again.
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
        /// Add a new status for a single username
        /// </summary>
        /// <param name="userID">The username to add the status to</param>
        /// <param name="statusName">The name of the status in question.</param>
        /// <param name="available">Indicates whether or not this status is considered 'in office'.</param>
        public static void AddStatusForSingleUser(int userID, string statusName, bool available)
        {
            try
            {
                // Add the new Status
                using (var context = new ABLCloudStaffContext())
                {
                    // Add the new status to the Status table.
                    Status s = new Status { Name = statusName, Available = available };
                    context.Statuses.Add(s);
                    context.SaveChanges();
                }

                // Refresh the context
                using (var context = new ABLCloudStaffContext())
                {
                    // Pull out the statusID of the recently added status.
                    int latestStatusID = context.Statuses.OrderBy(x => x.StatusID).Select(x => x.StatusID).ToList().LastOrDefault();

                    // Add a UserStatus object for this new statusID and the indicated username.
                    UserStatus us = new UserStatus { UserID = userID, StatusID = latestStatusID, DateAdded = DateTime.Now };
                    context.UserStatuses.Add(us);
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
        /// Permanantly remove the indicated status from the db.
        /// </summary>
        /// <param name="statusID">The Status to remove.</param>
        public static void DeleteStatus(int statusID)
        {
            try
            {
                using(var context = new ABLCloudStaffContext())
                {
                    Status statusToDelete = context.Statuses.Where(x => x.StatusID == statusID).FirstOrDefault();
                    context.Statuses.Remove(statusToDelete);
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
        /// Update the indicated status with the given information
        /// </summary>
        /// <param name="statusID">Status to update</param>
        /// <param name="name">The new Status name</param>
        /// <param name="available">Indicates whether or not users should be considered available on this status</param>
        public static void UpdateStatus(int statusID, string name, bool available)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the indicated Status object
                    Status statusToUpdate = context.Statuses.Where(x => x.StatusID == statusID).FirstOrDefault();

                    if (statusToUpdate == null)
                        throw new Exception("Bad StatusID, Status does not exist.");

                    // Update the field on this Status object
                    statusToUpdate.Name = name;
                    statusToUpdate.Available = available;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }
    }

    /// <summary>
    /// Object to hold information about a status 
    /// </summary>
    public class StatusInfo
    {
        public string statusID;
        public string name;
        public string available;
    }
}