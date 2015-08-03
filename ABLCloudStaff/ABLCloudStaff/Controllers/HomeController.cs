using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ABLCloudStaff.Models;
using ABLCloudStaff.Board_Logic;

namespace ABLCloudStaff.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Get state information on each user, push to the core/home view
        /// </summary>
        /// <returns>An ActionResult object</returns>
        public ActionResult Index()
        {
            List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
            return View(coreInfo);
            //List<Core> coreInfo = new List<Core> { new Core {User = new User {FirstName = "Mike", LastName = "Hoskins", IsActive = true, UserID = 1, UserTypeID = 1, UserType = new UserType {Type = "General"}
            //}, CoreID = 1, LocationID = 1, Location = new Location {LocationID = 1, Name = "AbacusBio"}, Status = new Status {StatusID = 1, Name = "In Office"}, StatusID = 1,
            //UserID = 1, StateStart = DateTime.Now}  };
            //return View(coreInfo);
        }

        /// <summary>
        /// Push a status and location to the database for a specific user
        /// </summary>
        /// <returns>An ActionResult object</returns>
        [HttpPost]
        public ActionResult SubmitStatusOrLocationUpdate(string userID, string statusID, string locationID)
        {
            try
            {
                if (userID != null)
                {
                    // Convert our parameters into a useful data type
                    int actualUserID = Convert.ToInt32(userID);
                    int actualStatusID = Convert.ToInt32(statusID);
                    int actualLocationID = Convert.ToInt32(locationID);

                    // Perform the update
                    CoreUtilities.UpdateStatus(actualUserID, actualStatusID);
                    CoreUtilities.UpdateLocation(actualUserID, actualLocationID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
            return View("Index", coreInfo);
        }

        /// <summary>
        /// Get all statuses available to a specific user
        /// </summary>
        /// <param name="userID">The user to search on</param>
        /// <returns>JSON containing the relavent statuses</returns>
        public JsonResult GetStatusesAjax(string userID)
        {
            Dictionary<string, string> statusDict = new Dictionary<string, string>();

            try
            {
                int thisUserID = Convert.ToInt32(userID);

                // Get the list of statuses available for this user.
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
        /// Get all the Locations available to a specific user
        /// </summary>
        /// <param name="userID">The user to search on.</param>
        /// <returns>Json containing the relavent locations</returns>
        public JsonResult GetLocationsAjax(string userID)
        {
            Dictionary<string, string> locationDict = new Dictionary<string, string>();

            try
            {
                int thisUserID = Convert.ToInt32(userID);

                // Get the list of Locations available to this user
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
        /// Return a list of TimeInfo objects, in half hour intervals, from now until 12am midnight.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRemainderOfToday()
        {
            List<TimeInfo> timeIntervals = new List<TimeInfo>();

            DateTime now = DateTimeUtilities.RoundUp(DateTime.Now, TimeSpan.FromMinutes(30));
            DateTime thisMidnight = DateTime.Now.AddDays(1).Date;

            while(now <= thisMidnight)
            {
                TimeInfo ti = new TimeInfo();

                // Create a new TimeInfo object and add it to the list
                if(now.Minute == 0)
                {
                    ti = new TimeInfo
                    {
                        numeric_repr = now.Hour.ToString() + "00",  // 24hr time
                        year = now.Year.ToString(),
                        month = now.Month.ToString(),
                        day = now.Day.ToString(),
                        hour = now.Hour.ToString(),
                        minute = now.Minute.ToString(),
                        second = now.Second.ToString()
                    };
                }
                else
                {
                    ti = new TimeInfo
                    {
                        numeric_repr = (now.Hour.ToString() + now.Minute.ToString()),
                        year = now.Year.ToString(),
                        month = now.Month.ToString(),
                        day = now.Day.ToString(),
                        hour = now.Hour.ToString(),
                        minute = now.Minute.ToString(),
                        second = now.Second.ToString()
                    };
                }
                
                timeIntervals.Add(ti);

                // Increment the current time by 30 minutes.
                now = now.AddMinutes(30);
            }

            // Convert our list to something that can be Json-ified
            IEnumerable<TimeInfo> data = timeIntervals;

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }

    /// <summary>
    /// Object to hold a timestamp, transportable via json.
    /// </summary>
    public class TimeInfo
    {
        public string numeric_repr;
        public string year;
        public string month;
        public string day;
        public string hour;
        public string minute;
        public string second;
    }
}