using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class NewController : ControllerBase
    {
        public ActionResult Index(int max = 0)
        {
            if (max == 0)
                return View();

            var list = new List<mp.Models.WaterfallItem>();

            Manager.Images.Items
                .Where(i => i.State == DAL.ImageStates.Ready && i.ID < max)
                .OrderByDescending(i => i.ID)
                .Take(20)
                .ToList()
                .ForEach(i =>
                {
                    list.Add(new Models.WaterfallItem { ID = i.ID, Item = new mp.Models.ImageInfo(i) });
                });

            return PartialView("ImageListFw236.pc", list);
        }
    }
}
