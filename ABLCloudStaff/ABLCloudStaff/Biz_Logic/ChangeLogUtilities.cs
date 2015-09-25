using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;
using System.Data.Entity;

namespace ABLCloudStaff.Biz_Logic
{
    /// <summary>
    /// Provides utilities necessary for logging a change of state or location by any username of the system.
    /// </summary>
    public static class ChangeLogUtilities
    {
        /// <summary>
        /// Get the latest status updates
        /// </summary>
        /// <param name="nRecords">Number of records to fetch</param>
        /// <returns>List of type StatusChangeLog</returns>
        public static List<StatusChangeLog> GetStatusChanges(int nRecords)
        {
            List<StatusChangeLog> changeLog = new List<StatusChangeLog>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Query for 'n' records, starting with the latest
                    changeLog = context.StatusChangeLogs.Include("User").Include("NewStatus").Include("OldStatus").OrderByDescending(x => x.StatusChangeTimeStamp).Take(nRecords).ToList();
                }
                
                
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now, ex.InnerException.Message);
                throw ex;
            }

            return changeLog;
        }

        /// <summary>
        /// Get the latest location updates
        /// </summary>
        /// <param name="nRecords">The number of records to fetch</param>
        /// <returns>List of type LocationChangeLog</returns>
        public static List<LocationChangeLog> GetLocationChanges(int nRecords)
        {
            List<LocationChangeLog> changeLog = new List<LocationChangeLog>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Query for 'n' record, starting with the latest
                    changeLog = context.LocationChangeLogs.Include("User").Include("NewLocation").Include("OldLocation").OrderByDescending(x => x.LocationChangeTimeStamp).Take(nRecords).ToList();
                }


            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now, ex.InnerException.Message);
                throw ex;
            }

            return changeLog;
        }

        /// <summary>
        /// Log the change from one status to another
        /// </summary>
        /// <param name="userID">The username to log the change for</param>
        /// <param name="newStatusID">The new status</param>
        /// <param name="oldStatusID">the old status</param>
        /// <param name="stateInitTimeStamp">The time that the previous state was initiated</param>
        public static void LogStatusChange(int userID, int newStatusID, int oldStatusID, DateTime stateInitTimeStamp)
        {
            StatusChangeLog changeLog = new StatusChangeLog();

            try
            {
                changeLog.UserID = userID;
                changeLog.NewStatusID = newStatusID;
                changeLog.OldStatusID = oldStatusID;
                changeLog.StatusChangeTimeStamp = DateTime.Now;
                changeLog.StatusInitTimeStamp = stateInitTimeStamp;

                // Save the new ChangeLog object to the db
                using (var context = new ABLCloudStaffContext())
                {
                    // Explicitly assign statuses
                    Status newStatus = context.Statuses.Where(x => x.StatusID == newStatusID).FirstOrDefault();
                    Status oldStatus = context.Statuses.Where(x => x.StatusID == oldStatusID).FirstOrDefault();
                    changeLog.NewStatus = newStatus;
                    changeLog.OldStatus = oldStatus;


                    context.StatusChangeLogs.Add(changeLog);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                tErrorUtilities.LogException(ex.Message, DateTime.Now, ex.InnerException.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Log the change from one Location to another
        /// </summary>
        /// <param name="userID">The username to log the change for</param>
        /// <param name="newLocID">The new location</param>
        /// <param name="oldLocID">The old location</param>
        /// <param name="stateInitTimeStamp">The time the previous state was initiated.</param>
        public static void LogLocationChange(int userID, int newLocID, int oldLocID, DateTime stateInitTimeStamp)
        {
            LocationChangeLog changeLog = new LocationChangeLog();

            try
            {
                changeLog.UserID = userID;
                changeLog.NewLocationID = newLocID;
                changeLog.OldLocationID = oldLocID;
                changeLog.LocationChangeTimeStamp = DateTime.Now;
                changeLog.LocationInitTimeStamp = stateInitTimeStamp;

                // Save the new ChangeLog object to the db
                using (var context = new ABLCloudStaffContext())
                {
                    // Explicitly assign locations
                    Location oldLocation = context.Locations.Where(x => x.LocationID == oldLocID).FirstOrDefault();
                    Location newLocation = context.Locations.Where(x => x.LocationID == newLocID).FirstOrDefault();
                    changeLog.OldLocation = oldLocation;
                    changeLog.NewLocation = newLocation;
                    context.LocationChangeLogs.Add(changeLog);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now, ex.InnerException.Message);
                throw ex;
            }
        }
    }

    /// <summary>
    /// Object to hold change-log information (status or location)
    /// </summary>
    public class ChangeLogInfo
    {
        public string firstName;
        public string lastName;
        public string oldState;
        public string newState;
        public string stateChangeTimestamp;
        public string prevStateInitTimestamp;
    }
}