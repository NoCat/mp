using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mp.DAL;

namespace mp.BLL
{
    public class AdminPixivTagManager:ManagerBase<AdminPixivTag>
    {
        public AdminPixivTagManager(MiaopassContext db, ManagerCollection collection) : base(db, collection) { }

        public override AdminPixivTag Update(AdminPixivTag entity, bool save = true)
        {
            DB.Update(entity, save);

            if(entity.IsSkip==false)
            {
                var worktags = DB.AdminPixivWorkTags.Where(wt => wt.TagID == entity.ID).ToList();
                foreach (var item in worktags)
                {
                    var image = item.Work.Image;
                    if(image!=null)
                    {
                        var tags = (from worktag in DB.AdminPixivWorkTags
                                    join tag in DB.AdminPixivTags.Where(t => t.MText != null) on worktag.TagID equals tag.ID
                                    where worktag.WorkID == item.WorkID
                                    select tag.MText).Distinct().ToList();

                        var description = "";
                        foreach (var t in tags)
                        {
                            description += string.Format("#{0}#", t);
                        }
                        description += item.Work.Description;

                        image.Description = description;
                        DB.Update(image);
                    }
                }
            }
            return entity;
        }
    }
}
