using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.DAL;

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
            var package = Service.Packages.Items.Where(p=>p.ID==packageId).FirstOrDefault();
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

            var sourceUrl = Service.Urls.CreateIfNotExist(new Url { Text = source, CRC32 = source.CRC32() }, (u => u.CRC32 == source.CRC32() && u.Text == source));
            var fromUrl = Service.Urls.CreateIfNotExist(new Url { Text = from, CRC32 = from.CRC32() }, (u => u.CRC32 == from.CRC32() && u.Text == from));

            var pick = new Pick { Description=description, FromUrlID=fromUrl.ID,SourceUrlID=sourceUrl.ID, PackageID=packageId };
            Service.Picks.Insert(pick);

            return Json(result);
        }
    }
}
