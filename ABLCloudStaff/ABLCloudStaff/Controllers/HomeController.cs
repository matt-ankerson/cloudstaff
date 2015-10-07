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
                    }
                    else
                    {
                        // Use the existing one.
                        visitorUserID = Convert.ToInt32(existingVisitorUserID);
                        // We also need to add a core instance again for this user.
                        CoreUtilities.AddCore(visitorUserID, Constants.DEFAULT_IN_STATUS, Constants.DEFAULT_LOCATION);
                    }

                    // If we've now got a visitor user and a visited user:
                    if ((visitorUserID != 0) && (actualUserBeingVisitedID != 0))
                    {
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
        /// Remove a given visitor
        /// </summary>
        /// <remarks>
        /// Update the log table where this visitor is concerned, and set their active status to false. They may wish to visit again.
        /// </remarks>
        /// <param name="visitorUserID">The visitor to remove.</param>
        /// <returns></returns>
        public ActionResult RemoveVisitor(string visitorUserID)
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
                ViewBag.Message = "Could not remove visitor, " + ex.Message;
                List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();
                return View("Index", coreInfo);
            }

            return RedirectToAction("Index", "Home");
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