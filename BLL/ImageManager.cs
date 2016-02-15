using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mp.DAL;

using EntityFramework.Extensions;

namespace mp.BLL
{
    public class ImageManager : ManagerBase<Image>
    {
        public ImageManager(MiaopassContext db, ManagerCollection collection) : base(db, collection) { }

        public override Image Add(Image entity, bool save = true)
        {
            var package = Collection.Packages.Find(entity.PackageID);

            DB.Transaction(() =>
            {
                DB.Add(entity);
                UpdatePackage(package, entity);
            });
            return entity;
        }

        public override Image Update(Image entity, bool save = true)
        {
            var package = DB.Packages.Find(entity.PackageID);

            DB.Transaction(() =>
            {
                DB.Update(entity);
                UpdatePackage(package, entity);
            });

            return entity;
        }

        public override Image Remove(Image entity, bool save = true)
        {
            var package = DB.Packages.Find(entity.PackageID);

            DB.Transaction(() =>
            {
                //修改图包信息
                if (package != null && package.CoverID == entity.ID)
                {
                    var coverId = DB.Images.Where(i => i.PackageID == package.ID && i.ID != entity.ID).OrderByDescending(i => i.ID).Select(i => i.ID).FirstOrDefault();
                    package.CoverID = coverId;
                    package.HasCover = false;

                    package.LastModify = DateTime.Now;
                    DB.Update(package);
                }

                //删除关于该图片的赞
                var sql1 = "delete from praise where imageid=?";
                DB.Database.ExecuteSqlCommand(sql1, entity.ID);

                //所有父节点转存数减1
                var sql2 = "update image set resavecount=resavecount-1 where id in (select parent from resave where child=?)";
                DB.Database.ExecuteSqlCommand(sql2, entity.ID);

                //删除所有转存记录
                var sql3 = "delete from resave where parent=? or child=?";
                DB.Database.ExecuteSqlCommand(sql3, entity.ID, entity.ID);

                //删除实体
                DB.Remove(entity);
            });

            return entity;

        }

        public override void AddRange(IEnumerable<Image> entities, bool save = true)
        {
            if (entities.Count() == 0)
                return;

            var packageid = entities.Select(e => e.PackageID).First();
            var package = DB.Packages.Find(packageid);

            DB.Transaction(() =>
            {
                DB.AddRange(entities);
                package.LastModify = DateTime.Now;
                if (package.HasCover == false)
                    package.CoverID = entities.Select(e => e.ID).Last();
                DB.Update(package);
            });
        }

        void UpdatePackage(Package package, Image image)
        {
            if (image.State == ImageStates.Ready)
            {
                if (package.HasCover == false)
                    package.CoverID = image.ID;
                package.LastModify = DateTime.Now;

                DB.Update(package);
            }
        }
    }
}
