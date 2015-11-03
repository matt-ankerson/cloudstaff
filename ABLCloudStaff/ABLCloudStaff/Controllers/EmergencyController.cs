using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABLCloudStaff.Models;
using ABLCloudStaff.Biz_Logic;

namespace ABLCloudStaff.Controllers
{
    public class EmergencyController : Controller
    {
        /// <summary>
        /// Get all users who're currently considered 'in' the workplace.
        /// </summary>
        /// <returns>Emergency view</returns>
        public ActionResult Emergency()
        {
            List<Core> usersInOffice = CoreUtilities.GetAvailableCoresForEmergency();

            return View(usersInOffice);
        }
	}
}