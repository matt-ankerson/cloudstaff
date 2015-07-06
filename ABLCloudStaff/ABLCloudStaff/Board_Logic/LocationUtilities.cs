using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Board_Logic
{
    /// <summary>
    /// Provides utilities necessary for fetching and manipulating info from the Location, UserLocation and User tables.
    /// </summary>
    public static class LocationUtilities
    {
        /// <summary>
        /// Gets all locations available for a specific user
        /// </summary>
        /// <param name="userID">The user to search on</param>
        /// <returns>A list containing all relavent locations.</returns>
        public static List<Location> GetAvailableLocations(int userID)
        {
            List<Location> locations = new List<Location>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the appropriate UserLocation objects
                    List<UserLocation> uls = context.UserLocations.Include("Location").Where(x => x.UserID == userID).ToList();
                    // Build the list of actual Locations
                    foreach (UserLocation userLoc in uls)
                        locations.Add(userLoc.Location);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return locations;
        }
    }
}