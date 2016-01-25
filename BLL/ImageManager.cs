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
                if (package.HasCover == false)
                    package.CoverID = entity.ID;
                package.LastModify = DateTime.Now;
                
                DB.Update(package);
            });
            return entity;
        }

        public override Image Remove(Image entity, bool save = true)
        {
            var package = Collection.Packages.Find(entity.PackageID);

            DB.Transaction(() =>
            {
                DB.Remove(entity);
                if (package != null && package.CoverID == entity.ID)
                {
                    //修改图包信息
                    var coverId = DB.Images.Where(i => i.PackageID == package.ID).OrderByDescending(i => i.ID).Select(i => i.ID).FirstOrDefault();
                    package.CoverID = coverId;
                    package.HasCover = false;

                    package.LastModify = DateTime.Now;
                    DB.Update(package);
                }

                //删除所有关于该图片的赞记录
                DB.Praises.Where(p => p.ImageID == entity.ID).Delete();

                //所有的父节点转存数减1
                DB.Images.Where(i => DB.ResaveChains.Where(c => c.Child == entity.ID).Select(c => c.Child).Contains(i.ID)).Update(i => new Image { ResaveCount = i.ResaveCount - 1 });

                //删除所有关于该图片的转存记录
                DB.ResaveChains.Where(r => r.Parent == entity.ID || r.Child == entity.ID).Delete();

            });
            return entity;

        }

        public override void AddRange(IEnumerable<Image> entities, bool save = true)
        {
            if (entities.Count() == 0)
                return;

            var packageid = entities.Select(e => e.PackageID).First();
            var package = DB.Packages.Find(packageid);

            DB.Transaction(() => {
                DB.AddRange(entities);
                package.LastModify = DateTime.Now;
                if (package.HasCover == false)
                    package.CoverID = entities.Select(e => e.ID).Last();
                DB.Update(package);
            });
        }
    }
}
