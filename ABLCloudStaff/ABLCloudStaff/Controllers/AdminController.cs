using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABLCloudStaff.Models;
using ABLCloudStaff.Biz_Logic;

namespace ABLCloudStaff.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Admin()
        {
            // Check Session for username
            string username = null;
            
            try
            {
                username = Session["username"].ToString();
            }
            catch (Exception ex) { }

            // If we have a username in session:
            if (string.IsNullOrEmpty(username))
            {
                // There is no username in session, redirect to the login screen.
                return RedirectToAction("LoginAdmin", "LoginAdmin");
            }
            else
            {
                // We have a username in session, return the admin screen.
                return View();
            }
        }

        /// <summary>
        /// Destroy the session's username variable and redirect to the main board.
        /// </summary>
        /// <returns>Redirect to Action (Main board)</returns>
        public ActionResult Logout()
        {
            try
            {
                // Destroy the session variable.
                Session["username"] = null;
            }
            catch (Exception ex) { }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Accept parameters necessary for adding a username.
        /// </summary>
        /// <returns>An ActionResult object</returns>
        [HttpPost]
        public ActionResult AddNewUser(string firstName, string lastName, string userTypeID, string active)
        {
            int actualUserTypeID = Convert.ToInt32(userTypeID);
            bool actualActive;

            if (active == "on")
                actualActive = true;
            else
                actualActive = false;

            // Perform the insertion.
            // This will also add a new core instance to the database for the new username
            // ... and all default statuses and locations.
            UserUtilities.AddUser(firstName, lastName, actualUserTypeID, actualActive);

            return RedirectToAction("Admin", "Admin");
        }

        /// <summary>
        /// Delete a username and their dependencies
        /// </summary>
        /// <param name="userID">The username to delete</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveUser(string userID)
        {
            // If we have a username ID
            if (!string.IsNullOrEmpty(userID))
            {
                int actualUserID = Convert.ToInt32(userID);
                UserUtilities.FlagUserDeleted(actualUserID);
            }

            return RedirectToAction("Admin", "Admin");
        }

        /// <summary>
        /// Get all username's names and IDs currently in database
        /// </summary>
        /// <returns>A Dictionary of users</returns>
        public JsonResult GetAllUsers()
        {
            // This dictionary will hold userID and corresponding name
            Dictionary<string, string> usersDict = new Dictionary<string, string>();

            List<User> rawUsers = UserUtilities.GetAllUsers();

            foreach(User u in rawUsers)
            {
                usersDict.Add(u.UserID.ToString(), u.FirstName + " " + u.LastName);
            }

            return Json(usersDict, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all users in verbose detail
        /// </summary>
        /// <returns>An Ienumberable of type UserInfo in JSON format</returns>
        public JsonResult GetFullUserInformations()
        {
            List<UserInfo> userInfo = new List<UserInfo>();

            // Get all our users (regardless of deleted status)
            List<User> allUsers = UserUtilities.GetAllUsers();

            // Iterate our users and build our list of stringified username information
            foreach (User user in allUsers)
            {
                UserInfo ui = new UserInfo();
                ui.userID = user.UserID.ToString();
                ui.firstName = user.FirstName;
                ui.lastName = user.LastName;
                ui.userType = user.UserType.Type;
                ui.userTypeID = user.UserTypeID.ToString();
                ui.isActive = user.IsActive.ToString();
                // Add to the list of verbose username details
                userInfo.Add(ui);
            }

            IEnumerable<UserInfo> data = userInfo;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Accept parameters necessary to update a username's information
        /// </summary>
        /// <param name="userID">The username's userID</param>
        /// <param name="firstName">String to set the firstName</param>
        /// <param name="lastName">String to set the lastName</param>
        /// <param name="userType">String to set the username type ID</param>
        /// <param name="isActive">String to indicate IsActive status</param>
        /// <returns>ActionResult object</returns>
        [HttpPost]
        public ActionResult UpdateUser(string userID, string firstName, string lastName, string userType, string isActive)
        {
            // Convert fields to appropriate types
            bool actualIsActive;

            if ((!string.IsNullOrEmpty(userID)) && (!string.IsNullOrEmpty(userType)))
            {
                int actualUserID = Convert.ToInt32(userID);
                int actualUserTypeID = Convert.ToInt32(userType);
                if (isActive == "on")
                    actualIsActive = true;
                else
                    actualIsActive = false;

                // Perform the update
                UserUtilities.UpdateUser(actualUserID, firstName, lastName, actualUserTypeID, actualIsActive);
            }

            return RedirectToAction("Admin", "Admin");
        }

        /// <summary>
        /// Update the indicated Status with the given information
        /// </summary>
        /// <param name="statusID">The Status to update</param>
        /// <param name="name">The new status name</param>
        /// <param name="available">Is this an 'available' status?</param>
        /// <returns>ActionResult object</returns>
        [HttpPost]
        public ActionResult UpdateStatus(string statusID, string name, string available)
        {
            // Convert fields to appropriate types
            bool actualAvailable;

            if((!string.IsNullOrEmpty(statusID)) && (!string.IsNullOrEmpty(name)))
            {
                int actualStatusID = Convert.ToInt32(statusID);

                if (available == "on")
                    actualAvailable = true;
                else
                    actualAvailable = false;

                // Perform the update
                StatusUtilities.UpdateStatus(actualStatusID, name, actualAvailable);
            }

            return RedirectToAction("Admin", "Admin");
        }

        /// <summary>
        /// Update an indicated location with the given information
        /// </summary>
        /// <param name="locationID">Location to update</param>
        /// <param name="name">The new name to assign</param>
        /// <returns>An ActionResult object</returns>
        public ActionResult UpdateLocation(string locationID, string name)
        {
            if((!string.IsNullOrEmpty(locationID)) && (!string.IsNullOrEmpty(name)))
            {
                int actualLocationID = Convert.ToInt32(locationID);

                // Perform the update
                LocationUtilities.UpdateLocation(actualLocationID, name);
            }

            return RedirectToAction("Admin", "Admin");
        }

        /// <summary>
        /// Get all available statuses, regardless of who they belong to.
        /// </summary>
        /// <returns>List of Statuses</returns>
        public JsonResult GetAllStatuses()
        {
            List<Status> statuses = StatusUtilities.GetAllStatuses();
            List<StatusInfo> statusInfos = new List<StatusInfo>();

            // Build the flat StatusInfo list
            foreach(Status s in statuses)
            {
                StatusInfo si = new StatusInfo { name = s.Name, statusID = s.StatusID.ToString(), available = s.Available.ToString() };
                statusInfos.Add(si);
            }

            IEnumerable<StatusInfo> data = statusInfos;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all available locations, regardless of who they belong to.
        /// </summary>
        /// <returns>List of Locations</returns>
        public JsonResult GetAllLocations()
        {
            List<Location> locations = LocationUtilities.GetAllLocations();
            List<LocationInfo> locationInfos = new List<LocationInfo>();

            // Build the flat StatusInfo list
            foreach (Location l in locations)
            {
                LocationInfo li = new LocationInfo { name = l.Name, locationID = l.LocationID.ToString() };
                locationInfos.Add(li);
            }

            IEnumerable<LocationInfo> data = locationInfos;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all available statuses, return as a dictionary
        /// </summary>
        /// <returns>Dictionary containing status and statusID</returns>
        public JsonResult GetAllStatusesForAutoComplete()
        {
            Dictionary<string, string> statusDict = new Dictionary<string, string>();
            List<Status> statuses = StatusUtilities.GetAllStatuses();

            foreach(Status s in statuses)
            {
                statusDict.Add(s.StatusID.ToString(), s.Name);
            }

            return Json(statusDict, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all available locations, return as a dictionary
        /// </summary>
        /// <returns>Dictionary containing locations and locationIDs</returns>
        public JsonResult GetAllLocationsForAutoComplete()
        {
            Dictionary<string, string> locationDict = new Dictionary<string, string>();
            List<Location> locations = LocationUtilities.GetAllLocations();

            foreach (Location l in locations)
            {
                locationDict.Add(l.LocationID.ToString(), l.Name);
            }

            return Json(locationDict, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Add a new status
        /// </summary>
        /// <param name="statusName">The name of the status in question</param>
        /// <param name="available">Indicates whether or not this status is considered 'in office'</param>
        /// <param name="userID">If not null, add the status just for this username.</param>
        /// <returns>ActionResult object</returns>
        public ActionResult AddStatus(string statusName, string available, string userID)
        {
            bool actualAvailable = false;
            int actualUserID = 0;

            if (!string.IsNullOrEmpty(statusName))
            {
                if (available == "on")
                    actualAvailable = true;

                // If we've got a userID, then we need to add the status ONLY for that username.
                if (!string.IsNullOrEmpty(userID))
                {
                    actualUserID = Convert.ToInt32(userID);
                    StatusUtilities.AddStatusForSingleUser(actualUserID, statusName, actualAvailable);
                }
                else
                {
                    StatusUtilities.AddStatusForAllUsers(statusName, actualAvailable);
                }         
            }

            return RedirectToAction("Admin", "Admin");
        }

        /// <summary>
        /// Add a new location
        /// </summary>
        /// <param name="locationName">The name of this location</param>
        /// <param name="userID">Optional, indicates the username to assign the location to.</param>
        /// <returns>ActionResult object</returns>
        public ActionResult AddLocation(string locationName, string userID)
        {
            int actualUserID = 0;

            if (!string.IsNullOrEmpty(locationName))
            {

                // If we've got a userID, then we need to add the lacation ONLY for that username.
                if (!string.IsNullOrEmpty(userID))
                {
                    actualUserID = Convert.ToInt32(userID);
                    LocationUtilities.AddLocationForSingleUser(actualUserID, locationName);
                }
                else
                {
                    LocationUtilities.AddLocationForAllUsers(locationName);
                }
            }

            return RedirectToAction("Admin", "Admin");
        }

        /// <summary>
        /// Remove an indicated Status, providing it's not currently in use.
        /// </summary>
        /// <param name="statusID"></param>
        /// <returns></returns>
        public ActionResult RemoveStatus(string statusID)
        {
            if (!string.IsNullOrEmpty(statusID))
            {
                int actualStatusID = Convert.ToInt32(statusID);

                // Get all StatusIDs that are currently 'in use'
                List<int> inUseStatusIDs = StatusUtilities.GetCurrentlyUsedStatusIDs();

                // If our statusID is freed up:
                if(!inUseStatusIDs.Contains(actualStatusID))
                {
                    // Perform the removal. (This will be a HARD DELETE)
                    StatusUtilities.DeleteStatus(actualStatusID);
                }
                else
                {
                    ViewBag.Message = "Cannot remove Status because it is currently in use.";
                }
            }

            return RedirectToAction("Admin", "Admin");
        }

        /// <summary>
        /// Remove an indicated Location, providing it's not currently in use.
        /// </summary>
        /// <param name="locationID">The location to delete</param>
        /// <returns>An ActionResult object</returns>
        public ActionResult RemoveLocation(string locationID)
        {
            if (!string.IsNullOrEmpty(locationID))
            {
                int actualLocationID = Convert.ToInt32(locationID);

                // Get all LocationIDs that are currently 'in use'
                List<int> inUseLocationIDs = LocationUtilities.GetCurrentlyUsedLocationIDs();

                // If our locationID is freed up, and isn't the core location
                if (!inUseLocationIDs.Contains(actualLocationID))
                {
                    if(actualLocationID != Constants.DEFAULT_LOCATION)
                    {
                        // Perform the removal. (This will be a HARD DELETE)
                        LocationUtilities.DeleteLocation(actualLocationID);
                    }
                    else
                    {
                        ViewBag.Message = "Cannot remove Location because it is a core location.";
                    }
                }
                else
                {
                    ViewBag.Message = "Cannot remove Location because it is currently in use.";
                }
            }

            return RedirectToAction("Admin", "Admin");
        }

        /// <summary>
        /// Get all status changes until an indicated point.
        /// </summary>
        /// <returns>A List of status change information objects</returns>
        public JsonResult GetStatusChanges()
        {
            
            List<ChangeLogInfo> changeLogInfo = new List<ChangeLogInfo>();
            int nRecords = 10;

            // Get the data from the status change log table ()
            List<StatusChangeLog> statusChangeLog = ChangeLogUtilities.GetStatusChanges(nRecords);

            foreach(var log in statusChangeLog)
            {
                ChangeLogInfo cli = new ChangeLogInfo();
                cli.firstName = log.User.FirstName;
                cli.lastName = log.User.LastName;
                cli.oldState = log.OldStatus.Name;
                cli.newState = log.NewStatus.Name;
                // Assign a meaningful date/time string
                cli.stateChangeTimestamp = log.StatusChangeTimeStamp.ToShortDateString() + ", " + log.StatusChangeTimeStamp.ToShortTimeString();
                cli.prevStateInitTimestamp = log.StatusInitTimeStamp.ToShortDateString() + ", " + log.StatusInitTimeStamp.ToShortTimeString();
                // Add to the basic object list
                changeLogInfo.Add(cli);
            }

            IEnumerable<ChangeLogInfo> data = changeLogInfo;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all location changes back until a certain point
        /// </summary>
        /// <returns>A list of location change objects</returns>
        public JsonResult GetLocationChanges()
        {
            List<ChangeLogInfo> changeLogInfo = new List<ChangeLogInfo>();
            int nRecords = 10;

            // Get the data from the location change log table ()
            List<LocationChangeLog> locationChangeLog = ChangeLogUtilities.GetLocationChanges(nRecords);

            foreach (var log in locationChangeLog)
            {
                ChangeLogInfo cli = new ChangeLogInfo();
                cli.firstName = log.User.FirstName;
                cli.lastName = log.User.LastName;
                cli.oldState = log.OldLocation.Name;
                cli.newState = log.NewLocation.Name;
                // Assign a meaningful date/time string
                cli.stateChangeTimestamp = log.LocationChangeTimeStamp.ToShortDateString() + ", " + log.LocationChangeTimeStamp.ToShortTimeString();
                cli.prevStateInitTimestamp = log.LocationInitTimeStamp.ToShortDateString() + ", " + log.LocationInitTimeStamp.ToShortTimeString();
                // Add to the basic object list
                changeLogInfo.Add(cli);
            }

            IEnumerable<ChangeLogInfo> data = changeLogInfo;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets all possible username types
        /// </summary>
        /// <returns>A list of type UserType</returns>
        public JsonResult GetUserTypes()
        {
            // Get all username types
            List<UserType> userTypes = UserUtilities.GetAllUserTypes();

            IEnumerable<UserType> data = userTypes;

            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}