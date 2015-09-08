using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABLCloudStaff.Controllers
{
    public class LoginAdminController : Controller
    {
        /// <summary>
        /// Pull up the login view for the administration screen.
        /// </summary>
        /// <returns>ActionResult object</returns>
        public ActionResult LoginAdmin()
        {
            return View();
        }
	}
}