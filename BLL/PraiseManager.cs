using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mp.DAL;

namespace mp.BLL
{
    public class PraiseManager : ManagerBase<Praise>
    {
        public PraiseManager(MiaopassContext db, ManagerCollection collection) : base(db, collection) { }

        public override Praise Add(Praise entity, bool save = true)
        {
            var image = DB.Images.Find(entity.ImageID);
            DB.Transaction(() =>
            {
                image.PraiseCount++;
                DB.Update(image);
                DB.Add(entity);
            });
            return entity;
        }

        public override Praise Remove(Praise entity, bool save = true)
        {
            var image = DB.Images.Find(entity.ImageID);
            DB.Transaction(() =>
            {
                image.PraiseCount--;
                DB.Update(image);
                DB.Remove(entity);
            });
            return entity;
        }
    }
}
