using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Board_Logic
{
    /// <summary>
    /// Provides utilities necessary for fetching and manipulating info from the Core table.
    /// </summary>
    public static class CoreUtilities
    {
        /// <summary>
        /// Gets all core instances
        /// </summary>
        /// <returns>All core instances</returns>
        public static List<Core> GetAllCoreInstances()
        {
            List<Core> coreInstances = new List<Core>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    coreInstances = context.Cores.Include("User").Include("Status").Include("Location").Where(x => x.User.IsActive == true).OrderBy(x => x.User.LastName).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return coreInstances;
        }

        /// <summary>
        /// Get a single core instance by CoreID
        /// </summary>
        /// <param name="coreID"></param>
        /// <returns>The core instance desired</returns>
        public static Core GetCoreInstanceByCoreID(int coreID)
        {
            Core thisInstance = new Core();

            try
            {
               using(var context = new ABLCloudStaffContext())
               {
                   thisInstance = context.Cores.Where(c => c.CoreID == coreID).FirstOrDefault();
               }
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return thisInstance;
        }

        /// <summary>
        /// Get a single Core instance by UserID
        /// </summary>
        /// <param name="userID">The UserID to search on</param>
        /// <returns>The Core instance for the given user</returns>
        public static Core GetCoreInstanceByUserID(int userID)
        {
            Core thisInstance;

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    thisInstance = context.Cores.Where(c => c.UserID == userID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return thisInstance;
        }

        /// <summary>
        /// Get a List of Core instances, by an arbitrarily lengthed list of CoreIDs
        /// </summary>
        /// <param name="coreIDs">The List of CoreIDs to fetch Core instances for</param>
        /// <returns>A List of Core instances</returns>
        public static List<Core> GetArbitraryNumberOfCoresByCoreID(List<int> coreIDs)
        {
            List<Core> coreInstances = new List<Core>();
            Core thisInstance;

            try
            {
                // Loop over the list of IDs
                foreach(int coreID in coreIDs)
                {
                    using(var context = new ABLCloudStaffContext())
                    {
                        // Get the Core instance indicated by this ID
                        thisInstance = context.Cores.Where(c => c.CoreID == coreID).FirstOrDefault();
                        // Add it to the List of Cores
                        coreInstances.Add(thisInstance);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return coreInstances;
        }

        /// <summary>
        /// Gets a list of Core instances, by an arbitrarily lengthed list of userIDs.
        /// </summary>
        /// <param name="userIDs">The UserIDs to search on</param>
        /// <returns>The list of correspinding Core instances</returns>
        public static List<Core> GetArbitraryNumberOfCoresByUserID(List<int> userIDs)
        {
            List<Core> coreInstances = new List<Core>();
            Core thisInstance;

            try
            {
                // Loop over the list of IDs
                foreach (int userID in userIDs)
                {
                    using (var context = new ABLCloudStaffContext())
                    {
                        // Get the Core instance indicated by this ID
                        thisInstance = context.Cores.Where(c => c.UserID == userID).FirstOrDefault();
                        // Add it to the List of Cores
                        coreInstances.Add(thisInstance);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return coreInstances;
        }

        /// <summary>
        /// Get the status held by a single core instance
        /// </summary>
        /// <param name="coreID">The core we want to pull the status from</param>
        /// <returns>A Status object</returns>
        public static Status GetStatusByCoreID(int coreID)
        {
            Core thisCore = new Core();
            Status thisStatus = new Status();

            try
            {
                using(var context = new ABLCloudStaffContext())
                {
                    // Eagerly load Status
                    thisCore = context.Cores.Include("Status").Where(c => c.CoreID == coreID).FirstOrDefault();
                }
                // Get the status
                thisStatus = thisCore.Status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return thisStatus;
        }

        /// <summary>
        /// Gets the Status held by a single Core instance
        /// </summary>
        /// <param name="userID">The UserID to search on</param>
        /// <returns>The Status object we need</returns>
        public static Status GetStatusByUserID(int userID)
        {
            Core thisCore = new Core();
            Status thisStatus = new Status();

            try
            {
                using(var context = new ABLCloudStaffContext())
                {
                    // Eagerly load status
                    thisCore = context.Cores.Include("Status").Where(c => c.UserID == userID).FirstOrDefault();
                }
                // Get the status
                thisStatus = thisCore.Status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return thisStatus;
        }
        
        /// <summary>
        /// Replace the status in the indicated core instance with the new desired status
        /// </summary>
        /// <param name="userID">The core instance to modify</param>
        /// <param name="newStatusID">The id of the new Status</param>
        /// <param name="returnTime">The time at which this user intends to return from this status</param>
        public static void UpdateStatus(int userID, int newStatusID, string returnTime = "")
        {
            Core currentCore = new Core();
            Status newStatus = new Status();

            int previousStatusID;
            DateTime previousStateInitTime;

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the Core instance indicated by the given UserID
                    currentCore = context.Cores.Where(c => c.UserID == userID).FirstOrDefault();

                    // Save info from the old state that we need.
                    previousStatusID = currentCore.StatusID;
                    previousStateInitTime = new DateTime(currentCore.StateStart.Year, 
                        currentCore.StateStart.Month, 
                        currentCore.StateStart.Day, 
                        currentCore.StateStart.Hour, 
                        currentCore.StateStart.Minute, 
                        currentCore.StateStart.Second, 
                        currentCore.StateStart.Millisecond);

                    // Proceed with update

                    // Get the new status from the status table
                    newStatus = context.Statuses.Where(s => s.StatusID == newStatusID).FirstOrDefault();

                    // Assign the new status to our Core instance
                    currentCore.StatusID = newStatus.StatusID;
                    currentCore.Status = newStatus;
                    currentCore.StateStart = DateTime.Now;

                    // If a return time was not supplied, remove returnTime from the Core instance
                    if(string.IsNullOrEmpty(returnTime))
                    {
                        // check if exists
                        if (currentCore.IntendedEndTime != null)
                        {
                            // remove the returnTime
                            currentCore.IntendedEndTime = null;
                        }
                    }
                    else    // if a return time was supplied, save it to the core instance
                    {
                        // Parse the returnTime to a valid datetime
                        DateTime theReturnTime = DateTime.Parse(returnTime);
                        currentCore.IntendedEndTime = theReturnTime;
                    }

                    // BUT: if the statusID supplied is the DEFAULT status, then it's essential that we remove returnTime from the core instance if it exists
                    if(newStatusID == Constants.DEFAULT_IN_STATUS)
                    {
                        // check if exists
                        if(currentCore.IntendedEndTime != null)
                        {
                            // remove the returnTime
                            currentCore.IntendedEndTime = null;
                        }
                    }    

                    // Commit to the database
                    context.SaveChanges();
                }
                

                // Submit to change-log
                ChangeLogUtilities.LogStatusChange(userID, newStatusID, previousStatusID, previousStateInitTime);
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Replace the location in the indicated core instance with the new desired location
        /// </summary>
        /// <param name="userID">The core instance to modify</param>
        /// <param name="newLocID">The id of the new Location</param>
        public static void UpdateLocation(int userID, int newLocID)
        {
            Core currentCore = new Core();
            Location newLocation = new Location();

            int previousLocationID;
            DateTime previousStateInitTime;

            // Assume an update is necessary
            bool updateNecessary = true;

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the Core instance indicated by the given UserID
                    currentCore = context.Cores.Where(c => c.UserID == userID).FirstOrDefault();

                    // Save info from the old state that we need.
                    previousLocationID = currentCore.LocationID;
                    previousStateInitTime = new DateTime(currentCore.StateStart.Year,
                        currentCore.StateStart.Month,
                        currentCore.StateStart.Day,
                        currentCore.StateStart.Hour,
                        currentCore.StateStart.Minute,
                        currentCore.StateStart.Second,
                        currentCore.StateStart.Millisecond);

                    // Is this update necessary?
                    if (previousLocationID == newLocID)
                    {
                        updateNecessary = false;
                    }
                    else  // If the locationIDs are different, go ahead with the update.
                    {
                        // Get the new location from the status table
                        newLocation = context.Locations.Where(l => l.LocationID == newLocID).FirstOrDefault();

                        // Assign the new Location to our Core instance
                        currentCore.LocationID = newLocation.LocationID;
                        currentCore.Location = newLocation;
                        currentCore.StateStart = DateTime.Now;

                        // Commit to the database
                        context.SaveChanges();
                    }
                }

                // Submit to change-log
                // But, we don't want to log a change if the Locaton hasn't actually changed.
                if (updateNecessary)
                    ChangeLogUtilities.LogLocationChange(userID, newLocID, previousLocationID, previousStateInitTime);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Set the given user's core to the default status and remove any abnormalities from the core instance.
        /// </summary>
        /// <param name="userID">User to apply the change to</param>
        public static void UpdateStatusIn(int userID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the appropriate core instance
                    Core thisCore = context.Cores.Where(x => x.UserID == userID).FirstOrDefault();
                    if (thisCore == null)
                        throw new Exception("User does not exist.");

                    // Grab some of the old information for making an addition to the log
                    int previousStatusID = thisCore.StatusID;
                    int previousLocationID = thisCore.LocationID;
                    DateTime previousStateInitTime = thisCore.StateStart;

                    // Modify the core instance accordingly
                    thisCore.StatusID = Constants.DEFAULT_IN_STATUS;
                    thisCore.LocationID = Constants.DEFAULT_LOCATION;
                    thisCore.StateStart = DateTime.Now;
                    thisCore.IntendedEndTime = null;

                    // Save changes
                    context.SaveChanges();

                    // Submit to the changelog if necessary.
                    if (previousStatusID != thisCore.StatusID)
                        ChangeLogUtilities.LogStatusChange(userID, thisCore.StatusID, previousStatusID, previousStateInitTime);
                    if (previousLocationID != thisCore.LocationID)
                        ChangeLogUtilities.LogLocationChange(userID, thisCore.LocationID, previousLocationID, previousStateInitTime);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Set the given user's status to 'out of office', remove any abnormalities from the core instance
        /// </summary>
        /// <param name="userID">The user to apply the change to.</param>
        public static void UpdateStatusOut(int userID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the appropriate core instance
                    Core thisCore = context.Cores.Where(x => x.UserID == userID).FirstOrDefault();
                    if (thisCore == null)
                        throw new Exception("User does not exist.");

                    // Grab some of the old information for making an addition to the log
                    int previousStatusID = thisCore.StatusID;
                    int previousLocationID = thisCore.LocationID;
                    DateTime previousStateInitTime = thisCore.StateStart;

                    // Modify the core instance accordingly
                    thisCore.StatusID = Constants.DEFAULT_OUT_STATUS;
                    thisCore.LocationID = Constants.DEFAULT_LOCATION;
                    thisCore.StateStart = DateTime.Now;
                    thisCore.IntendedEndTime = null;

                    // Save changes
                    context.SaveChanges();

                    // Submit to the changelog if necessary.
                    if (previousStatusID != thisCore.StatusID)
                        ChangeLogUtilities.LogStatusChange(userID, thisCore.StatusID, previousStatusID, previousStateInitTime);
                    if (previousLocationID != thisCore.LocationID)
                        ChangeLogUtilities.LogLocationChange(userID, thisCore.LocationID, previousLocationID, previousStateInitTime);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Add a new Core instance, for a new user.
        /// </summary>
        /// <param name="userID">The new user id, this user must exist.</param>
        /// <param name="statusID">The status id, this status must exist.</param>
        /// <param name="locationID">The location id, this location must exist.</param>
        public static void AddCore(int userID, int statusID, int locationID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // We need to ensure that the given user, status and location all exist.
                    List<int> existingUsers = context.Users.Select(x => x.UserID).ToList();
                    List<int> existingStatuses = context.Statuses.Select(x => x.StatusID).ToList();
                    List<int> existingLocations = context.Locations.Select(x => x.LocationID).ToList();
                    // We also need to make sure that this user doesn't already have a core instance.
                    List<int> existingCores = context.Cores.Select(x => x.UserID).ToList();

                    if (!existingUsers.Contains(userID))
                        throw new Exception("User does not exist.");
                    if (!existingStatuses.Contains(statusID))
                        throw new Exception("Status does not exist.");
                    if (!existingLocations.Contains(locationID))
                        throw new Exception("Location does not exist.");
                    if (existingCores.Contains(userID))
                        throw new Exception("User already has a core instance.");

                    // Create and add the new Core instance
                    Core newCore = new Core {UserID = userID, StatusID = statusID, LocationID = locationID, StateStart = DateTime.Now };
                    context.Cores.Add(newCore);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Remove the indicated core instance from the core table
        /// </summary>
        /// <param name="userID">Core instance indicated by userID</param>
        public static void DeleteCore(int userID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    var coreToDelete = context.Cores.SingleOrDefault(x => x.UserID == userID);

                    if(coreToDelete != null)
                    {
                        context.Cores.Remove(coreToDelete);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}