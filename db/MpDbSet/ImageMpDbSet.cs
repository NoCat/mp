using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class ImageMpDbSet : MpDbSet<Image>
    {
        public ImageMpDbSet(MiaopassContext db) : base(db) { }

        public override Image AddEx(Image entity, bool save = true)
        {
            DB.Transaction(() => {
                Add(entity);
                var package =DB.Packages.Find(entity.PackageID);
                if (package.HasCover == false)
                    package.CoverID = entity.ID;
                package.LastModify = DateTime.Now;
                DB.Packages.UpdateEx(package);
            });
            return entity;
        }
    }
}
