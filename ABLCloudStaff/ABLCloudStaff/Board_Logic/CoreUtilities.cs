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
                    coreInstances = context.Cores.Include("User").Include("Status").Include("Location").ToList();
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
        public static void UpdateStatus(int userID, int newStatusID)
        {
            Core thisCore = new Core();
            Status newStatus = new Status();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the Core instance indicated by the given UserID
                    thisCore = context.Cores.Where(c => c.UserID == userID).FirstOrDefault();

                    // Get the new status from the status table
                    newStatus = context.Statuses.Where(s => s.StatusID == newStatusID).FirstOrDefault();

                    // Assign the new status to our Core instance
                    thisCore.StatusID = newStatus.StatusID;
                    thisCore.Status = newStatus;

                    context.SaveChanges();
                }
                
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
            Core thisCore = new Core();
            Location thisLoc = new Location();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the Core instance indicated by the given userID
                    thisCore = context.Cores.Where(c => c.UserID == userID).FirstOrDefault();

                    // Get the new location from the location table
                    thisLoc = context.Locations.Where(l => l.LocationID == newLocID).FirstOrDefault();

                    // Assign the new location to our Core instance
                    thisCore.LocationID = thisLoc.LocationID;
                    thisCore.Location = thisLoc;

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