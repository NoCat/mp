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
                var praise= DB.Praises.Where(p => images.Select(i => i.ID).Contains(p.ImageID));
                DB.RemoveRange(praise,false);

                //删除所有关于图包内图片的转存记录
                var resaveChains= DB.ResaveChains.Where(c => images.Select(i => i.ID).Contains(c.Parent) || images.Select(i => i.ID).Contains(c.Child));
                DB.RemoveRange(resaveChains,false);

                //删除图包内的图片
                DB.RemoveRange(images, false);

                //删除图包
                DB.Remove(entity,false);

                DB.SaveChanges();
            });
            return entity;
        }
    }
}
