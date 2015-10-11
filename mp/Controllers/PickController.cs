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

            DAL.File file = null;
            //检查是否已经下载过该图片
            {
                var sourceUrl = DB.CreateIfNotExist<Url>((u => u.CRC32 == source.CRC32() && u.Url == source), new Url { Url = source, CRC32 = source.CRC32() });
                var fromUrl = DB.CreateIfNotExist<Url>((u => u.CRC32 == from.CRC32() && u.Url == from), new Url { Url = from, CRC32 = from.CRC32() });

                var download = DB.CreateIfNotExist<Download>(d => d.SourceUrlID == sourceUrl.ID, new Download { FromUrlID = fromUrl.ID, SourceUrlID = sourceUrl.ID });
                //判断是否下载过了
                if (download.FileID != 0)
                {
                    //已经下载过,直接添加图片
                    new BLL.ImageManager(DB).Insert(package, new Image
                    {
                        Description = description,
                        FileID = download.FileID,
                        PackageID = package.ID,
                        UserID = Security.User.ID,
                        FromUrlID = fromUrl.ID
                    });
                }
                else
                {
                    //未下载过的,添加到pick任务
                    var pick = new Pick
                    {
                        DownloadID = download.ID,
                        Description = description,
                        FromUrlID = fromUrl.ID,
                        PackageID = packageId,
                        UserID = Security.User.ID
                    };
                }
            }

            return Json(result);
        }
    }
}
