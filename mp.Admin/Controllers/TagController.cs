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

        public ActionResult Index(string keyword, string orderby = "weight", string filter = "no-edit")
        {
            ViewBag.Keyword = keyword;
            ViewBag.OrderBy = orderby;
            ViewBag.Filter = filter;

            IQueryable<AdminPixivTag> taglist = Manager.AdminPixivTags.Items;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                taglist = taglist.Where(t => t.PText.Contains(keyword));
            }

            switch (filter)
            {
                case "no-edit":
                    taglist = taglist.Where(t => t.MText == null && t.IsSkip == false);
                    break;
                case "edited":
                    taglist = taglist.Where(t => t.MText != null);
                    break;
                case "skip":
                    taglist = taglist.Where(t => t.IsSkip == true);
                    break;
            }

            switch (orderby)
            {
                case "weight":
                    taglist = taglist.OrderByDescending(t => t.Weight);
                    break;
                case "count":
                    taglist = taglist.OrderByDescending(t => t.CitationCount);
                    break;
                default:
                    taglist = taglist.OrderByDescending(t => t.ID);
                    break;
            }

            return View(taglist.Take(20).ToList());
        }

        public ActionResult Edit(int id)
        {
            var model = Manager.AdminPixivTags.Find(id);
            if (model == null)
                model = new AdminPixivTag();

            ViewBag.ModalType = "edit";

            return PartialView("modal", model);
        }
        [HttpPost]
        public ActionResult Edit(int id, string mtext, bool skip = false)
        {
            var result = new AjaxResult();
            var tag = Manager.AdminPixivTags.Find(id);
            if (string.IsNullOrWhiteSpace(mtext) == false)
                mtext = mtext.Trim();
            else
                mtext = null;

            tag.MText = mtext;
            tag.IsSkip = skip;

            Manager.AdminPixivTags.Update(tag);

            if (tag.IsSkip == false)
            {
                var works = Manager.AdminPixivWorkTags.Items.Where(wt => wt.TagID == tag.ID).ToList();
                foreach (var item in works)
                {
                    var image = item.Work.Image;
                    if (image != null)
                    {
                        var tags = (from worktag in Manager.AdminPixivWorkTags.Items
                                    join tag1 in Manager.AdminPixivTags.Items.Where(t => t.MText != null) on worktag.TagID equals tag1.ID
                                    where worktag.WorkID == item.WorkID
                                    select tag1.MText).Distinct().ToList();

                        var description = "";
                        foreach (var t in tags)
                        {
                            description += string.Format("#{0}#", t);
                        }
                        if (string.IsNullOrWhiteSpace(item.Work.UserName) == false)
                        {
                            description += string.Format("#{0}#", item.Work.UserName);
                        }

                        description += item.Work.Description;

                        image.Description = description;
                        Manager.Images.Update(image);
                    }
                }
            }


            result.Success = true;
            result.Message = "ok";

            return JsonContent(result);
        }

        public ActionResult Add()
        {
            var model = new AdminPixivTag();
            return PartialView("modal", model);
        }
        [HttpPost]
        public ActionResult Add(string ptext = "", string mtext = "")
        {
            var result = new AjaxResult();
            ptext = ptext.Trim();
            if (ptext.Length == 0)
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
            if (tag != null)
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

        public ActionResult Query(string keyword)
        {
            return View();
        }
    }
}
