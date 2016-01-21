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
            routes.MapRoute(name: "package-images", url: "package/{packageId}/images/{max}/{thumb}", defaults: new { controller = "package", action = "Images", max = 0 ,thumb="fw236"});
            routes.MapRoute(name: "package", url: "package/{id}/{action}", defaults: new { controller = "package", action = "Index" }, constraints: new { id=@"\d+"});
            //图片页
            routes.MapRoute(name: "image", url: "image/{id}/{action}", defaults: new { controller = "image", action = "index" }, constraints: new { id = @"\d+" });
            //用户页
            routes.MapRoute(name: "user", url: "user/{userId}/{subPage}/{max}", defaults: new { controller = "user", action = "index", max = 0 });
            routes.MapRoute(name: "user-default", url: "user/{userId}/{action}", defaults: new { controller = "user", action = "index" });
            //搜索页
            routes.MapRoute(name: "search", url: "search/{kw}/{max}", defaults: new { controller = "search",action = "index", kw = "", max = 0 });

            //默认路由
            routes.MapRoute(name: "default", url: "{controller}/{action}", defaults: new { controller = "home", action = "index" });
        }
    }
}