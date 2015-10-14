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

        public ActionResult Index(string kw,int max=0)
        {            
            if (max == 0)
            {
                ViewBag.Keyword = kw;
                return View("index.pc");
            }
            else
            {
                var list = new List<ImageInfo>();
                if(string.IsNullOrWhiteSpace(kw)==false|| kw.Length>2)
                {
                    Service.Images.Items.Where(i => i.Description.Contains(kw) && i.ID < max).OrderByDescending(i => i.ID).Take(20).ToList()
                        .ForEach(i => list.Add(new ImageInfo(i)));
                }
                return PartialView("ImageListFw236.pc",list);
            }
        }

    }
}
