﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Biz_Logic
{
    /// <summary>
    /// Provides utilities necessary for fetching and manipulating info from the Location, UserLocation and User tables.
    /// </summary>
    public static class LocationUtilities
    {
        /// <summary>
        /// Gets all locations available for a specific username
        /// </summary>
        /// <param name="userID">The username to search on</param>
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
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return locations;
        }

        /// <summary>
        /// Get all Locations in the db
        /// </summary>
        /// <returns>List of all locations</returns>
        public static List<Location> GetAllLocations()
        {
            List<Location> locations = new List<Location>();

            try
            {
                using(var context = new ABLCloudStaffContext())
                {
                    locations = context.Locations.ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return locations;
        }

        /// <summary>
        /// Get all default locations
        /// </summary>
        /// <returns>List of locations</returns>
        public static List<Location> GetDefaultLocations()
        {
            List<Location> locations = new List<Location>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get default locations
                    List<int> defaultLocations = context.DefaultLocations.Select(x => x.LocationID).ToList();

                    locations = context.Locations.Where(x => defaultLocations.Contains(x.LocationID)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return locations;
        }

        /// <summary>
        /// Get a list of IDs corresponding to all locations that're currently in use.
        /// </summary>
        /// <returns>Lit of LocationIDs</returns>
        public static List<int> GetCurrentlyUsedLocationIDs()
        {
            List<int> locationIDs = new List<int>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    locationIDs = context.Cores.Select(x => x.LocationID).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return locationIDs;
        }

        /// <summary>
        /// Add a new location for all users
        /// </summary>
        /// <remarks>
        /// It is important that we add this location for future users. To do this, add a new default location to the DefaultLocation table.
        /// </remarks>
        /// <param name="locationName">The name of this location</param>
        public static void AddLocationForAllUsers(string locationName)
        {
            try
            {
                // Add the new Location
                using (var context = new ABLCloudStaffContext())
                {
                    // Add the new location to the Location table.
                    Location l = new Location { Name = locationName };
                    context.Locations.Add(l);
                    context.SaveChanges();
                }

                // Refresh the context
                using (var context = new ABLCloudStaffContext())
                {
                    // Pull out the locationID of the recently added location.
                    int latestLocationID = context.Locations.OrderBy(x => x.LocationID).Select(x => x.LocationID).ToList().LastOrDefault();

                    // Get a list of all users.
                    List<User> allUsers = UserUtilities.GetAllUsers();

                    // Loop over the users
                    foreach (User u in allUsers)
                    {
                        // Add a UserLocation object for this new locationID for every username.
                        UserLocation ul = new UserLocation { UserID = u.UserID, LocationID = latestLocationID, DateAdded = DateTime.Now };
                        context.UserLocations.Add(ul);
                        context.SaveChanges();
                    }

                    // Add a new default location
                    context.DefaultLocations.Add(new DefaultLocation { LocationID = latestLocationID });
                    // Save again
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
        /// Add a new location for all users
        /// </summary>
        /// <param name="locationName">The name of this location</param>
        public static void AddLocationForSingleUser(int userID, string locationName)
        {
            try
            {
                // Add the new Location
                using (var context = new ABLCloudStaffContext())
                {
                    // Add the new location to the Location table.
                    Location l = new Location { Name = locationName };
                    context.Locations.Add(l);
                    context.SaveChanges();
                }

                // Refresh the context
                using (var context = new ABLCloudStaffContext())
                {
                    // Pull out the locationID of the recently added location.
                    int latestLocationID = context.Locations.OrderBy(x => x.LocationID).Select(x => x.LocationID).ToList().LastOrDefault();

                    // Add a UserLocation object for this new locationID for the indicated username.
                    UserLocation ul = new UserLocation { UserID = userID, LocationID = latestLocationID, DateAdded = DateTime.Now };
                    context.UserLocations.Add(ul);
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
        /// Permanantly remove the indicated location from the db.
        /// </summary>
        /// <param name="locationID">The location to remove</param>
        public static void DeleteLocation(int locationID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    Location locationToDelete = context.Locations.Where(x => x.LocationID == locationID).FirstOrDefault();
                    context.Locations.Remove(locationToDelete);
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
        /// Update the indicated Location with the given information
        /// </summary>
        /// <param name="locationID">The Location to update</param>
        /// <param name="name">The new name for this Location</param>
        public static void UpdateLocation(int locationID, string name)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the indicated Location object
                    Location locationToUpdate = context.Locations.Where(x => x.LocationID == locationID).FirstOrDefault();

                    if (locationToUpdate == null)
                        throw new Exception("Bad LocationID, Location does not exist.");

                    // Update the fields on the Location object
                    locationToUpdate.Name = name;
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
    /// Object to hold information about a Location
    /// </summary>
    public class LocationInfo
    {
        public string locationID;
        public string name;
    }
}