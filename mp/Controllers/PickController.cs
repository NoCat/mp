using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class PickController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int packageId=0, string description="", string source="", string from="")
        {
            description=description.Trim();
            source=source.Trim();
            from=from.Trim();

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
            if(package==null)
            {
                result.Success = false;
                result.Message = "图包不存在";
                return Json(result);
            }
            //判断是否有添加图片的权限
            if(package.UserID!=Security.User.ID)
            {
                result.Success = false;
                result.Message = "无操作权限";
                return Json(result);
            }

            DAL.File file = null;
            //检查是否已经下载过该图片
            {
                var url = DB.Urls.Where(u => u.CRC32 == source.CRC32() && u.Url == source);
                if(url!=null)
                {

                }
            }

            return Json(result);
        }
    }
}
