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
        //
        // GET: /Upload/

        [HttpPost]
        public ActionResult Index(string filename, int chunk, int chunks)
        {
            var result = new AjaxResult();
            using (var fs = System.IO.File.Create(string.Format("~/temp/{0)_{1}", filename, chunk).MapPath()))
            {
                fs.Write(Request.Files["file"].InputStream);
            }

            if (chunk == chunks - 1)
            {
                var mergePath = string.Format("~/temp/{0}", filename).MapPath();
                using (var fs = System.IO.File.Create(mergePath))
                {
                    for (int i = 0; i < chunks; i++)
                    {
                        var chunkPath = string.Format("~/temp/{0)_{1}", filename, i).MapPath();
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
