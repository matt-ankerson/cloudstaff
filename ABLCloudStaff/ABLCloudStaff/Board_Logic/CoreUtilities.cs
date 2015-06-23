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
                    coreInstances = context.Cores.ToList();
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
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static Status GetStatusByUserID(int userID)
        {
            // TODO
            return null;
        }

        // Write methods to change statuses and locations
    }
}