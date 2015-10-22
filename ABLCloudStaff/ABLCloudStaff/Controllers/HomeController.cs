using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Specialized;
using ABLCloudStaff.Models;
using ABLCloudStaff.Biz_Logic;

namespace ABLCloudStaff.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Get state information on each username, push to the core/home view
        /// </summary>
        /// <returns>An ActionResult object</returns>
        public ActionResult Index()
        {
            List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();

            return View(coreInfo);
        }

        /// <summary>
        /// Push a status, location and return time to the database for a specific user.
        /// </summary>
        /// <remarks>
        /// Only performs update if necessary.
        /// </remarks>
        /// <param name="userID">The userID of the user to perform the update for.</param>
        /// <param name="statusID">The new statusID</param>
        /// <param name="locationID">The new locationID.</param>
        /// <param name="returnTime">The new return time.</param>
        private void _submitStateUpdateForUser(int userID, int statusID, int locationID, string returnTime)
        {
            try
            {
                // Perform the update. ReturnTime is handled as an optional field. 
                CoreUtilities.UpdateStatus(userID, statusID, returnTime);
                CoreUtilities.UpdateLocation(userID, locationID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Push a status and location to the database for a specific username
        /// </summary>
        /// <returns>An ActionResult object</returns>
        [HttpPost]
        public ActionResult SubmitStatusOrLocationUpdate(string userID, string statusID, string locationID, string returnTime)
        {
            try
            {
                // If no userID is supplied, silently ignore this request.
                if (userID != null)
                {
                    // Convert our parameters into a useful data type
                    int actualUserID = Convert.ToInt32(userID);
                    int actualStatusID = Convert.ToInt32(statusID);
                    int actualLocationID = Convert.ToInt32(locationID);

                    _submitStateUpdateForUser(actualUserID, actualStatusID, actualLocationID, returnTime);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Failed to perform update for user. " + ex.Message;
                List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
                return View("Index", coreInfo);
            }

            //List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
            //return View("Index", coreInfo);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Push a status, location and return time to the database for a set of users.
        /// </summary>
        /// <param name="userIDs">All userIDs to perform the update for.</param>
        /// <param name="statusID">The new statusID</param>
        /// <param name="locationID">The new locationID</param>
        /// <param name="returnTime">The new return time.</param>
        /// <returns>Redirects to home page.</returns>
        [HttpPost]
        public ActionResult SubmitStatusOrLocationUpdateForGroup(List<string> userIDs, string statusID, string locationID, string returnTime)
        {
            try
            {
                // Convert our parameters into a useful data type
                int actualStatusID = Convert.ToInt32(statusID);
                int actualLocationID = Convert.ToInt32(locationID);

                // Integer list for saving a group to the Group table.
                List<int> members = new List<int>();
                
                // For each userID:
                foreach (string userIDStr in userIDs)
                {
                    if(!string.IsNullOrEmpty(userIDStr))
                    {
                        int actualUserID = Convert.ToInt32(userIDStr);
                        // Perform the update for this user:
                        _submitStateUpdateForUser(actualUserID, actualStatusID, actualLocationID, returnTime);
                        // Add the userID to the member list.
                        members.Add(actualUserID);
                    }  
                }

                // Add a group
                //GroupUtilities.AddGroup(members);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Failed to perform update for group. " + ex.Message;
                List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
                return View("Index", coreInfo);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Accept params necessary to add a new visitor to the workplace.
        /// </summary>
        /// <param name="firstName">Firstname of new visitor</param>
        /// <param name="lastName">Lastname of new visitor</param>
        /// <param name="company">Company associated with new visitor.</param>
        /// <param name="intendedDepartTime">The time that the visitor intends to leave.</param>
        /// <param name="existingVisitorUserID">When given, this field indicates that this is a returning visitor, thus we don't need to add a new user.</param>
        /// <returns>Redirects to home/index action.</returns>
        public ActionResult AddVisitor(string firstName, string lastName, string company, string intendedDepartTime, string userBeingVisitedID, string existingVisitorUserID = "")
        {
            int visitorUserID = 0;
            int actualUserBeingVisitedID = 0;

            DateTime timeOfDeparture = DateTime.Now;

            // Try parse the given return time.
            try
            {
                timeOfDeparture = DateTime.Parse(intendedDepartTime);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Could not add visitor, invalid return time given.";
                List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
                return View("Index", coreInfo);
            }  

            try
            {
                // Only accept this visit if a 'userBeingVisited' is defined.
                if (!string.IsNullOrEmpty(userBeingVisitedID))
                {
                    actualUserBeingVisitedID = Convert.ToInt32(userBeingVisitedID);

                    // We need to add a new user for the new visitor, unless we've been given an existing userID.
                    if (string.IsNullOrEmpty(existingVisitorUserID))
                    {
                        // Create a new user
                        visitorUserID = UserUtilities.AddUserAsVisitor(firstName, lastName);
                        // Set the 'intendedEndTime' on this core instance to be the visitor's intended time of departure.
                        CoreUtilities.SetReturnTimeForUser(visitorUserID, timeOfDeparture);
                    }
                    else
                    {
                        // Use the existing one.
                        visitorUserID = Convert.ToInt32(existingVisitorUserID);
                        // We also need to add a core instance again for this user.
                        CoreUtilities.AddCore(visitorUserID, Constants.VISITING_STATUS, Constants.DEFAULT_LOCATION);
                        // Set the 'intendedEndTime' on this core instance to be the visitor's intended time of departure.
                        CoreUtilities.SetReturnTimeForUser(visitorUserID, timeOfDeparture);
                        // Activate the user. (so they show up on the main board)
                        UserUtilities.FlagUserActive(visitorUserID);
                    }

                    // If we've now got a visitor user and a visited user:
                    if ((visitorUserID != 0) && (actualUserBeingVisitedID != 0))
                    {   

                        // Now we have everything we need to add a visitor log
                        VisitorLogUtilities.LogVisitorIn(visitorUserID, actualUserBeingVisitedID, company, timeOfDeparture);
                    }
                    else
                    {
                        // Report invalid users provided
                        ViewBag.Message = "Could not add visitor, invalid users provided.";
                        List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
                        return View("Index", coreInfo);
                    }
                }
                else
                {
                    // There was no user given that the visitor intends to visit.
                    ViewBag.Message = "Could not add visitor, no person to visit provided.";
                    List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
                    return View("Index", coreInfo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Could not add visitor, " + ex.Message;
                List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
                return View("Index", coreInfo);
            }
            
            // Success!
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Add a new group to the database and all indicated members.
        /// </summary>
        /// <param name="name">The name of this group</param>
        /// <param name="userIDs">The members to add.</param>
        /// <param name="active">Whether or not this group is out or in. (true would indicate they're out)</param>
        /// <returns>Error or redirects to home.</returns>
        public ActionResult AddGroup(string name, List<string> userIDs, string active)
        {
            try
            {
                bool groupOut = false;
                if (active == "on")
                    groupOut = true;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Could not add group, " + ex.Message;
                List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
                return View("Index", coreInfo);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Remove a given visitor
        /// </summary>
        /// <remarks>
        /// Update the log table where this visitor is concerned, and set their active status to false. They may wish to visit again.
        /// </remarks>
        /// <param name="visitorUserID">The visitor to remove.</param>
        /// <returns></returns>
        public JsonResult RemoveVisitor(string visitorUserID)
        {
            try
            {
                if (!string.IsNullOrEmpty(visitorUserID))
                {
                    int actualVisitorUserID = Convert.ToInt32(visitorUserID);

                    // Flag the user/visitor as deleted.
                    UserUtilities.FlagUserDeleted(actualVisitorUserID);
                    // Update the visitor log
                    VisitorLogUtilities.LogVisitorOut(actualVisitorUserID);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.DenyGet);
            }

            return Json("request-ok", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get a dictionary of all general or admin users and their respective IDs.
        /// </summary>
        /// <returns>Json dictionary of users and IDs.</returns>
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
        /// Get an ordered dictionary of all general or admin users and their respective IDs.
        /// </summary>
        /// <returns>Json dictionary of users and IDs.</returns>
        public JsonResult GetGeneralAndAdminUsersOrdered()
        {
            // This dictionary will hold userID and corresponding name
            OrderedDictionary usersDict = new OrderedDictionary();

            List<User> rawUsers = UserUtilities.GetGeneralAndAdminUsers();

            foreach (User u in rawUsers)
            {
                usersDict.Add(u.UserID.ToString(), u.FirstName + " " + u.LastName);
            }

            return Json(usersDict, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get members of a particular group.
        /// </summary>
        /// <param name="groupID">The groupID to seach on.</param>
        /// <returns>List of users.</returns>
        public JsonResult GetMembersOfGroup(string groupID)
        {
            List<UserInfo> userInfos = new List<UserInfo>();
            List<User> rawUsers = new List<User>();

            if (!string.IsNullOrEmpty(groupID))
            {
                int actualGroupID = Convert.ToInt32(groupID);

                // Get all users of this group
                rawUsers = GroupUtilities.GetMembersOfGroup(actualGroupID);
            }

            // Build list of serialisible user info objects.
            foreach (User u in rawUsers)
            {
                userInfos.Add(new UserInfo{ userID = u.UserID.ToString(), 
                    firstName = u.FirstName, 
                    lastName = u.LastName, 
                    isActive = u.IsActive.ToString()});
            }

            IEnumerable<UserInfo> data = userInfos;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get a dictionary of all visitor type users and thier respective IDs
        /// </summary>
        /// <returns>Json dictionary of users and their IDs.</returns>
        public JsonResult GetVisitorUsers()
        {
            // This dictionary will hold userID and corresponding name
            Dictionary<string, string> usersDict = new Dictionary<string, string>();

            List<User> rawUsers = UserUtilities.GetVisitorUsers();

            // For each user, pull up their last visitor log instance, so we can provide 
            // some distinguishing information in the UI.
            foreach (User u in rawUsers)
            {
                // Get this user's last visit.
                VisitorLog lastVl = VisitorLogUtilities.GetLastLogForVisitor(u.UserID);
                // Append some useful information in the dict.
                usersDict.Add(u.UserID.ToString(), u.FirstName + " " + u.LastName + " (" + lastVl.Company + ")");
            }

            return Json(usersDict, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get a list of all Groups stored in the db.
        /// </summary>
        /// <returns>List of groups</returns>
        public JsonResult GetAllGroups()
        {
            List<GroupInfo> groupsSerialized = new List<GroupInfo>();

            // Get list of all our groups:
            List<Group> groups = GroupUtilities.GetAllGroups();

            // Build list of serialized objects
            foreach (Group g in groups)
            {
                groupsSerialized.Add(new GroupInfo {
                     GroupID = g.GroupID.ToString(),
                     Active = g.Active.ToString(),
                     Name = g.Name,
                     Priority = g.Priority.ToString()
                });
            }

            IEnumerable<GroupInfo> data = groupsSerialized;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Regardless of other circumstances, set the status to the default status 'in office'
        /// </summary>
        /// <param name="userID">The username to apply this change to</param>
        /// <returns>Indicates success or failure.</returns>
        public JsonResult SetStatusIn(string userID)
        {
            // If a userID was provided
            if (!string.IsNullOrEmpty(userID))
            {
                // Convert to a usable type
                int actualUserID = Convert.ToInt32(userID);

                try
                {
                    // Perform the update
                    CoreUtilities.UpdateStatusIn(actualUserID);

                    // Pull out the new core instance given to this user.
                    // Populate a data response with the current information for this user.
                    Core c = CoreUtilities.GetCoreInstanceByUserID(actualUserID);
                    CoreInfo data = new CoreInfo
                    {
                        userID = c.UserID.ToString(),
                        statusID = c.StatusID.ToString(),
                        locationID = c.LocationID.ToString(),
                        status = c.Status.Name,
                        location = c.Location.Name
                    };

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("request-failed", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("no userID", JsonRequestBehavior.DenyGet);
            }
        }

        /// <summary>
        /// Regardless of other circumstances, set the status to the default out status.
        /// </summary>
        /// <param name="userID">The username to apply this change to.</param>
        /// <returns>Indicates success or failure.</returns>
        public JsonResult SetStatusOut(string userID)
        {
            // If there was a userID provided.
            if (!string.IsNullOrEmpty(userID))
            {
                // Convert to a usable type
                int actualUserID = Convert.ToInt32(userID);

                try
                {
                    // Perform the update
                    CoreUtilities.UpdateStatusOut(actualUserID);

                    // Pull out the new core instance given to this user.
                    // Populate a data response with the current information for this user.
                    Core c = CoreUtilities.GetCoreInstanceByUserID(actualUserID);
                    CoreInfo data = new CoreInfo
                    {
                        userID = c.UserID.ToString(),
                        statusID = c.StatusID.ToString(),
                        locationID = c.LocationID.ToString(),
                        status = c.Status.Name,
                        location = c.Location.Name
                    };

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("request-failed", JsonRequestBehavior.AllowGet);
                }    
            }
            else
            {
                return Json("no userID", JsonRequestBehavior.DenyGet);
            }
            
        }

        /// <summary>
        /// Get all statuses available to a specific username
        /// </summary>
        /// <param name="userID">The username to search on</param>
        /// <returns>JSON containing the relavent statuses</returns>
        public JsonResult GetStatusesAjax(string userID)
        {
            Dictionary<string, string> statusDict = new Dictionary<string, string>();

            try
            {
                int thisUserID = Convert.ToInt32(userID);

                // Get the list of statuses available for this username.
                List<Status> statuses = StatusUtilities.GetAvailableStatuses(thisUserID);
                // Pull out the actual status names for the string list
                foreach (var status in statuses)
                    statusDict.Add(status.StatusID.ToString(), status.Name);
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Json(statusDict, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all the Locations available to a specific username
        /// </summary>
        /// <param name="userID">The username to search on.</param>
        /// <returns>Json containing the relavent locations</returns>
        public JsonResult GetLocationsAjax(string userID)
        {
            Dictionary<string, string> locationDict = new Dictionary<string, string>();

            try
            {
                int thisUserID = Convert.ToInt32(userID);

                // Get the list of Locations available to this username
                List<Location> locations = LocationUtilities.GetAvailableLocations(thisUserID);

                // Pull out the actual location names for the string list
                foreach (Location loc in locations)
                    locationDict.Add(loc.LocationID.ToString(), loc.Name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Json(locationDict, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns the current system time. 
        /// </summary>
        /// <remarks>
        /// This is requested from the server (rather than clientside) in order to achieve consistency.
        /// </remarks>
        /// <returns>Json string object with time details.</returns>
        public JsonResult GetSystemTime()
        {
            DateTime now = DateTime.Now;

            TimeInfo data = new TimeInfo
            {
                year = now.Year.ToString(),
                month = now.Month.ToString(),
                day = now.Day.ToString(),
                hour = now.Hour.ToString(),
                minute = now.Minute.ToString(),
                second = now.Second.ToString()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        } 
       
        /// <summary>
        /// Return a list of TimeInfo object, in half hour intervals, from 6am until 12am midnight.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllDayTimes(string date)
        {
            List<TimeInfo> timeIntervals = new List<TimeInfo>();

            DateTime startDay = DateTime.Parse(date);

            DateTime start = new DateTime(startDay.Year, startDay.Month, startDay.Day, Constants.START_OF_DAY, 0, 0);
            DateTime end = start.AddHours(Constants.WORKDAY_DURATION);

            while (start <= end)
            {
                TimeInfo ti = new TimeInfo();

                // Create a new TimeInfo object and add it to the list
                ti = new TimeInfo
                {
                    numeric_repr = start.ToShortTimeString(),
                    dateString = start.ToString(),
                    year = start.Year.ToString(),
                    month = start.Month.ToString(),
                    day = start.Day.ToString(),
                    hour = start.Hour.ToString(),
                    minute = start.Minute.ToString(),
                    second = start.Second.ToString()
                };

                timeIntervals.Add(ti);

                // Increment the current time by 30 minutes.
                start = start.AddMinutes(30);
            }

            // Convert our list to something that can be Json-ified
            IEnumerable<TimeInfo> data = timeIntervals;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Return a list of TimeInfo objects, in half hour intervals, from start until 12am midnight.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRemainderOfToday()
        {
            List<TimeInfo> timeIntervals = new List<TimeInfo>();

            DateTime start = DateTimeUtilities.RoundUp(DateTime.Now, TimeSpan.FromMinutes(30));
            int remainingHours = Constants.END_OF_DAY - start.Hour;
            DateTime end = start.AddHours(remainingHours);

            while(start <= end)
            {
                TimeInfo ti = new TimeInfo();

                // Create a new TimeInfo object and add it to the list
                ti = new TimeInfo
                {
                    numeric_repr = start.ToShortTimeString(),
                    dateString = start.ToString(),
                    year = start.Year.ToString(),
                    month = start.Month.ToString(),
                    day = start.Day.ToString(),
                    hour = start.Hour.ToString(),
                    minute = start.Minute.ToString(),
                    second = start.Second.ToString()
                };
                
                timeIntervals.Add(ti);

                // Increment the current time by 30 minutes.
                start = start.AddMinutes(30);
            }

            // Convert our list to something that can be Json-ified
            IEnumerable<TimeInfo> data = timeIntervals;

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}