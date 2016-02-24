using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.BLL;
using mp.Models;

namespace mp.Controllers
{
    public class NewController : ControllerBase
    {
        //
        // GET: /New/

        public ActionResult Index()
        {
            var imageList = new List<ImageInfo>();

            Manager.Images.Items.Where(i => i.State == DAL.ImageStates.Ready).Take(40).OrderByDescending(i=>i.FileID).ToList().ForEach(i =>
            {
                imageList.Add(new ImageInfo(i));
            });

            ViewBag.ImageList = imageList;

            return View("index");
        }

        [HttpPost]
        public ActionResult GetNew()
        {
            var ajaxResult = new AjaxResult();
            var imageList = new List<ImageInfo>();

            return;
        }

    }
}
