using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace mp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(name: "Home", url: "", defaults: new { controller = "home", action = "index" });
            routes.MapRoute(name: "package-page", url: "package/{packageId}/page/{max}", defaults: new { controller = "package", action = "page" });
            routes.MapRoute(name: "package", url: "package/{id}", defaults: new { controller = "package", action = "Index" });
        }
    }
}