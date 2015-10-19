using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Admin.Models;
using PagedList;
using mp.DAL;
using Newtonsoft.Json.Linq;

namespace mp.Admin.Controllers
{
    public class SubAccountController : ControllerBase
    {
        //
        // GET: /SubAccount/

        public ActionResult Index(int id, int page = 1, int size = 20)
        {
            var list = DB.Packages.Where(p => p.UserID == id).Select(p => new PackageInfo(p)).ToPagedList(page, size);
            ViewBag.List = list;
            return View();
        }

        public ActionResult PackageCreate(int id, string title, string description)
        {
            var package = new Package { UserID = id, Title = title, Description = description };
            DB.PackageInsert(package);
            return View();
        }

        public ActionResult PackageAddImages(int id, int packageId, string values)
        {
            foreach (JObject item in JArray.Parse(values))
            {
                var fileid = (int)item["id"];
                var description = (string)item["description"];

                DB.ImageInsert(new Image { FileID = fileid, PackageID = packageId, UserID = id, Description = description });
            }

            return View();
        }

        public ActionResult PackageEdit(int id)
        {
            return View();
        }
    }
}
