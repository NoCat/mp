using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.DAL;
using mp.BLL;

namespace mp.Controllers
{
    public class PickController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int packageId = 0, string description = "", string source = "", string from = "")
        {
            description = description.Trim();
            source = source.Trim();
            from = from.Trim();

            var result = new AjaxResult();
            //判断是否已经登录
            if (Security.IsLogin == false)
            {
                result.Success = false;
                result.Message = "请先登录";
                return Json(result);
            }
            //判断图包是否存在
            var package = DB.Packages.Find(packageId);
            if (package == null)
            {
                result.Success = false;
                result.Message = "图包不存在";
                return Json(result);
            }
            //判断是否有添加图片的权限
            if (package.UserID != Security.User.ID)
            {
                result.Success = false;
                result.Message = "无操作权限";
                return Json(result);
            }

            var manager = new PickManager(DB);
            manager.Add(from, source, packageId, description);

            return Json(result);
        }
    }
}
