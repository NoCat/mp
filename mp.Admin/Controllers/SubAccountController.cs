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
    [MPAuthorize]
    public class SubAccountController : ControllerBase
    {
        //
        // GET: /SubAccount/

        public ActionResult Index(int id)
        {
            var subAccount = DB.Users.Find(id);
            var result = DB.Packages.Where(p => p.UserID == id).OrderByDescending(p=>p.ID).Take(40);
            var list = new List<PackageInfo>();

            result.ToList().ForEach(p => list.Add(new PackageInfo(p)));
            ViewBag.List = list;
            ViewBag.SubAccount = new UserInfo(subAccount);
            return View();
        }

        public ActionResult PackageCreate(int id, string title, string description)
        {
            var package = new Package { UserID = id, Title = title, Description = description,LastModify=new DateTime(1990,1,1) };
            DB.PackageInsert(package);
            return RedirectToAction("index", new { id = id });
        }

        public ActionResult PackageAddImages(int id, int packageId, string values)
        {
            var result = new AjaxResult();
            foreach (JObject item in JArray.Parse(values))
            {
                var fileid = (int)item["id"];
                var description = (string)item["description"];

                DB.ImageInsert(new Image { FileID = fileid, PackageID = packageId, UserID = id, Description = description });
            }

            return Json(result);
        }

        public ActionResult PackageEdit(int id)
        {
            return View();
        }
    }
}
