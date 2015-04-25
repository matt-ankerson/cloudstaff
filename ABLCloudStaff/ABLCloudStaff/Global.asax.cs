using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using ABLCloudStaff.Models;

namespace ABLCloudStaff
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Initialise the database (for testing)
            Database.SetInitializer(new DropCreateABLCloudStaffAlways());
        }
    }
}
