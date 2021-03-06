﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using ABLCloudStaff.Models;

namespace ABLCloudStaff
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Default stuff
            AreaRegistration.RegisterAllAreas();

            // Manually installed WebApi 2.2 after making an MVC project.
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Default stuff
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Initialise the database
#if DEBUG
            Database.SetInitializer(new DropCreateABLCloudStaffAlways());
#else
            Database.SetInitializer(new CreateABLCloudStaffIfNotExists());
#endif
        }
    }
}
