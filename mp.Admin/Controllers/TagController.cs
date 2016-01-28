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

        public ActionResult Index(string keyword)
        {            
            IQueryable<AdminPixivTag> taglist = Manager.AdminPixivTags.Items;
            if(!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                taglist = taglist.Where(t => t.PText.StartsWith(keyword));
            }
            return View(taglist.Take(20).ToList());
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
            
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result),"application/json");            
        }

        public ActionResult Query(string keyword)
        {
            return View();
        }
    }
}
