using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mp.DAL;

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

                Collection.Packages.Update(package);
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
                    var coverId = DB.Images.Where(i => i.PackageID == package.ID).OrderByDescending(i => i.ID).Select(i => i.ID).FirstOrDefault();
                    package.CoverID = coverId;
                    if (package.HasCover)
                        package.HasCover = false;

                    package.LastModify = DateTime.Now;

                    Collection.Packages.Update(package);
                }
            });
            return entity;

        }
    }
}
