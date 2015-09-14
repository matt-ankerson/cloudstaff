using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
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

                    // Perform the update. ReturnTime is handled as an optional field. 
                    CoreUtilities.UpdateStatus(actualUserID, actualStatusID, returnTime);
                    CoreUtilities.UpdateLocation(actualUserID, actualLocationID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
            //return View("Index", coreInfo);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Regardless of other circumstances, set the status to the default status 'in office'
        /// </summary>
        /// <param name="userID">The username to apply this change to</param>
        /// <returns>Indicates success or failure.</returns>
        public JsonResult SetStatusIn(string userID)
        {
            string data = null;

            // If a userID was provided
            if(!string.IsNullOrEmpty(userID))
            {
                // Convert to a usable type
                int actualUserID = Convert.ToInt32(userID);

                try
                {
                    // Perform the update
                    CoreUtilities.UpdateStatusIn(actualUserID);

                    // Pull out the new status given to this user
                    Status s = CoreUtilities.GetStatusByUserID(actualUserID);
                    data = s.Name;
                }
                catch (Exception ex)
                {
                    data = "request-failed";
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Regardless of other circumstances, set the status to the default out status.
        /// </summary>
        /// <param name="userID">The username to apply this change to.</param>
        /// <returns>Indicates success or failure.</returns>
        public JsonResult SetStatusOut(string userID)
        {
            string data = null;

            // If there was a userID provided.
            if (!string.IsNullOrEmpty(userID))
            {
                // Convert to a usable type
                int actualUserID = Convert.ToInt32(userID);

                try
                {
                    // Perform the update
                    CoreUtilities.UpdateStatusOut(actualUserID);

                    // Pull out the new status given to this user
                    Status s = CoreUtilities.GetStatusByUserID(actualUserID);
                    data = s.Name;
                }
                catch (Exception ex)
                {
                    data = "request-failed";
                }    
            }

            return Json(data, JsonRequestBehavior.AllowGet);
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
        /// Return a list of TimeInfo objects, in half hour intervals, from now until 12am midnight.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRemainderOfToday()
        {
            List<TimeInfo> timeIntervals = new List<TimeInfo>();

            DateTime now = DateTimeUtilities.RoundUp(DateTime.Now, TimeSpan.FromMinutes(30));
            DateTime thisMidnight = DateTime.Now.AddDays(1).Date;

            while(now < thisMidnight)
            {
                TimeInfo ti = new TimeInfo();

                // Create a new TimeInfo object and add it to the list
                ti = new TimeInfo
                {
                    numeric_repr = now.ToShortTimeString(),
                    dateString = now.ToString(),
                    year = now.Year.ToString(),
                    month = now.Month.ToString(),
                    day = now.Day.ToString(),
                    hour = now.Hour.ToString(),
                    minute = now.Minute.ToString(),
                    second = now.Second.ToString()
                };
                
                timeIntervals.Add(ti);

                // Increment the current time by 30 minutes.
                now = now.AddMinutes(30);
            }

            // Convert our list to something that can be Json-ified
            IEnumerable<TimeInfo> data = timeIntervals;

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}