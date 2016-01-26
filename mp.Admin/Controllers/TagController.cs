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
            
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result),"application/json");            
        }
    }
}
