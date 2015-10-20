using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Admin.Models;
using System.IO;
using mp.BLL;

namespace mp.Admin.Controllers
{
    public class UploadController : ControllerBase
    {
        public ActionResult Index(string name, int chunk, int chunks,HttpPostedFileBase data)
        {
            var result = new AjaxResult();
            var path = Server.MapPath("~/temp/");
            using (var fs = System.IO.File.Create(path + name + "_" + chunk))
            {
                fs.Write(data.InputStream);
            }

            if (chunk == chunks - 1)
            {
                var mergePath = path + name;
                using (var fs = System.IO.File.Create(mergePath))
                {
                    for (int i = 0; i < chunks; i++)
                    {
                        var chunkPath = path + name + "_" + i;
                        using (var cf = System.IO.File.OpenRead(chunkPath))
                        {
                            fs.Write(cf);
                        }
                        System.IO.File.Delete(chunkPath);
                    }

                    var manager = new FileManager(DB);
                    var file= manager.Create(fs);
                    result.Data = new { id = file.ID };
                }

                System.IO.File.Delete(mergePath);
            }

            return Json(result);
        }
    }
}
