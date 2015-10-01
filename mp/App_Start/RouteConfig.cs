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
            //主页
            routes.MapRoute(name: "Home", url: "", defaults: new { controller = "home", action = "index" });
            //图包页
            routes.MapRoute(name: "package-images", url: "package/{packageId}/images/{max}", defaults: new { controller = "package", action = "Images",max=0 });
            routes.MapRoute(name: "package", url: "package/{packageId}", defaults: new { controller = "package", action = "Index" });
            //用户页
            routes.MapRoute(name: "user-default", url: "user/{userId}", defaults: new { controller = "user", action = "index" });
            routes.MapRoute(name: "user", url: "user/{userId}/{subPage}/{max}", defaults: new { controller = "user", action = "index",max=0 });
        }
    }
}