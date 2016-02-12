using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mp.DAL;

namespace mp.BLL
{
    public class AdminPixivWorkTagManager : ManagerBase<AdminPixivWorkTag>
    {
        public AdminPixivWorkTagManager(MiaopassContext db, ManagerCollection collection) : base(db, collection) { }

        public override AdminPixivWorkTag Add(AdminPixivWorkTag entity, bool save = true)
        {
            var tag = DB.AdminPixivTags.Find(entity.TagID);
            tag.CitationCount++;
            tag.Weight++;
            Collection.AdminPixivTags.Update(tag);

            var wt = Items.Where(i => i.TagID == entity.TagID && i.WorkID == entity.WorkID).FirstOrDefault();
            if (wt != null)
                return wt;

            return DB.Add(entity, save);
        }
    }
}
