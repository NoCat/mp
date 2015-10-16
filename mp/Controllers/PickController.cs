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

            var sourceUrl = DB.UrlCreateIfNotExist(new Url { Text = source, CRC32 = source.CRC32() }, (u => u.CRC32 == source.CRC32() && u.Text == source));
            var fromUrl = DB.UrlCreateIfNotExist(new Url { Text = from, CRC32 = from.CRC32() }, (u => u.CRC32 == from.CRC32() && u.Text == from));

            //根据sourceUrl查找下载列表
            var download = DB.DownloadCreateIfNotExist(new Download { FromUrlID = fromUrl.ID, SourceUrlID = sourceUrl.ID }, d => d.SourceUrlID == sourceUrl.ID);
            //判断文件是否已经下载过
            if(download.FileID!=0)
            {
                //文件已经下载过，直接添加到图包
                DB.ImageInsert(new Image { Description = description, FileID = download.FileID, PackageID = package.ID, UserID = package.UserID, FromUrlID = fromUrl.ID });
            }
            else
            {
                //文件未下载过，添加pick任务
                DB.PickInsert(new Pick { DownloadID = download.ID, FromUrlID = fromUrl.ID, PackageID = package.ID, UserID = package.UserID, Description = description });
            }

            return Json(result);
        }
    }
}
