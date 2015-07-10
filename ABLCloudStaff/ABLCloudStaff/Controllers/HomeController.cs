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
    }
}