using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ABLCloudStaff.Models;

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