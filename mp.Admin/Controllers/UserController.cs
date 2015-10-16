using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using mp.Admin.Models;

namespace mp.Admin.Controllers
{
    //网站普通用户管理
    public class UserController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Find(string email)
        {
            return View();
        }

        public ActionResult Create(string email, string name, string password = "12345678")
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddImages(int userId, int packageId,string json)
        {
            var list=JArray.Parse(json);
            foreach (JObject item in list)
            {
                var fileId=(int)item["id"];
                var description=(string)item["description"];
                DB.ImageInsert(new DAL.Image { UserID = userId, PackageID = packageId, FileID = fileId, Description = description});
            }
            return Json(new AjaxResult());
        }
    }
}
