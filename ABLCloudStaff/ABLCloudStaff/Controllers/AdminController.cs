using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            UserUtilities.AddUser(firstName, lastName);

            return View("Admin");
        }
	}
}