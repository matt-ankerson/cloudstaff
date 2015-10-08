using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABLCloudStaff.Models;
using ABLCloudStaff.Biz_Logic;

namespace ABLCloudStaff.Controllers
{
    public class DisplaySmallController : Controller
    {
        /// <summary>
        /// Get all core instances, push to the display small view.
        /// </summary>
        /// <returns>Action result object</returns>
        public ActionResult DisplaySmall()
        {
            List<Core> coreInfo = CoreUtilities.GetAllCoreInstances();

            return View(coreInfo);
        }
	}
}