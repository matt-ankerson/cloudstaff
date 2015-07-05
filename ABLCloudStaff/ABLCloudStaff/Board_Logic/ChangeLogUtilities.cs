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
        /// Log the change from one state to another
        /// </summary>
        /// <param name="oldState">The previous state</param>
        /// <param name="newState">The new state</param>
        public static void LogChange(Core oldState, Core newState)
        {
            // Declare a ChangeLog object
            ChangeLog cl = new ChangeLog();

            try
            {
                // Ensure that both Core objects are for the same user.
                if (oldState.UserID != newState.UserID)
                    throw new Exception("Core objects are not of the same user.");

                // Load the ChangeLog object up with info from the old and new Core objects which hold our states.
                cl.UserID = oldState.UserID;
                cl.NewStatusID = newState.StatusID;
                cl.OldStatusID = oldState.StatusID;
                cl.NewLocationID = newState.LocationID;
                cl.OldLocationID = oldState.LocationID;
                cl.StateChangeTimeStamp = DateTime.Now;
                cl.StateInitTimeStamp = oldState.StateStart;

                // Save the new ChangeLog object to the db
                using (var context = new ABLCloudStaffContext())
                {
                    context.ChangeLogs.Add(cl);
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