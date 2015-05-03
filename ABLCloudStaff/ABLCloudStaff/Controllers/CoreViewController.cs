// Author: Matthew Ankerson
// Date: 30 April 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Controllers
{
    public class CoreViewController : Controller
    {
        /// <summary>
        /// Get state information on each user, push to the core view
        /// </summary>
        /// <returns></returns>
        public ActionResult CoreView()
        {
            using (var db = new ABLCloudStaffContext())
            {
                List<Core> coreInfo = new List<Core>();
                

                try
                {
                    // Eagerly pull all the info we will need
                    coreInfo = db.CoreTable.Include("User").ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                

                return View(coreInfo);
            }

        }
	}
}