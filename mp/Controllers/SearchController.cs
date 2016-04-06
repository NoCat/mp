using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Models;

namespace mp.Controllers
{
    public class SearchController : ControllerBase
    {

        public ActionResult Index(string kw="", int max = 0)
        {
            if(kw=="")
            {
                return Redirect("~/");
            }

            if (max == 0)
            {
                ViewBag.Keyword = kw;
                return View("index.pc");
            }
            else
            {
                var list = new List<WaterfallItem>();
                if (string.IsNullOrWhiteSpace(kw) == false || kw.Length > 2)
                {
                    var items = Manager.Images.Items
                        .Where(i => i.Description.Contains(kw) && i.ID < max)
                        .GroupBy(i => i.FileID)
                        .Select(g => g.Max(i => i.ID))
                        .OrderByDescending(i => i)
                        .Take(20);

                    var result = from image in Manager.Images.Items
                                 join id in items on image.ID equals id
                                 select image;

                    result.ToList()
                    .ForEach(i => list.Add(new WaterfallItem { ID = i.ID, Item = new ImageInfo(i) }));
                }
                return PartialView("ImageListFw236.pc", list);
            }
        }

    }
}
