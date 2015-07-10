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
        public ActionResult AddNewUser(string firstName, string lastName)
        {
            // Perform the insertion.
            // This will also add a new core instance to the database for the new user
            // ... and all default statuses and locations.
            UserUtilities.AddUser(firstName, lastName);

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
            int actualUserID = Convert.ToInt32(userID);
            UserUtilities.DeleteUser(actualUserID);
            return View("Admin");
        }

        /// <summary>
        /// Get all users currently in database
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
                cli.stateChangeTimestamp = log.StatusChangeTimeStamp.ToShortTimeString();
                cli.prevStateInitTimestamp = log.StatusInitTimeStamp.ToShortTimeString();
                // Add to the basic object list
                changeLogInfo.Add(cli);
            }

            return Json(changeLogInfo, JsonRequestBehavior.AllowGet);
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
}