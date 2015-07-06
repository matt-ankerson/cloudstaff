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

            List<Core> coreInfo = new List<Core>();

            using (var db = new ABLCloudStaffContext())
            {
                try
                {
                    // Eagerly pull all the info we will need
                    coreInfo = db.Cores.Include("User").Include("Status").ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }               
            }

            return View(coreInfo);
        }

        /// <summary>
        /// Get all statuses available to a specific user
        /// </summary>
        /// <param name="userID">The user to search on</param>
        /// <returns>JSON containing the relavent statuses</returns>
        public JsonResult GetStatusesAjax(string userID)
        {
            List<string> statusNames = new List<string>();

            try
            {
                int thisUserID = Convert.ToInt32(userID);

                // Get the list of statuses available for this user.
                List<Status> statuses = StatusUtilities.GetAvailableStatuses(thisUserID);
                // Pull out the actual status names for the string list
                foreach (var status in statuses)
                    statusNames.Add(status.Name);
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Json(statusNames, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all the Locations available to a specific user
        /// </summary>
        /// <param name="userID">The user to search on.</param>
        /// <returns>Json containing the relavent locations</returns>
        public JsonResult GetLocationsAjax(string userID)
        {
            List<string> locationNames = new List<string>();

            try
            {
                int thisUserID = Convert.ToInt32(userID);

                // Get the list of Locations available to this user
                List<Location> locations = LocationUtilities.GetAvailableLocations(thisUserID);

                // Pull out the actual location names for the string list
                foreach (Location loc in locations)
                    locationNames.Add(loc.Name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Json(locationNames, JsonRequestBehavior.AllowGet);
        }







        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}