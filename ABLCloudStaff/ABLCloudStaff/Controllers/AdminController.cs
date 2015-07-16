using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABLCloudStaff.Models;
using ABLCloudStaff.Board_Logic;

namespace ABLCloudStaff.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Admin()
        {
            return View();
        }

        /// <summary>
        /// Accept parameters necessary for adding a user.
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
            // This will also add a new core instance to the database for the new user
            // ... and all default statuses and locations.
            UserUtilities.AddUser(firstName, lastName, actualUserTypeID, actualActive);

            return View("Admin");
        }

        /// <summary>
        /// Delete a user and their dependencies
        /// </summary>
        /// <param name="userID">The user to delete</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveUser(string userID)
        {
            // If we have a user ID
            if (!string.IsNullOrEmpty(userID))
            {
                int actualUserID = Convert.ToInt32(userID);
                UserUtilities.FlagUserDeleted(actualUserID);
            }
            
            return View("Admin");
        }

        /// <summary>
        /// Get all user's names and IDs currently in database
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

            // Iterate our users and build our list of stringified user information
            foreach (User user in allUsers)
            {
                UserInfo ui = new UserInfo();
                ui.userID = user.UserID.ToString();
                ui.firstName = user.FirstName;
                ui.lastName = user.LastName;
                ui.userType = user.UserType.Type;
                ui.userTypeID = user.UserTypeID.ToString();
                ui.isActive = user.IsActive.ToString();
                // Add to the list of verbose user details
                userInfo.Add(ui);
            }

            IEnumerable<UserInfo> data = userInfo;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Accept parameters necessary to update a user's information
        /// </summary>
        /// <param name="userID">The user's userID</param>
        /// <param name="firstName">String to set the firstName</param>
        /// <param name="lastName">String to set the lastName</param>
        /// <param name="userType">String to set the user type ID</param>
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

            return View("Admin");
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
                StatusInfo si = new StatusInfo { name = s.Name, statusID = s.StatusID.ToString(), worksite = s.Worksite.ToString() };
                statusInfos.Add(si);
            }

            IEnumerable<StatusInfo> data = statusInfos;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Accept params necessary to add a new status for a single user
        /// </summary>
        /// <param name="userID">The user to add the status to</param>
        /// <param name="name"></param>
        /// <param name="worksite"></param>
        /// <returns></returns>
        public ActionResult AddNewStatusForSingleUser(string userID, string name, string worksite)
        {
            return View("Admin");
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
        /// Gets all possible user types
        /// </summary>
        /// <returns>A list of type UserType</returns>
        public JsonResult GetUserTypes()
        {
            // Get all user types
            List<UserType> userTypes = UserUtilities.GetAllUserTypes();

            IEnumerable<UserType> data = userTypes;

            return Json(data, JsonRequestBehavior.AllowGet);
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

    /// <summary>
    /// Object to hold information about a user in verbose detail
    /// </summary>
    public class UserInfo
    {
        public string userID;
        public string firstName;
        public string lastName;
        public string userType;
        public string userTypeID;
        public string isActive;
    }

    /// <summary>
    /// Object to hold information about a status 
    /// </summary>
    public class StatusInfo
    {
        public string statusID;
        public string name;
        public string worksite;
    }
}