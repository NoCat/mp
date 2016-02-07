using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.DAL;
using mp.Admin.Models;

namespace mp.Admin.Controllers
{
    public class TagController : ControllerBase
    {
        //
        // GET: /Tag/

        public ActionResult Index(int page=1)
        {
            var taglist = Manager.AdminPixivTags.Items.Where(i => (i.ID >= 40 * (page - 1) && i.ID < 40 * page)).ToList();
            return View(taglist);
        }

        public ActionResult Edit(int id,string mtext)
        {
            var result = new AjaxResult();
            var tag = Manager.AdminPixivTags.Find(id);
            tag.MText = mtext;
            try
            {
                Manager.AdminPixivTags.Update(tag);
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
                throw;
            }

            return JsonContent(result);
        }

        public ActionResult Add(string ptext="",string mtext="")
        {
            var result = new AjaxResult();
            ptext = ptext.Trim();
            if(ptext.Length==0)
            {
                result.Success = false;
                result.Message = "ptext不能为空";
                return JsonContent(result);
            }

            mtext = mtext.Trim();
            if (mtext.Length == 0)
            {
                result.Success = false;
                result.Message = "mtext不能为空";
                return JsonContent(result);
            }

            var tag = Manager.AdminPixivTags.Items.Where(t => t.PText == ptext).FirstOrDefault();
            if(tag!=null)
            {
                result.Success = false;
                result.Message = "ptext已经存在";
                return JsonContent(result);
            }

            tag = new AdminPixivTag { PText = ptext, MText = mtext };
            Manager.AdminPixivTags.Add(tag);

            result.Success = true;
            result.Message = "插入成功";
            return JsonContent(result);
        }
    }
}
