using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class ImageController : ControllerBase
    {
        //
        // GET: /Image/

        public ActionResult Index(int imageId=0)
        {
            var image = DB.Images.Find(imageId);
            if (image == null)
                return Redirect("/");

            ViewBag.ImageInfo = new BLL.ImageInfo(image);
            return View("Index.pc");
        }
    }
}
