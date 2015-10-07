using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace mp
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/mp.js").Include(
                "~/js/jquery.js",
                "~/js/bootstrap.js",
                "~/js/client.js",
                "~/js/home.js",
                "~/js/masonry.pkgd.min.js",
                "~/js/modal.js"
                ));

            bundles.Add(new StyleBundle("~/css/mp.css").Include(
                "~/css/bootstrap.css",
                "~/css/footer.css",
                "~/css/head-panel.css",
                "~/css/header.css",
                "~/css/tools.css",
                "~/css/waterfall-item-236.css"
                ));
        }
    }
}