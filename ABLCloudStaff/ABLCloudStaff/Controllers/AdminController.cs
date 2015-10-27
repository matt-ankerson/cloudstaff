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

                // Check for any error messages in TempData that we should propagate to the UI.
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"].ToString();
                }
                

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
        public ActionResult AddNewUser(string firstName, string lastName, string userTypeID, string active, string username, string password, string passwordConfirm)
        {
            int actualUserTypeID = Convert.ToInt32(userTypeID);

            // Is this an acitve user?
            bool actualActive;
            if (active == "on")
                actualActive = true;
            else
                actualActive = false;

            // Perform the insertion.
            // This will also add:
            // - A new core instance to the database for the new user.
            // - All default statuses and locations.
            // - A new authentication instance for this user to use for the API. (and admin interface if appropriate)
            try
            {
                // Before performing inset, ensure the passwords match.
                if (!password.Equals(passwordConfirm))
                    throw new Exception("Passwords do not match.");

                UserUtilities.AddUser(firstName, lastName, actualUserTypeID, actualActive, username, password);
            }
            catch (Exception ex)
            {
                // Report the error
                ViewBag.Message = "There was an error: " + ex.Message;
                return View("Admin");
            }

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
        /// Get a dicitonary of all general and admin users
        /// </summary>
        /// <returns>Return json dict of users.</returns>
        public JsonResult GetGeneralAndAdminUsers()
        {
            // This dictionary will hold userID and corresponding name
            Dictionary<string, string> usersDict = new Dictionary<string, string>();

            List<User> rawUsers = UserUtilities.GetGeneralAndAdminUsers();

            foreach (User u in rawUsers)
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
                if (user.Authentication != null)
                    ui.username = user.Authentication.UserName;
                else
                    ui.username = "";
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
        public ActionResult UpdateUser(string userID, string firstName, string lastName, string userType, 
            string isActive, string username, string password, string passwordCheck)
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

                // Perform the update.
                // We also need to update the user's associated authentication information.
                try
                {
                    // Before performing inset, ensure the passwords match.
                    if (!password.Equals(passwordCheck))
                        throw new Exception("Passwords do not match.");

                    UserUtilities.UpdateUser(actualUserID, firstName, lastName, actualUserTypeID, actualIsActive);

                    // Now update the fields on the associated Authentication instance. (Providing a password was provided)
                    if (!string.IsNullOrEmpty(password))
                        AuthenticationUtilities.UpdateAuthentication(actualUserID, username, password);
                }
                catch (Exception ex)
                {
                    // Report the error
                    ViewBag.Message = "There was an error: " + ex.Message;
                    return View("Admin");
                }
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
                    // If this is not a core status:
                    if ((actualStatusID != Constants.DEFAULT_IN_STATUS) || (actualStatusID != Constants.DEFAULT_IN_STATUS))
                    {
                        try
                        {
                            // Perform the removal. (This will be a HARD DELETE)
                            StatusUtilities.DeleteStatus(actualStatusID);
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "There was a problem removing the requested Status.";
                            return PartialView("Admin", "Admin");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Cannot remove Status because it is a core Status.";
                        return PartialView("Admin", "Admin");
                    }
                }
                else
                {
                    ViewBag.Message = "Cannot remove Status because it is currently in use.";
                    return PartialView("Admin", "Admin");
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
                        try
                        {
                            // Perform the removal. (This will be a HARD DELETE)
                            LocationUtilities.DeleteLocation(actualLocationID);
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "There was a problem removing the requested location.";
                            return PartialView("Admin", "Admin");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Cannot remove Location because it is a core location.";
                        return PartialView("Admin", "Admin");
                    }
                }
                else
                {
                    ViewBag.Message = "Cannot remove Location because it is currently in use.";
                    return PartialView("Admin", "Admin");
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

        /// <summary>
        /// Get a list of all groups (active or not)
        /// </summary>
        /// <returns>List of GroupInfo objects in JSON format.</returns>
        public JsonResult GetAllGroups()
        {
            List<GroupInfo> groupInfos = new List<GroupInfo>();           
            List<Group> groups = GroupUtilities.GetAllGroups();

            foreach (Group g in groups)
            {
                groupInfos.Add(new GroupInfo {
                     GroupID = g.GroupID.ToString(),
                     Active = g.Active.ToString(),
                     Name = g.Name,
                     Priority = g.Priority.ToString()
                });
            }

            IEnumerable<GroupInfo> data = groupInfos;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Return a list of users belonging to an indicated group.
        /// </summary>
        /// <param name="groupID">The group to get members of.</param>
        /// <returns>Dictionary of users and thier id.</returns>
        public JsonResult GetMembersOfGroup(string groupID)
        {
            Dictionary<string, string> members = new Dictionary<string, string>();

            if(!string.IsNullOrEmpty(groupID))
            {
                int actualGroupID = Convert.ToInt32(groupID);
                List<User> rawMembers = GroupUtilities.GetMembersOfGroup(actualGroupID);

                foreach (User u in rawMembers)
                {
                    members.Add(u.UserID.ToString(), u.FirstName + " " + u.LastName);
                }
            }

            return Json(members, JsonRequestBehavior.AllowGet);
            
        }

        /// <summary>
        /// Update an indicated group with the given information.
        /// </summary>
        /// <param name="groupID">Group to update</param>
        /// <param name="name">New name for group</param>
        /// <param name="priority">New priority value for group</param>
        /// <returns></returns>
        public ActionResult UpdateGroup(string groupID, string name, List<string> members, string priority="0")
        {
            try
            {
                if (string.IsNullOrEmpty(groupID))
                    throw new Exception("Group ID failed to propagate.");
                if (string.IsNullOrEmpty(name))
                    throw new Exception("No group name supplied.");
                if ((members.Count <= 0)||(members == null))
                    throw new Exception("At least one member must be supplied for a group.");

                int actualGroupID = Convert.ToInt32(groupID);
                int actualPriority = Convert.ToInt32(priority);
                List<int> actualMembers = new List<int>();

                // Build list of integer IDs for members
                foreach (string userIDStr in members)
                    actualMembers.Add(Convert.ToInt32(userIDStr));

                // Perform the update. This will replace members with those supplied.
                GroupUtilities.UpdateGroup(actualGroupID, name, actualPriority, actualMembers);
            }
            catch (Exception ex)
            {
                // Report the error
                TempData["Message"] = "There was an error: " + ex.Message;
            }

            return RedirectToAction("Admin", "Admin");
        }

        /// <summary>
        /// Remove an indicated group.
        /// </summary>
        /// <param name="groupID">The group to remove.</param>
        /// <returns>Redirects to admin home.</returns>
        public ActionResult RemoveGroup(string groupID)
        {
            try
            {
                int actualGroupID = Convert.ToInt32(groupID);
                // Perform the deletion:
                GroupUtilities.RemoveGroup(actualGroupID);
            }
            catch (Exception ex)
            {
                // Report the error
                ViewBag.Message = "There was an error: " + ex.Message;
                return View("Admin");
            }

            return RedirectToAction("Admin", "Admin");
        }
	}
}