using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Board_Logic
{
    /// <summary>
    /// Provides utilities necessary for logging a change of state or location by any user of the system.
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
                    // Query for 'n' record, starting with the latest
                    changeLog = context.StatusChangeLogs.OrderByDescending(x => x.StatusChangeTimeStamp).Take(nRecords).ToList();
                }
                
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                    changeLog = context.LocationChangeLogs.OrderByDescending(x => x.LocationChangeTimeStamp).Take(nRecords).ToList();
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return changeLog;
        }

        /// <summary>
        /// Log the change from one status to another
        /// </summary>
        /// <param name="userID">The user to log the change for</param>
        /// <param name="newStatusID">The new status</param>
        /// <param name="oldStatusID">the old status</param>
        /// <param name="stateInitTimeStamp">The time that the previous state was initiated</param>
        public static void LogStatusChange(int userID, int newStatusID, int oldStatusID, DateTime stateInitTimeStamp)
        {
            StatusChangeLog changeLog = new StatusChangeLog();

            try
            {
                // Ensure that this is a valid status change
                if (newStatusID == oldStatusID)
                    throw new Exception("Status IDs are identical");

                changeLog.UserID = userID;
                changeLog.NewStatusID = newStatusID;
                changeLog.OldStatusID = oldStatusID;
                changeLog.StatusChangeTimeStamp = DateTime.Now;
                changeLog.StatusInitTimeStamp = stateInitTimeStamp;

                // Save the new ChangeLog object to the db
                using (var context = new ABLCloudStaffContext())
                {
                    context.StatusChangeLogs.Add(changeLog);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Log the change from one Location to another
        /// </summary>
        /// <param name="userID">The user to log the change for</param>
        /// <param name="newLocID">The new location</param>
        /// <param name="oldLocID">The old location</param>
        /// <param name="stateInitTimeStamp">The time the previous state was initiated.</param>
        public static void LogLocationChange(int userID, int newLocID, int oldLocID, DateTime stateInitTimeStamp)
        {
            LocationChangeLog changeLog = new LocationChangeLog();

            try
            {
                // Ensure that this is a valid status change
                if (newLocID == oldLocID)
                    throw new Exception("Location IDs are identical");

                changeLog.UserID = userID;
                changeLog.NewLocationID = newLocID;
                changeLog.OldLocationID = oldLocID;
                changeLog.LocationChangeTimeStamp = DateTime.Now;
                changeLog.LocationInitTimeStamp = stateInitTimeStamp;

                // Save the new ChangeLog object to the db
                using (var context = new ABLCloudStaffContext())
                {
                    context.LocationChangeLogs.Add(changeLog);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}