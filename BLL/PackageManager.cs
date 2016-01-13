using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntityFramework.Extensions;

using mp.DAL;

namespace mp.BLL
{
    public class PackageManager : ManagerBase<Package>
    {
        public PackageManager(MiaopassContext db, ManagerCollection collection) : base(db, collection) { }

        public override Package Remove(Package entity, bool save = true)
        {
            DB.Transaction(() =>
            {
                var images = DB.Images.Where(i => i.PackageID == entity.ID);

                //删除所有关于图包内图片赞的记录
                DB.Praises.Where(p => images.Select(i => i.ID).Contains(p.ImageID)).Delete();

                //删除所有关于图包内图片的转存记录
                DB.ResaveChains.Where(c => images.Select(i => i.ID).Contains(c.Parent) || images.Select(i => i.ID).Contains(c.Child)).Delete();

                //删除图包内的图片
                images.Delete();

                //删除图包
                DB.Remove(entity);
            });
            return entity;
        }
    }
}
